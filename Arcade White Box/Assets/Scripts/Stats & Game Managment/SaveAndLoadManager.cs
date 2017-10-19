using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Class for handling persistent game data
/// </summary>
public class SaveAndLoadManager : MonoBehaviour
{
    #region Public Fields

    public GameData gameData = new GameData();
    public Scene openScene;

    private TimeAndCalendar _timeAndCalendarLink;
    private PlayerManager _playerManagerLink;

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

    #endregion Public Fields

    #region Private Fields

    private const string FILE_EXTENSION = ".lul";

    // Save Load Data
    private string saveFile;

    #endregion Private Fields

    #region Public Methods

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

    /// <summary>
    /// Load game data from file for active use
    /// THIS IS NO LONGER USED. LEFT IN FOR REFERENCE. CLEANUP TOWARDS FINAL VERSION!
    /// </summary>
    /// <param name="gameName"></param>
    /// <returns></returns>
    public void LoadGame(string gameName)
    {
        // Assemble path to file to load game from
        String fullFilePath = SavePath + gameName + FILE_EXTENSION;

        if (File.Exists(fullFilePath))
        {
            // Put it into a file
            Debug.Log("Deserializing " + fullFilePath);

            FileStream fs = File.Open(fullFilePath, FileMode.Open);

            // Deserialize the XML Save File (Using XmlSerializer instead of BinarySerializer)
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GameData));
            XmlReader reader = XmlReader.Create(fs);
            //reader.Read(); //skip BOM
            gameData = xmlSerializer.Deserialize(reader) as GameData;

            AssignValues(gameData);
            fs.Close();

        }
        else
        {
            Debug.Log("Failed to load file " + fullFilePath);
        }
    }

    /// <summary>
    /// Save all game data to file
    /// THIS IS NO LONGER USED. LEFT IN FOR REFERENCE. CLEANUP TOWARDS FINAL VERSION!
    /// </summary>
    /// <param name="saveFile"></param>
    public void SaveGame(string saveFile)
    {
        //CheckDirectory();
        

        // Update saveFile name
        if (saveFile == null)
        {
            saveFile = GenerateNewSaveName();
        }

        this.saveFile = saveFile;

        //FileStream fs = File.Create(GameDic.Instance.SavePath + saveFile);
        UpdateSaveData(saveFile);

        string fullSavePath = SavePath + saveFile + FILE_EXTENSION;

        FileStream fs;

        // Create a file or open an old one up for writing to
        if (!File.Exists(fullSavePath))
        {
            fs = File.Create(fullSavePath);
        }
        else
        {
            fs = File.OpenWrite(fullSavePath);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        TextWriter textWriter = new StreamWriter(fs);
        serializer.Serialize(textWriter, gameData);
        fs.Close();

        Debug.Log("Game Saved to " + fullSavePath);
    }


    /// <summary>
    /// Saves the current instantiation of the GameData class to a binary file.
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <returns></returns>
    public void SaveData(string saveFileName)
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        string fullSavePath = SavePath + saveFileName + FILE_EXTENSION;


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.lul");
        // Create a file or open an old one up for writing to
        if (!File.Exists(fullSavePath))
        {
            saveFile = File.Create(fullSavePath);
        }
        else
        {
            saveFile = File.OpenWrite(fullSavePath);
        }

        formatter.Serialize(saveFile, gameData);

        saveFile.Close();
    }

    /// <summary>
    /// Loads the @param file name and overwrites the current instantiation of the GameData class with the values contained within the save file.
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <returns></returns>
    public void LoadData(string saveFileName)
    {
        string fullFilePath = SavePath + saveFileName + FILE_EXTENSION;
        if (File.Exists(fullFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(fullFilePath, FileMode.Open);

            gameData = (GameData)formatter.Deserialize(saveFile);
            AssignValues(gameData);

            saveFile.Close();
        }
        else
        {
            Debug.Log("Save file " + fullFilePath + " does not exist!");
        }
    }

    public void UpdateGameData()
    {
        gameData.arcadeName = _playerManagerLink.ArcadeName;
        gameData.playerName = _playerManagerLink.PlayerName;
        gameData.playerMoney = _playerManagerLink.CurrentCash;
        gameData.currentMinute = _timeAndCalendarLink.CurrentMinute;
        gameData.currentHour = _timeAndCalendarLink.CurrentHour;
        gameData.currentDay = _timeAndCalendarLink.CurrentDay;
        gameData.currentMonth = _timeAndCalendarLink.CurrentMonth;
        gameData.currentYear = _timeAndCalendarLink.CurrentYear;
    }

    /// <summary>
    /// Set Current Save Related Information on gameData
    /// </summary>
    /// <param name="saveFile"></param>
    private void UpdateSaveData(string saveFile)
    {
        gameData.lastSaveFile = saveFile;
        gameData.lastSaveTime = DateTime.Now.ToBinary();
    }

    #endregion Public Methods

    #region Private Methods


    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {
    }


    void Start()
    {
        _playerManagerLink = GameManager.Instance.GetComponent<PlayerManager>();
    }

    public void Initialise()
    {
        _timeAndCalendarLink = GameManager.Instance.SceneManagerLink.GetComponent<TimeAndCalendar>();
        _playerManagerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
    }

    // Assigns data to where it should be after a load
    private void AssignValues(GameData data)
    {
        _timeAndCalendarLink.CurrentDay = data.currentDay;
        _timeAndCalendarLink.CurrentMonth = data.currentMonth;
        _timeAndCalendarLink.CurrentYear = data.currentYear;
        _timeAndCalendarLink.CurrentMinute = data.currentMinute;
        _timeAndCalendarLink.CurrentHour = data.currentHour;

        _playerManagerLink.CurrentCash = data.playerMoney;
        _playerManagerLink.PlayerName = data.playerName;
        _playerManagerLink.ArcadeName = data.arcadeName;
        

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

    #endregion Private Methods
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
public class PlacedMachineData
{
    public float PositionX, PositionY, PositionZ;
    public float RotationX, RotationY, RotationZ;

}

