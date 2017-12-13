using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider masterSlider, musicSlider, sfxSlider;

    [SerializeField]
    private Toggle fullscreen, musicMuteToggle;

    [SerializeField]
    private Dropdown resolutionDropDown, textureQualityDropdown, antialiasingDropdown, vSyncDropdown;

    [SerializeField]
    public AudioSource gameAudioSource;

    private SoundManager soundManager;

    private float masterValue;
    private float musicValue;
    private float sfxValue;
    private bool masterMute;
    private bool musicMute;
    private bool sfxMute;
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

    public Slider MasterSlider
    {
        get
        {
            return masterSlider;
        }

        set
        {
            masterSlider = value;
        }
    }

    public Slider SfxSlider
    {
        get
        {
            return sfxSlider;
        }

        set
        {
            sfxSlider = value;
        }
    }
       

    void Awake()
    {
        soundManager = GameManager.Instance.SoundManager;
        gameAudioSource = Instantiate(gameAudioSource);
        soundManager.setGameAudioSource(gameAudioSource);

        GameManager.Instance.SettingManager.LoadSettings();
        //StartCoroutine(GameManager.Instance.SoundManager.InGameMusic());
        //GameManager.Instance.SoundManager.InGameMusic();

        MasterSlider.value = GameManager.Instance.SettingManager.GetMasterVolume();
        MusicSlider.value = GameManager.Instance.SettingManager.GetMusicVolume();
        SfxSlider.value = GameManager.Instance.SettingManager.GetSfxVolume();
        gameAudioSource.volume = MusicSlider.value;

        musicMuteToggle.isOn = GameManager.Instance.SettingManager.GetMusicMute();
        musicMuteToggle.isOn = musicMuteToggle.isOn;
        MusicMute();
        
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
        GameManager.Instance.SettingManager.LoadSettings();

        resolutionDropDown.RefreshShownValue();
    }

    void Update()
    {
        //GameManager.Instance.SoundManager.InGameMusic();
        StartCoroutine(GameManager.Instance.SoundManager.InGameMusic());
    }
    public void ChangeMasterVolume()
    {
       //Master Vol here
    }

    public void ChangeMusicVolume()
    {
        GameManager.Instance.SettingManager.SetMusicVolume(MusicSlider.value);
        gameAudioSource.volume = MusicSlider.value;
    }

    public void MusicMute()
    {
        GameManager.Instance.SettingManager.SetMusicMute(musicMuteToggle.isOn);

        if (musicMuteToggle.isOn)
        {
            musicSlider.interactable = false;
            gameAudioSource.volume = 0;
        }

        else
        {
            musicSlider.interactable = true;
            gameAudioSource.volume = musicSlider.value;
        }
    }

    public void ChangeSfxVolume()
    {
        GameManager.Instance.SettingManager.SetSfxVolume(SfxSlider.value);
        //gameAudioSource.volume = MusicSlider.value;
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
        MasterSlider.value = masterValue;
        MusicSlider.value = musicValue;
        SfxSlider.value = sfxValue;
        musicMuteToggle.isOn = musicMute;
        fullscreen.isOn = fullscreenOn;
        resolutionDropDown.value = resolutionOption;
        textureQualityDropdown.value = textureOption;
        antialiasingDropdown.value = antialiasingOption;
        vSyncDropdown.value = vSyncOption;
    }

    public void DefaultSettings()
    {
        masterValue = GameManager.Instance.SettingManager.GetMasterVolume();
        musicValue = GameManager.Instance.SettingManager.GetMusicVolume();
        sfxValue = GameManager.Instance.SettingManager.GetSfxVolume();
        musicMute = GameManager.Instance.SettingManager.GetMusicMute();
        fullscreenOn = GameManager.Instance.SettingManager.GetIfFullscreen();
        resolutionOption = GameManager.Instance.SettingManager.GetResolutionIndex();
        textureOption = GameManager.Instance.SettingManager.GetTextureQuality();
        antialiasingOption = GameManager.Instance.SettingManager.GetAnitaliasing();
        vSyncOption = GameManager.Instance.SettingManager.GetVSync();
    }
}
