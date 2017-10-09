using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private const string FILE_EXTENSION = ".xml";

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
    /// Finds the value associated with the flag
    /// </summary>
    /// <param name="flagName"></param>
    /// <returns></returns>
    public int GetFlag(string flagName)
    {
        GameFlag flag = gameData.gameFlags.Find(x => x.flag == flagName);

        // Create Non-existant flags but default to 0
        if (flag == null)
        {
            SetFlag(flagName, 0);
            return 0;
        }

        return flag.value;
    }

    /// <summary>
    /// Checks if a particular level has been cleared yet or not
    /// </summary>
    /// <param name="level">Level to check</param>
    /// <returns>True if cleared and false otherwise</returns>
    public bool GetLevelCleared(int level)
    {
        return GetFlag("level" + level + "cleared") == 1 ? true : false;
    }

    /// <summary>
    /// Load game data from file for active use
    /// </summary>
    /// <param name="gameName"></param>
    /// <returns></returns>
    public void LoadGame(string gameName)
    {
        CheckDirectory();

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
            reader.Read(); //skip BOM
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
    /// </summary>
    /// <param name="saveFile"></param>
    public void SaveGame(string saveFile)
    {
        CheckDirectory();
        

        // Update saveFile name
        if (saveFile == null)
        {
            saveFile = GenerateNewSaveName();
        }

        this.saveFile = saveFile;

        // FileStream fs = File.Create(GameDic.Instance.SavePath + saveFile);
        GetValues();
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
    /// Set Current Save Related Information on gameData
    /// </summary>
    /// <param name="saveFile"></param>
    private void UpdateSaveData(string saveFile)
    {
        gameData.lastSaveFile = saveFile;
        gameData.lastSaveTime = DateTime.Now.ToBinary();
    }

    // For flag storing and getting
    public void SetFlag(string flagName, int value)
    {
        // Overwrite Old Key/Values
        GameFlag oldFlag = gameData.gameFlags.Find(x => x.flag == flagName);

        // Either update the value or add a new one if it does not exist
        if (oldFlag != null)
        {
            oldFlag.value = value;
        }
        else
        {
            // Does not exist in list
            gameData.gameFlags.Add(new GameFlag(flagName, value));
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Checks if the file has not yet been created
    /// </summary>
    /// <param name="saveFile"></param>
    /// <returns></returns>
    private bool IsNewFile(string saveFile)
    {
        return !File.Exists(SavePath + saveFile + FILE_EXTENSION);
    }

    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {
        _timeAndCalendarLink = GameManager.Instance.LevelManagerLink.GetComponent<TimeAndCalendar>();
        _playerManagerLink = GameManager.Instance.GetComponent<PlayerManager>();
    }


    /// <summary>
    /// Checks to see if the SavePath directory exists and creates a new one of it does not.
    /// </summary>
    private void CheckDirectory()
    {
        // Check if directory exists, if not create it
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
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
            // Old Binary Formmater Method BinaryFormatter bf = new BinaryFormatter(); FileStream
            // fs = File.Open(fullFilePath, FileMode.Open);

            // Put it into a file PlayerData data = (PlayerData)bf.Deserialize(fs);

            // fs.Close();

            // XML SERIALIZER TEST INSTEAD OF BINARYFORMATTER
            FileStream fs = File.Open(fullFilePath, FileMode.Open);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GameData));
            XmlReader reader = XmlReader.Create(fs);
            GameData data = xmlSerializer.Deserialize(reader) as GameData;
            fs.Close();

            return data;
        }
        else
        {
            Debug.LogError("Failed to save to file " + fullFilePath);
            return null;
        }
    }

    /// <summary>
    /// Make sure that the save / load directory exists.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckDirectory();
    }

    private void GetValues()
    {
      //  gameData.arcadeName = _playerManagerLink.ArcadeName;
      //  gameData.playerName = _playerManagerLink.PlayerName;
        gameData.playerMoney = _playerManagerLink.CurrentCash;
    }

    // Assigns data to where it should be after a load
    private void AssignValues(GameData data)
    {
        //TimeAndCalendar timeAndCalendarLink = GameManager.Instance.LevelManagerLink.GetComponent<TimeAndCalendar>();
       // PlayerManager _playerManagerLink = GameManager.Instance.GetComponent<PlayerManager>();
        _timeAndCalendarLink.startDay = data.currentDay;
        _timeAndCalendarLink.startMonth = data.currentMonth;
        _timeAndCalendarLink.startYear = data.currentYear;
        _timeAndCalendarLink.startMinute = data.currentMinute;
        _timeAndCalendarLink.startHour = data.currentHour;

        _playerManagerLink.CurrentCash = data.playerMoney;
        _playerManagerLink.PlayerName = data.playerName;
        _playerManagerLink.ArcadeName = data.arcadeName;
        

    }

    #endregion Private Methods
}

[Serializable]
public class GameFlag
{
    #region Public Fields

    public string flag;
    public int value;

    #endregion Public Fields

    #region Public Constructors

    public GameFlag()
    {
    }

    public GameFlag(string flag, int value)
    {
        this.flag = flag;
        this.value = value;
    }

    #endregion Public Constructors
}

[Serializable]
public class GameData
{
    #region Public Fields


    public List<GameFlag> gameFlags;

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
        playerName = "Jill";
        arcadeName = "Start Arcade";
        currentHour = 12;
        currentMinute = 0;
        currentDay = 1;
        currentMonth = 1;
        currentYear = 2000;

        gameFlags = new List<GameFlag>();
    }

    #endregion Public Constructors

}

