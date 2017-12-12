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
    public void SetMasterVolume(float newMasterVolume)
    {
        gameSettings.masterVolume = newMasterVolume;
    }

    public void SetMusicVolume(float newMusicVolume)
    {
        gameSettings.musicVolume = newMusicVolume;
    }

    public void SetSfxVolume(float newSfxVolume)
    {
        gameSettings.sfxVolume = newSfxVolume;
    }

    public void SetMasterMute (bool newMasterMute)
    {
        gameSettings.masterMute = newMasterMute;
    }

    public void SetMusicMute (bool newMusicMute)
    {
        gameSettings.musicMute = newMusicMute;
    }

    public void SetSfxMute (bool newSfxMute)
    {
        gameSettings.sfxMute = newSfxMute;
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

    public float GetMasterVolume()
    {
        return gameSettings.masterVolume;
    }

    public float GetMusicVolume()
    {
        return gameSettings.musicVolume;
    }

    public float GetSfxVolume()
    {
        return gameSettings.sfxVolume;
    }

    public bool GetMasterMute()
    {
        return gameSettings.masterMute;
    }

    public bool GetMusicMute()
    {
        return gameSettings.musicMute;
    }

    public bool GetSfxMute()
    {
        return gameSettings.sfxMute;
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
