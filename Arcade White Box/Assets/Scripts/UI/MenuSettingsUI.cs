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

    //[SerializeField]
    //private AudioSource menuMusic;

    private SoundManager soundManager;

    void Awake()
    {
        soundManager = GameManager.Instance.SoundManager;
        menuAudioSource = Instantiate(menuAudioSource);
        soundManager.setAudioSource(menuAudioSource);

        GameManager.Instance.SettingManager.LoadSettings();
        GameManager.Instance.SoundManager.MenuAudio();

        musicSlider.value = GameManager.Instance.SettingManager.GetMusicVolume();
        menuAudioSource.volume = musicSlider.value;
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

    public void ChangeMusicVolume()
    {
        GameManager.Instance.SettingManager.SetMusicVolume(musicSlider.value);
        menuAudioSource.volume = musicSlider.value;
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
}
