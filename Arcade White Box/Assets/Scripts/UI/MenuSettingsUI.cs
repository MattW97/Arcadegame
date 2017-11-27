using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettingsUI : MonoBehaviour
{
    [SerializeField]
    private Slider masterSlider, musicSlider, sfxSlider;

    [SerializeField]
    private Toggle fullscreen;

    [SerializeField]
    private Dropdown resolutionDropDown, textureQualityDropdown, antialiasingDropdown, vSyncDropdown;

    [SerializeField]
    private AudioSource menuAudioSource;

    private SoundManager soundManager;
    
    private float musicValue;
    private bool fullscreenOn;
    private int resolutionOption;
    private int textureOption;
    private int antialiasingOption;
    private int vSyncOption;

    public Slider MusicSlider
    {
        get
        {
            return musicSlider;
        }

        set
        {
            musicSlider = value;
        }
    }

    void Awake()
    {
        soundManager = GameManager.Instance.SoundManager;
        menuAudioSource = Instantiate(menuAudioSource);
        soundManager.setMenuAudioSource(menuAudioSource);

        GameManager.Instance.SettingManager.LoadSettings();
        GameManager.Instance.SoundManager.MenuAudio();

        MusicSlider.value = GameManager.Instance.SettingManager.GetMusicVolume();
        menuAudioSource.volume = MusicSlider.value;
        fullscreen.isOn = GameManager.Instance.SettingManager.GetIfFullscreen();
        fullscreen.isOn = fullscreen.isOn;
        resolutionDropDown.value = GameManager.Instance.SettingManager.GetResolutionIndex();
        textureQualityDropdown.value = GameManager.Instance.SettingManager.GetTextureQuality();
        antialiasingDropdown.value = GameManager.Instance.SettingManager.GetAnitaliasing();
        vSyncDropdown.value = GameManager.Instance.SettingManager.GetVSync();

        Resolution[] resolutions = GameManager.Instance.SettingManager.GetResolutionList();

        foreach (Resolution resolution in resolutions)
        {
            resolutionDropDown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
    }

    void Start()
    {   
        resolutionDropDown.RefreshShownValue();
    }

    void OnEnable()
    {
        try
        {
            GameManager.Instance.SettingManager.LoadSettings();
        }
        catch (Exception e)
        {
            print("MenuSettingUI threw an error again!");
        }
        resolutionDropDown.RefreshShownValue();
    }

    public void ChangeMusicVolume()
    {
        GameManager.Instance.SettingManager.SetMusicVolume(MusicSlider.value);
        menuAudioSource.volume = MusicSlider.value;
    }

    public void Fullscreen()
    {
        GameManager.Instance.SettingManager.SetFullscreen(fullscreen.isOn);
        Screen.fullScreen = fullscreen.isOn;
    }

    public void OnResolutionChange()
    {   
        Resolution[] resolutions = GameManager.Instance.SettingManager.GetResolutionList();
        Screen.SetResolution(resolutions[resolutionDropDown.value].width, resolutions[resolutionDropDown.value].height, Screen.fullScreen);
        GameManager.Instance.SettingManager.SetResolution(resolutionDropDown.value);        
    }

    public void OnTextureChange()
    {
        QualitySettings.masterTextureLimit = textureQualityDropdown.value;
        GameManager.Instance.SettingManager.SetTextureQuality(textureQualityDropdown.value);
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = (int)Mathf.Pow(2f, antialiasingDropdown.value);
        GameManager.Instance.SettingManager.SetAntialiasing((int)Mathf.Pow(2f, antialiasingDropdown.value));
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = vSyncDropdown.value;
        GameManager.Instance.SettingManager.SetVSync(vSyncDropdown.value);
    }

    public void ApplySettings()
    {
        GameManager.Instance.SettingManager.SaveSettings();
    }
    
    public void CancelSettings()
    {
        MusicSlider.value = musicValue;
        fullscreen.isOn = fullscreenOn;
        resolutionDropDown.value = resolutionOption;
        textureQualityDropdown.value = textureOption;
        antialiasingDropdown.value = antialiasingOption;
        vSyncDropdown.value = vSyncOption;
        print("Res "+ resolutionOption);
        print("Tex " + textureOption);
        print("anti " + antialiasingOption);
        print("vsync " + vSyncOption);
        print("full " + fullscreenOn);
    }

    public void DefaultSettings()
    {
        musicValue = GameManager.Instance.SettingManager.GetMusicVolume();
        fullscreenOn = GameManager.Instance.SettingManager.GetIfFullscreen();
        resolutionOption = GameManager.Instance.SettingManager.GetResolutionIndex();
        textureOption = GameManager.Instance.SettingManager.GetTextureQuality();
        antialiasingOption = GameManager.Instance.SettingManager.GetAnitaliasing();
        vSyncOption = GameManager.Instance.SettingManager.GetVSync();

        print("Res " + resolutionOption);
        print("Tex " + textureOption);
        print("anti " + antialiasingOption);
        print("vsync " + vSyncOption);
        print("full " + fullscreenOn);
    }
}
