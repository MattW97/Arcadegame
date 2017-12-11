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

        resolutions = Screen.resolutions;
        if (File.Exists(Application.persistentDataPath + "/gameSettings.json"))
        {
            LoadSettings();
        }
        else
        {
            string jsonData = JsonUtility.ToJson(gameSettings, true);
            File.WriteAllText(Application.persistentDataPath + "/gameSettings.json", jsonData);
        }
        
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

    /////////////////////// - Sets - ////////////////////////////////

    public void SetMusicVolume(float newMusicVolume)
    {
        gameSettings.musicVolume = newMusicVolume;
    }

    public void SetMenuEffectsVolume(float newSoundEffectVolume)
    {
        gameSettings.menuEffectVolume = newSoundEffectVolume;
    }

    public void SetResolution(int newResolution)
    {
        gameSettings.resolutionIndex = newResolution;
    }

    public void SetTextureQuality(int textureQuality)
    {
        gameSettings.textureQuality = textureQuality;
    }

    public void SetAntialiasing(int antialiasing)
    {
        gameSettings.antialiasing = antialiasing;
    }
    public void SetVSync(int vSync)
    {
        gameSettings.vSync = vSync;
    }
    public void SetFullscreen(bool ifFullscreen)
    {
        gameSettings.fullscreen = ifFullscreen;
    }

    /////////////////////// - Gets - ////////////////////////////////
    
    public float GetMusicVolume()
    {
        return gameSettings.musicVolume;
    }

    public int GetResolutionIndex()
    {
        return gameSettings.resolutionIndex;
    }

    public int GetTextureQuality()
    {
        return gameSettings.textureQuality;
    }

    public int GetAnitaliasing()
    {
        return gameSettings.antialiasing;
    }

    public int GetVSync()
    {
        return gameSettings.vSync;
    }

    public bool GetIfFullscreen()
    {
        return gameSettings.fullscreen;
    }
}
