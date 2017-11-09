using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject startMenu, gameModeMenu, careerMenu, sandboxMenu, loadMenu, optionsMenu, arcadeName;

    [SerializeField]
    private Text arcadeNameTextField;

    public Object sceneToSwitchTo;

    private SettingManager settings;

    public string newName;

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

    public void CloseOptions()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
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
        GameManager.Instance.GetComponent<SaveAndLoadManager>().saveData.stats.arcadeName = arcadeNameTextField.text;
        GameManager.Instance.GetComponent<SaveAndLoadManager>().CreateBaseSave();
       // SceneManager.LoadScene(sceneToSwitchTo.name);
        SceneManager.LoadScene(3);
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
