using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using BayatGames.SaveGameFree;

/// <summary>
/// Class for saving and loading persistent game data and autosaving.
/// </summary>
public class SaveAndLoadManager : MonoBehaviour
{
    public bool autosaveOn;
    public enum AutoSaveDelay { OneMinute, FiveMinute, TenMinute, ThirtyMinute, Hour }
    [SerializeField] private AutoSaveDelay autosaveDelay;

    private TimeAndCalendar _timeAndCalendarLink;
    private PlayerManager _playerManagerLink;
    private EconomyManager _economyManagerLink;
    private ObjectManager _objectManager;
    private GameObject instantiatedObjectParent;
    private GameObject instantiatedCustomerParent;

    //private List<PlaceableObjectSaveable> placeableSaveList;
    private List<CustomerSaveable> customerSaveList;
    private GameData gameData = new GameData();
    public SaveableData saveData = new SaveableData();
    public Scene openScene;


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
        foreach (SaveableData data in GetAllSaveFiles())
        {
            //if (data.lastSaveFile == testFileName)
            //{
            //    return true;
            //}
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
    public List<SaveableData> GetAllSaveFiles()
    {
        List<SaveableData> allSaves = new List<SaveableData>();
        // Check Save Path
        foreach (string fileName in Directory.GetFiles(SavePath))
        {
            // Get Player Data for Each File
            allSaves.Add(GetSaveFile(fileName));
        }
        return allSaves;
    }

    /// <summary>
    /// Creates a base save, almost identical to SaveStats. This is only used when switching from the main menu to a level.
    /// It allows passing the Player Name and Arcade Name entered by the player on the main menu to whichever level they select.
    /// Load this file with the LoadStats function, pass in the variable "Base" to ensure you load the correct file.
    /// </summary>
    public void CreateBaseSave()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SavedGames/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames/");
        string fullSavePath = SavePath + "Base" + FILE_EXTENSION;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;
        if (!File.Exists(fullSavePath))
            saveFile = File.Create(fullSavePath);
        else
            saveFile = File.OpenWrite(fullSavePath);
        formatter.Serialize(saveFile, saveData.stats);
        saveFile.Close();
    }

