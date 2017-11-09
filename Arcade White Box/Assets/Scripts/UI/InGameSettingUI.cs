using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider masterSlider, musicSlider, sfxSlider;

    [SerializeField]
    private Toggle fullscreen;

    [SerializeField]
    private Dropdown resolutionDropDown;

    [SerializeField]
    public AudioSource gameAudioSource;

    private SoundManager soundManager;


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
        gameAudioSource = Instantiate(gameAudioSource);
        soundManager.setGameAudioSource(gameAudioSource);

        GameManager.Instance.SettingManager.LoadSettings();
        GameManager.Instance.SoundManager.InGameAudio();

        MusicSlider.value = GameManager.Instance.SettingManager.GetMusicVolume();
        gameAudioSource.volume = MusicSlider.value;
        fullscreen.isOn = GameManager.Instance.SettingManager.GetIfFullscreen();
        fullscreen.isOn = fullscreen.isOn;
        resolutionDropDown.value = GameManager.Instance.SettingManager.GetResolutionIndex();

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
        print(GameManager.Instance.SettingManager);
        GameManager.Instance.SettingManager.LoadSettings();
        resolutionDropDown.RefreshShownValue();
    }

    void Update()
    {
        GameManager.Instance.SoundManager.InGameAudio();
    }

    public void ChangeMusicVolume()
    {
        GameManager.Instance.SettingManager.SetMusicVolume(MusicSlider.value);
        gameAudioSource.volume = MusicSlider.value;
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
    public void CancelSettings()
    {
        GameManager.Instance.SettingManager.LoadSettings();
        print(gameAudioSource.volume);

    }
}
