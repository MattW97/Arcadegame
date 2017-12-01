using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{

    [SerializeField]
    private GameObject audioUnselected, graphicsUnselected, cameraUnselected, interfaceUnselected, audioSelected, graphicsSelected,
                       cameraSelected, interfaceSelected, audioPanel, graphicsPanel, cameraPanel, interfacePanel, optionsPanel, pausePanel, backPanel;

    private bool tabOpen;
    private float volume;
    private float value;

    void Start()
    {
        audioSelected.SetActive(true);
        graphicsSelected.SetActive(false);
        cameraSelected.SetActive(false);
        interfaceSelected.SetActive(false);
    }

    public void AudioTab()
    {
        ////////////////////// Panels ///////////////////////////////

        audioPanel.SetActive(true);
        graphicsPanel.SetActive(false);
        cameraPanel.SetActive(false);
        interfacePanel.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        audioUnselected.SetActive(false);
        graphicsUnselected.SetActive(true);
        cameraUnselected.SetActive(true);
        interfaceUnselected.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        audioSelected.SetActive(true);
        graphicsSelected.SetActive(false);
        cameraSelected.SetActive(false);
        interfaceSelected.SetActive(false);

    }

    public void GraphicsTab()
    {
        ////////////////////// Panels ///////////////////////////////

        audioPanel.SetActive(false);
        graphicsPanel.SetActive(true);
        cameraPanel.SetActive(false);
        interfacePanel.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        audioUnselected.SetActive(true);
        graphicsUnselected.SetActive(false);
        cameraUnselected.SetActive(true);
        interfaceUnselected.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        audioSelected.SetActive(false);
        graphicsSelected.SetActive(true);
        cameraSelected.SetActive(false);
        interfaceSelected.SetActive(false);

    }

    public void CameraTab()
    {
        ////////////////////// Panels ///////////////////////////////

        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        cameraPanel.SetActive(true);
        interfacePanel.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        audioUnselected.SetActive(true);
        graphicsUnselected.SetActive(true);
        cameraUnselected.SetActive(false);
        interfaceUnselected.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        audioSelected.SetActive(false);
        graphicsSelected.SetActive(false);
        cameraSelected.SetActive(true);
        interfaceSelected.SetActive(false);

    }

    public void InterfaceTab()
    {
        ////////////////////// Panels ///////////////////////////////

        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        cameraPanel.SetActive(false);
        interfacePanel.SetActive(true);

        //////////////////// Half Tabs /////////////////////////////

        audioUnselected.SetActive(true);
        graphicsUnselected.SetActive(true);
        cameraUnselected.SetActive(true);
        interfaceUnselected.SetActive(false);

        //////////////////// Full Tabs /////////////////////////////

        audioSelected.SetActive(false);
        graphicsSelected.SetActive(false);
        cameraSelected.SetActive(false);
        interfaceSelected.SetActive(true);

    }

    public void ActivatePauseMenu()
    {
        backPanel.SetActive(true);
        pausePanel.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main Menu 1");
    }

    public void DeactivatePauseMenu()
    {
        backPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void ActivateAllMenus()
    {
        //backPanel.SetActive(true);
        optionsPanel.SetActive(true);
        pausePanel.SetActive(false);
        DefaultSettings();
    }

    public void DefaultSettings()
    {
        value = this.GetComponent<InGameSettingUI>().MusicSlider.value;
    }

    public void LoadDefaultSettings()
    {
        this.GetComponent<InGameSettingUI>().MusicSlider.value = value;
    }

    public void DeactivateAllMenus()
    {
        //backPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
}