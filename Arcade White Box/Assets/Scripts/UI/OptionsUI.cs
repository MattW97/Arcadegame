using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{

    [SerializeField]
    private GameObject audioHalfTab, graphicsHalfTab, cameraHalfTab, interfaceHalfTab, audioFullTab, graphicsFullTab, cameraFullTab, interfaceFullTab, audioPanel, graphicsPanel, cameraPanel, interfacePanel, optionsPanel, masterEnd, musicEnd, sfxEnd; // optionsMenu;

    private bool tabOpen;
    private float volume;
    private float value;

    public void AudioTab()
    {
        ////////////////////// Panels ///////////////////////////////

        audioPanel.SetActive(true);
        graphicsPanel.SetActive(false);
        cameraPanel.SetActive(false);
        interfacePanel.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        audioHalfTab.SetActive(false);
        graphicsHalfTab.SetActive(true);
        cameraHalfTab.SetActive(true);
        interfaceHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        audioFullTab.SetActive(true);
        graphicsFullTab.SetActive(false);
        cameraFullTab.SetActive(false);
        interfaceFullTab.SetActive(false);

    }

    public void GraphicsTab()
    {
        ////////////////////// Panels ///////////////////////////////

        audioPanel.SetActive(false);
        graphicsPanel.SetActive(true);
        cameraPanel.SetActive(false);
        interfacePanel.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        audioHalfTab.SetActive(true);
        graphicsHalfTab.SetActive(false);
        cameraHalfTab.SetActive(true);
        interfaceHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        audioFullTab.SetActive(false);
        graphicsFullTab.SetActive(true);
        cameraFullTab.SetActive(false);
        interfaceFullTab.SetActive(false);

    }

    public void CameraTab()
    {
        ////////////////////// Panels ///////////////////////////////

        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        cameraPanel.SetActive(true);
        interfacePanel.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        audioHalfTab.SetActive(true);
        graphicsHalfTab.SetActive(true);
        cameraHalfTab.SetActive(false);
        interfaceHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        audioFullTab.SetActive(false);
        graphicsFullTab.SetActive(false);
        cameraFullTab.SetActive(true);
        interfaceFullTab.SetActive(false);

    }

    public void InterfaceTab()
    {
        ////////////////////// Panels ///////////////////////////////

        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        cameraPanel.SetActive(false);
        interfacePanel.SetActive(true);

        //////////////////// Half Tabs /////////////////////////////

        audioHalfTab.SetActive(true);
        graphicsHalfTab.SetActive(true);
        cameraHalfTab.SetActive(true);
        interfaceHalfTab.SetActive(false);

        //////////////////// Full Tabs /////////////////////////////

        audioFullTab.SetActive(false);
        graphicsFullTab.SetActive(false);
        cameraFullTab.SetActive(false);
        interfaceFullTab.SetActive(true);

    }

    public void ActivateAllMenus()
    {
        optionsPanel.SetActive(true);
        DefaultSettings();
    }

    public void DefaultSettings()
    {
        value = this.GetComponent<InGameSettingUI>().MusicSlider.value;


        print(value);
        
    }

    public void LoadDefaultSettings()
    {
        this.GetComponent<InGameSettingUI>().MusicSlider.value = value;
    }

    public void DeactivateAllMenus()
    {
        optionsPanel.SetActive(false);
    }

    public void Update()
    {
        if (this.GetComponent<InGameSettingUI>().MusicSlider.value == 1)
        {
            musicEnd.SetActive(true);
        }
        else
        {
            musicEnd.SetActive(false);
        }
    }
}