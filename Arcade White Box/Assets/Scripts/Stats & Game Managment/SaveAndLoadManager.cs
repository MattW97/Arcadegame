﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using BayatGames.SaveGameFree;

/// <summary>
/// Class for handling persistent game data
/// </summary>
public class SaveAndLoadManager : MonoBehaviour
{
    public enum AutoSaveDelay { OneMinute, FiveMinute, TenMinute, ThirtyMinute, Hour }
    [SerializeField] private AutoSaveDelay autosaveDelay;

    private TimeAndCalendar _timeAndCalendarLink;
    private PlayerManager _playerManagerLink;
    private EconomyManager _economyManagerLink;
    private ObjectManager _objectManager;
    private GameObject instantiatedObjectParent;
    private GameObject instantiatedCustomerParent;

    public List<PlaceableObjectSaveable> placeableSaveList;
    public List<CustomerSaveable> customerSaveList;
    public GameData gameData = new GameData();
    public SaveableData saveData = new SaveableData();
    public Scene openScene;

    public bool autosaveOn;

    private string savePath;
    public string SavePath
    {
        get
        {
            if (savePath != null)
                return savePath;
            else
            {
                savePath = Application.persistentDataPath + "/SavedGames/";
                return savePath;
            }
        }
    }

    private string autoSaveName = "AutoSave";




    private const string FILE_EXTENSION = ".lul";

    // Save Load Data
    private string saveFile;

   



    /// <summary>
    /// Deletes the save file if it exists and errors out otherwise.
    /// </summary>
    /// <param name="saveFile"></param>
    public void DeleteSaveFile(string saveFile)
    {
        if (File.Exists(SavePath + saveFile + FILE_EXTENSION))
        {
            File.Delete(SavePath + saveFile + FILE_EXTENSION);
        }
        else
        {
            Debug.LogError("Failed to delete non existant file " + SavePath + saveFile + FILE_EXTENSION);
        }
    }

