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
    private Dropdown resolutionDropDown;

    [SerializeField]
    private AudioSource menuMusic;

    void Awake()
    {
        GameManager.Instance.SettingManager.LoadSettings();

        resolutionDropDown.RefreshShownValue();

        musicSlider.value = GameManager.Instance.SettingManager.GetMusicVolume();
        menuMusic.volume = musicSlider.value;
        fullscreen.isOn = GameManager.Instance.SettingManager.GetIfFullscreen();
        fullscreen.isOn = fullscreen.isOn;
        resolutionDropDown.value = GameManager.Instance.SettingManager.GetResolutionIndex();

        Resolution[] resolutions = GameManager.Instance.SettingManager.GetResolutionList();

        foreach (Resolution resolution in resolutions)
        {
            resolutionDropDown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
    }

    public void ChangeMusicVolume()
    {
        GameManager.Instance.SettingManager.SetMusicVolume(musicSlider.value);
        menuMusic.volume = musicSlider.value;
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

    public void ApplySettings()
    {
        GameManager.Instance.SettingManager.SaveSettings();
    }
}
