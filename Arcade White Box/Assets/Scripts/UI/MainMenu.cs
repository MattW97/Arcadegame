using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject startMenu, gameModeMenu, careerMenu, sandboxMenu, loadMenu, optionsMenu, soundOptions, visualOptions, arcadeName;

    [SerializeField]
    private Text arcadeNameTextField;

    [SerializeField]
    private Text char1, char2, char3, char4, char5, char6, char7, char8, char9, char10, char11, char12, char13, char14, char15;

    public Object sceneToSwitchTo;

    private SettingManager settings;

    public string newName;

    public string newArcadeName;

    void Start()
    {
        startMenu.SetActive(true);
    }

    public void Play()
    {
        //SceneManager.LoadScene(1);
        startMenu.SetActive(false);
        gameModeMenu.SetActive(true);

    }

    public void Load()
    {
        startMenu.SetActive(false);
        loadMenu.SetActive(true);
    }

    public void Options()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void SoundOptions()
    {
        optionsMenu.SetActive(false);
        soundOptions.SetActive(true);
    }
    public void VisualOptions()
    {
        optionsMenu.SetActive(false);
        visualOptions.SetActive(true);
    }

    public void BackOptions()
    {
        optionsMenu.SetActive(true);
        soundOptions.SetActive(false);
        visualOptions.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Career()
    {
        gameModeMenu.SetActive(false);
        careerMenu.SetActive(true);
    }
    
    public void SandBox()
    {
        gameModeMenu.SetActive(false);
        sandboxMenu.SetActive(true);
    }
    public void SandboxBuild()
    {
        newArcadeName = (char1.text + char2.text + char3.text + char4.text + char5.text + char6.text + char7.text + char8.text + char9.text + char10.text + char11.text + char12.text + char13.text + char14.text + char15.text);
        GameManager.Instance.GetComponent<SaveAndLoadManager>().saveData.stats.arcadeName = newArcadeName;
        GameManager.Instance.GetComponent<SaveAndLoadManager>().CreateBaseSave();
       // SceneManager.LoadScene(sceneToSwitchTo.name);
        SceneManager.LoadScene("Level 2");
    }

    public void ReturnToMain()
    {
        startMenu.SetActive(true);
        gameModeMenu.SetActive(false);
        careerMenu.SetActive(false);
        sandboxMenu.SetActive(false);
        loadMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

}