    /// <summary>
    /// Checks if the save file exists in the file system
    /// </summary>
    /// <param name="testFileName"></param>
    /// <returns>True if it exists and false otherwise</returns>
    public bool DoesFileExist(string testFileName)
    {
        foreach (GameData data in GetAllSaveFiles())
        {
            if (data.lastSaveFile == testFileName)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Create a new file name, check for existing files of same player name
    /// </summary>
    /// <returns></returns>
    public string GenerateNewSaveName()
    {
        int attempt = 0;
        string newSaveName = "";

        while (newSaveName == "")
        {
            // Save Name is Player Name
            string checkString = gameData.playerName;

            // Add a number if original already taken
            if (attempt != 0) checkString += attempt;

            if (!File.Exists(SavePath + checkString))
            {
                // Make the check string the new file name
                newSaveName = checkString;
            }

            attempt++;
        }

        return newSaveName;
    }

    /// <summary>
    /// Gets a list of all save files in the save directory.
    /// </summary>
    /// <returns></returns>
    public List<GameData> GetAllSaveFiles()
    {
        List<GameData> allSaves = new List<GameData>();

        // Check Save Path
        foreach (string fileName in Directory.GetFiles(SavePath))
        {
            // Get Player Data for Each File
            allSaves.Add(GetSaveFile(fileName));
        }

        return allSaves;
    }

    public void CreateBaseSave()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SavedGames/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames/");
        string fullSavePath = SavePath + "Base" + FILE_EXTENSION;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;
        if (!File.Exists(fullSavePath))
        {
            saveFile = File.Create(fullSavePath);
        }
        else
        {
            saveFile = File.OpenWrite(fullSavePath);
        }
        formatter.Serialize(saveFile, saveData.stats);

        saveFile.Close();
    }

    /// <summary>
    /// Saves the current instantiation of the GameData class to a binary file.
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <returns></returns>
    public void SaveStats(string saveFileName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SavedGames/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames/");

        string fullSavePath = SavePath + saveFileName + FILE_EXTENSION;


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;

        if (!File.Exists(fullSavePath))
        {
            saveFile = File.Create(fullSavePath);
        }
        else
        {
            saveFile = File.OpenWrite(fullSavePath);
        }
        UpdateGameData();

        formatter.Serialize(saveFile, saveData.stats);

        saveFile.Close();
    }

    /// <summary>
    /// Loads the @param file name and overwrites the current instantiation of the GameData class with the values contained within the save file.
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <returns></returns>
    public void LoadStats(string saveFileName)
    {
        string fullFilePath = SavePath + saveFileName + FILE_EXTENSION;
        if (File.Exists(fullFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(fullFilePath, FileMode.Open);

            saveData.stats = (GameData)formatter.Deserialize(saveFile);
            AssignValues(saveData);

            saveFile.Close();
        }
        else
        {
            Debug.Log("Save file " + fullFilePath + " does not exist!");
        }
    }

    public void SaveScene(string saveFileName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SavedGames/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames/");

        string fullSavePath = SavePath + saveFileName + FILE_EXTENSION;
        UpdateGameData();

        SaveGame.Save<SaveableData>(fullSavePath, saveData);
    }

    public void LoadScene(string saveFileName)
    {
        string fullSavePath = SavePath + saveFileName + FILE_EXTENSION;
        GameManager.Instance.SceneManagerLink.GetComponent<LevelInteraction>().ClearObjectParent();
        GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>().ClearCustomerParent();
        GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>().LoadClearLists();
        saveData = SaveGame.Load<SaveableData>(fullSavePath, saveData);
        AssignValues(saveData);
        instantiatedObjectParent = GameObject.Find("Level/Placed Objects");
        instantiatedCustomerParent = GameObject.Find("Customers");
        CreateObjects();
        CreateCustomers();
        GameManager.Instance.GridManagerLink.GetComponent<GridGeneration>().UpdateGrid();
    }

    private void CreateObjects()
    {
        foreach (PlaceableObjectSaveable obj in saveData.placeableSaveList)
        {
            for (int i = 0; i < _objectManager.AllPlaceableObjects.Count; i++)
            {
                if (obj.prefabName == _objectManager.AllPlaceableObjects[i].name)
                {
                    InstantiateNewObject(_objectManager.AllPlaceableObjects[i], new Vector3(obj.PosX, obj.PosY, obj.PosZ), Quaternion.Euler(new Vector3(obj.RotX, obj.RotY, obj.RotZ)), FindTileFromID(obj.tile_ID));
                }
            }

        }
        placeableSaveList.Clear();
    }

    private void CreateCustomers()
    {
        foreach (CustomerSaveable cust in saveData.customerSaveList)
        {
            for (int i = 0; i < GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>().customers.Length; i++)
            {
                if (cust.prefabName == GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>().customers[i].name)
                {
                       InstantiateNewCustomer(GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>().customers[i], cust, 
                        new Vector3(cust.PosX, cust.PosY, cust.PosZ),
                        Quaternion.Euler(new Vector3(cust.RotX, cust.RotY, cust.RotZ)));

                        
                }
            }
        }
    }

    private Tile FindTileFromID(int tile_ID)
    {
        Tile tile = null; 
        for (int i = 0; i < _objectManager.AllTiles.Length; i++)
        {
            if (tile_ID == _objectManager.AllTiles[i].GetID())
            {
                tile = _objectManager.AllTiles[i];
            }

        }
        return tile;
    }

    private void InstantiateNewObject(PlaceableObject objectToPlace, Vector3 position, Quaternion rotation, Tile objectTile)
    {
        GameObject newObject = Instantiate(objectToPlace.gameObject, position, rotation, instantiatedObjectParent.transform);
        GameManager.Instance.SceneManagerLink.GetComponent<LevelManager>().AddObjectToLists(newObject);
      
        PlaceableObject newPlaceableObject = newObject.GetComponent<PlaceableObject>();

        newPlaceableObject.PlacedOnTile = objectTile;
        newPlaceableObject.prefabName = objectToPlace.name;
        objectTile.SetIfPlacedOn(true);
    }

    private void InstantiateNewCustomer(Customer cust, CustomerSaveable custSave, Vector3 position, Quaternion rotation)
    {
        Customer newCustomer = Instantiate(cust, position, rotation, instantiatedCustomerParent.transform) as Customer;
        newCustomer.SetCustomerNeeds(custSave.foodNeed, custSave.toiletNeed, custSave.excitementNeed);
        newCustomer.prefabName = custSave.prefabName;
        newCustomer.SetManager(GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>());
    }

    public void AutoSaveHandler(bool on)
    {
        if (on)
        {
            if (autosaveDelay == AutoSaveDelay.OneMinute)
            {
                StartCoroutine(AutoSave(60));
            }
            else if (autosaveDelay == AutoSaveDelay.FiveMinute)
            {
                StartCoroutine(AutoSave(360));
            }
            else if (autosaveDelay == AutoSaveDelay.TenMinute)
            {
                StartCoroutine(AutoSave(600));
            }
            else if (autosaveDelay == AutoSaveDelay.ThirtyMinute)
            {
                StartCoroutine(AutoSave(1800));
            }
            else if (autosaveDelay == AutoSaveDelay.Hour)
            {
                StartCoroutine(AutoSave(3600));
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator<UnityEngine.WaitForSeconds> AutoSave(float delay)
    {
        SaveScene(autoSaveName);
        yield return new WaitForSeconds(delay);
        StartCoroutine(AutoSave(delay));
    }
    private void AutoSaveSingular()
    {
        SaveScene(autoSaveName);
    }



    public void UpdateGameData()
    {
        saveData.stats.arcadeName = _playerManagerLink.ArcadeName;
        saveData.stats.playerName = _playerManagerLink.PlayerName;
        saveData.stats.playerMoney = _economyManagerLink.CurrentCash;
        saveData.stats.currentMinute = _timeAndCalendarLink.CurrentMinute;
        saveData.stats.currentHour = _timeAndCalendarLink.CurrentHour;
        saveData.stats.currentDay = _timeAndCalendarLink.CurrentDay;
        saveData.stats.currentMonth = _timeAndCalendarLink.CurrentMonth;
        saveData.stats.currentYear = _timeAndCalendarLink.CurrentYear;
    }

    public void ClearLists()
    {
        saveData.placeableSaveList.Clear();
        saveData.customerSaveList.Clear();
    }

    /// <summary>
    /// Set Current Save Related Information on gameData
    /// </summary>
    /// <param name="saveFile"></param>
    private void UpdateSaveData(string saveFile)
    {
        saveData.stats.lastSaveFile = saveFile;
        saveData.stats.lastSaveTime = DateTime.Now.ToBinary();
    }



    public void Initialise()
    {
        _timeAndCalendarLink = GameManager.Instance.SceneManagerLink.GetComponent<TimeAndCalendar>();
        _playerManagerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
        _economyManagerLink = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        _objectManager = GameManager.Instance.SceneManagerLink.GetComponent<ObjectManager>();
        if (autosaveOn)
        {
            AutoSaveHandler(true);
        }
    }

    // Assigns data to where it should be after a load
    private void AssignValues(SaveableData data)
    {
        _timeAndCalendarLink.CurrentDay = data.stats.currentDay;
        _timeAndCalendarLink.CurrentMonth = data.stats.currentMonth;
        _timeAndCalendarLink.CurrentYear = data.stats.currentYear;
        _timeAndCalendarLink.CurrentMinute = data.stats.currentMinute;
        _timeAndCalendarLink.CurrentHour = data.stats.currentHour;

        _economyManagerLink.CurrentCash = data.stats.playerMoney;
        _playerManagerLink.PlayerName = data.stats.playerName;
        _playerManagerLink.ArcadeName = data.stats.arcadeName;
        

    }

    /// <summary>
    /// Retrieves the data stored inside of a save file
    /// </summary>
    /// <param name="fullFilePath"></param>
    /// <returns></returns>
    private GameData GetSaveFile(string fullFilePath)
    {
        if (File.Exists(fullFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(fullFilePath, FileMode.Open);
            gameData = (GameData)bf.Deserialize(fs);
            fs.Close();
            return gameData;
        }
        else
        {
            Debug.LogError("Failed to save to file " + fullFilePath);
            return null;
        }
    }

    

}



[Serializable]
public class GameData
{
    #region Public Fields
    public float playerMoney;
    public int currentHour, currentMinute;
    public int currentYear, currentMonth, currentDay;
    public string playerName;
    public string arcadeName;

    // Needs properties to access
    [NonSerialized]

    public string lastSaveFile;

    public long lastSaveTime;

    #endregion Public Fields

    #region Public Constructors

    /// <summary>
    /// Default Constructor for New Game - Contains Starting Stats
    /// </summary>
    public GameData()
    {
        playerMoney = 0;
        playerName = "Default_Player_Name";
        arcadeName = "Default_Arcade_Name";
        currentHour = 0;
        currentMinute = 0;
        currentDay = 1;
        currentMonth = 1;
        currentYear = 2000;
    }

    #endregion Public Constructors

}

[Serializable]
public class MachineData
{
    public int prefabID;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
    public int useCost;
}


[Serializable]
public class SaveableData
{
    public List<GameObject> allGameObjects;
    public List<MachineData> allMachineData;
    public List<CustomerSaveable> customerSaveList;
    public GameData stats;
    public List<PlaceableObjectSaveable> placeableSaveList;

}