    /// <summary>
    /// Specifically saves an instantiation of the GameData class inside SaveableData.
    /// Should be used if you need to save things such as player name/cash, but not the level itself.
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
            saveFile = File.Create(fullSavePath);
        else
            saveFile = File.OpenWrite(fullSavePath);
        UpdateSaveableDataStats();
        formatter.Serialize(saveFile, saveData.stats);
        saveFile.Close();
    }

    /// <summary>
    /// Loads the @param file name and overwrites what is currently contained in SaveData.stats. 
    /// Used to load a file saved with the SaveStats and SaveScene functions.
    /// Using this function will not update the scene the player is currently in. Do not use this to load a level.
    /// Also used with the CreateBaseSave function, to allow the passage and saving of player data from the Main Menu scene to any level scene.
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
            Debug.Log("Save file " + fullFilePath + " does not exist!");
    }

    /// <summary>
    /// Saves the current instantiation of the SaveableData class. This will contain all the stats of the player, 
    /// every PlaceableObject object placed in the current scene and every customer as well.
    /// @param The name of the file that is being saved to disk.
    /// </summary>
    public void SaveScene(string saveFileName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SavedGames/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedGames/");
        string fullSavePath = SavePath + saveFileName + FILE_EXTENSION;
        UpdateSaveableDataStats();
        SaveGame.Save<SaveableData>(fullSavePath, saveData);
    }

    /// <summary>
    /// Loads the @param file from disk and overwrites the current instantiation of the SaveableData class.
    /// This function will destroy all machines and customers in the current level, then reinstantiate the ones contained in the @param file from disk.
    /// ANY MACHINE OR CUSTOMER THAT HAS NOT BEEN SAVED BEFORE THIS FUNCTION IS CALLED WILL BE DELETED AND UNRECOVERABLE.
    /// This function will also update the pathfinding grid once it is finished, to allow the AI to navigate the new level layout.
    /// </summary>
    public void LoadScene(string saveFileName)
    {
        string fullSavePath = SavePath + saveFileName + FILE_EXTENSION;
        GameManager.Instance.SceneManagerLink.GetComponent<LevelInteraction>().ClearObjectParent();
        GameManager.Instance.ScriptHolderLink.GetComponent<CustomerManager>().ClearCustomerParent();
        GameManager.Instance.ScriptHolderLink.GetComponent<CustomerManager>().LoadClearLists();
        saveData = SaveGame.Load<SaveableData>(fullSavePath, saveData);
        AssignValues(saveData);
        instantiatedObjectParent = GameObject.Find("Level/Placed Objects");
        instantiatedCustomerParent = GameObject.Find("Customers");
        CreateObjects();
        CreateCustomers();
        GameManager.Instance.PathingGridManagerLink.GetComponent<PathingGridSetup>().UpdateGrid();
    }

    /// <summary>
    /// Instantiates every placeable object that is in the current instantiation of the SaveableData class. 
    /// This function is called from Load Scene.
    /// </summary>
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
        //placeableSaveList.Clear();
    }

    /// <summary>
    /// Instantiates every customer contained inside the current instantiation of the SaveableData class.
    /// This function is called from the LoadScene function.
    /// </summary>
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

    /// <summary>
    /// Finds and returns a tile based on the @param ID from the list inside ObjectManager.
    /// Returns the tile if found, returns null otherwise.
    /// </summary>
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

    /// <summary>
    /// Object instantiation method. Called from CreateObjects(). 
    /// @param The placeableoject to place, a vector 3 containing position, a quaternion containing rotation and the Tile to place on.
    /// </summary>
    private void InstantiateNewObject(PlaceableObject objectToPlace, Vector3 position, Quaternion rotation, Tile objectTile)
    {
        GameObject newObject = Instantiate(objectToPlace.gameObject, position, rotation, instantiatedObjectParent.transform);
        GameManager.Instance.ScriptHolderLink.GetComponent<LevelManager>().AddObjectToLists(newObject);
      
        PlaceableObject newPlaceableObject = newObject.GetComponent<PlaceableObject>();

        newPlaceableObject.PlacedOnTile = objectTile;
        newPlaceableObject.PrefabName = objectToPlace.name;
        objectTile.SetIfPlacedOn(true);
    }

    /// <summary>
    /// Customer instantiation method. Called from CreateCustomers(). 
    /// @param The customer model to load, the customer saveable class to load data from, a Vector3 containing position, a Quaternion containing rotation.
    /// </summary>
    private void InstantiateNewCustomer(Customer cust, CustomerSaveable custSave, Vector3 position, Quaternion rotation)
    {
        Customer newCustomer = Instantiate(cust, position, rotation, instantiatedCustomerParent.transform) as Customer;
        newCustomer.SetCustomerNeeds(custSave.bladderStat, custSave.happinessStat, custSave.hungerStat, custSave.hygieneStat, custSave.queasinessStat, custSave.weakStat);
        newCustomer.prefabName = custSave.prefabName;
    }

    /// <summary>
    /// @param Autosaving bool
    /// Handles the autosaving. Starts a Coroutine if passed true. Stops all coroutines, stopping autosaving if passed false.
    /// Can be used to toggle autosvaing on and off.
    /// This is where autosaving timing changes should be made if necessary.
    /// </summary>
    public void AutoSaveHandler(bool on)
    {
        if (on)
        {
            if (autosaveDelay == AutoSaveDelay.OneMinute)
            {
                StopAllCoroutines();
                StartCoroutine(AutoSave(60));
            }
            else if (autosaveDelay == AutoSaveDelay.FiveMinute)
            {
                StopAllCoroutines();
                StartCoroutine(AutoSave(360));
            }
            else if (autosaveDelay == AutoSaveDelay.TenMinute)
            {
                StopAllCoroutines();
                StartCoroutine(AutoSave(600));
            }
            else if (autosaveDelay == AutoSaveDelay.ThirtyMinute)
            {
                StopAllCoroutines();
                StartCoroutine(AutoSave(1800));
            }
            else if (autosaveDelay == AutoSaveDelay.Hour)
            {
                StopAllCoroutines();
                StartCoroutine(AutoSave(3600));
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    /// <summary>
    /// Autosave function, recurvsive function. 
    /// Calls SaveScene() and passes in the autoSaveName variable. 
    /// Will overwrite the autosave file on every call. Previous autosaves will be lost.
    /// Recursion recalls this method after the delay specified from AutoSaveHandler. 
    /// Do not call this method from anywhere else, if you wish to do a one off AutoSave, use AutoSaveSingular() instead.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator<UnityEngine.WaitForSeconds> AutoSave(float delay)
    {
        SaveScene(autoSaveName);
        yield return new WaitForSeconds(delay);
        StartCoroutine(AutoSave(delay));
    }

    /// <summary>
    /// Performs a singular autosave. Will overwrite the autosave file on every call. Previous autosaves will be lost.
    /// </summary>
    public void AutoSaveSingular()
    {
        SaveScene(autoSaveName);
    }

    /// <summary>
    /// Updates stats subclass of the current instantiation of the SaveableData class with data currently being used by the game for saving.
    /// Will overwrite any data contained in that instantiation.
    /// This function is called in SaveStats() and SaveScene(). There is no harm in calling this method elsewhere, but is unnecessary.
    /// </summary>
    private void UpdateSaveableDataStats()
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

    /// <summary>
    /// Clears all lists inside the current instantiation of the SaveableData class. 
    /// This is to prevent duplication on saving. 
    /// </summary>
    public void ClearLists()
    {
        saveData.placeableSaveList.Clear();
        saveData.customerSaveList.Clear();
    }

    /// <summary>
    /// Class initialisiation. Assigns all the links and starts autosaving if enabled.
    /// </summary>
    public void Initialise()
    {
        _timeAndCalendarLink = GameManager.Instance.ScriptHolderLink.GetComponent<TimeAndCalendar>();
        _playerManagerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
        _economyManagerLink = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        _objectManager = GameManager.Instance.SceneManagerLink.GetComponent<ObjectManager>();
        if (autosaveOn)
        {
            AutoSaveHandler(true);
        }
    }

    /// <summary>
    /// Assigns data after it has been loaded from file. 
    /// @param the data class that has been loaded from file
    /// </summary>
    /// <param name="data"></param>
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
    private SaveableData GetSaveFile(string fullFilePath)
    {
        if (File.Exists(fullFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(fullFilePath, FileMode.Open);
            saveData = (SaveableData)bf.Deserialize(fs);
            fs.Close();
            return saveData;
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
public class SaveableData
{
    public GameData stats;
    public List<CustomerSaveable> customerSaveList;
    public List<PlaceableObjectSaveable> placeableSaveList;

}

