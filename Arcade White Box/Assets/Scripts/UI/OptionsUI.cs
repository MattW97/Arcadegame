using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{

    [SerializeField]
    private GameObject audioTab, graphicsTab, cameraTab, interfaceTab, optionsPanel; // optionsMenu;

    private bool tabOpen;
    private float volume;
    private float value;

    public void AudioTab()
    {
        audioTab.SetActive(true);
        graphicsTab.SetActive(false);
        cameraTab.SetActive(false);
        interfaceTab.SetActive(false);

    }

    public void GraphicsTab()
    {
        audioTab.SetActive(false);
        graphicsTab.SetActive(true);
        cameraTab.SetActive(false);
        interfaceTab.SetActive(false);
    }

    public void CameraTab()
    {
        audioTab.SetActive(false);
        graphicsTab.SetActive(false);
        cameraTab.SetActive(true);
        interfaceTab.SetActive(false);
    }

    public void InterfaceTab()
    {
        audioTab.SetActive(false);
        graphicsTab.SetActive(false);
        cameraTab.SetActive(false);
        interfaceTab.SetActive(true);
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
        print("val" + value);
       // print("vol" + volume);
        this.GetComponent<InGameSettingUI>().MusicSlider.value = value;
    }

    public void DeactivateAllMenus()
    {
        optionsPanel.SetActive(false);
    }

    void Update()
    {
        //print(value);
    }
}