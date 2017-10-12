using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SettingManager : MonoBehaviour
{
    private Resolution[] resolutions;
    private GameSettings gameSettings;


    void Awake()
    { 
        gameSettings = new GameSettings();

        LoadSettings();

        resolutions = Screen.resolutions;

    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gameSettings.json", jsonData);
    }

    public void LoadSettings()
    {
        File.ReadAllText(Application.persistentDataPath + "/gamesettings.json");
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gameSettings.json"));

    }

    public Resolution[] GetResolutionList()
    {
        return resolutions;
    }

    public void SetMusicVolume(float newMusicVolume)
    {
        gameSettings.musicVolume = newMusicVolume;
    }

    public void SetResolution(int newResolution)
    {
        gameSettings.resolutionIndex = newResolution;
    }

    public void SetFullscreen(bool ifFullscreen)
    {
        gameSettings.fullscreen = ifFullscreen;
    }

    public float GetMusicVolume()
    {
        return gameSettings.musicVolume;
    }

    public int GetResolutionIndex()
    {
        return gameSettings.resolutionIndex;
    }

    public bool GetIfFullscreen()
    {
        return gameSettings.fullscreen;
    }
}
