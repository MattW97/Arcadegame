using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject startMenu, gameModeMenu, careerMenu, sandboxMenu, 
                       loadMenu, optionsMenu, soundOptions, visualOptions, arcadeName;

    [SerializeField]
    private GameObject mainCamera, idleCamera;

    [SerializeField]
    private Text arcadeNameTextField;    

    public Object sceneToSwitchTo;

    private SettingManager settings;

    public CameraZoom cameraZoom;

    public ScreenFade screenFade;

    public string newName;

    public float idleTime = 10.0f; // Time till idle in seconds
    private float timeOutTimer = 0.0f;

    void Start()
    {
        startMenu.SetActive(true);
        gameModeMenu.SetActive(false);
        careerMenu.SetActive(false);
        sandboxMenu.SetActive(false);
        loadMenu.SetActive(false);
        optionsMenu.SetActive(false);
        visualOptions.SetActive(false);
        soundOptions.SetActive(false);
    }

    public void Update()
    {
        timeOutTimer += Time.deltaTime;
        CameraIdle();
    }

    public void CameraIdle()
    {
        if (!Input.anyKey && !(Input.GetAxis("Mouse X") > 0) && !(Input.GetAxis("Mouse Y") > 0))
        {
            if (timeOutTimer > idleTime)
            {
                idleCamera.SetActive(true);
                mainCamera.SetActive(false);
            }
            
        }
        else
        {
            timeOutTimer = 0.0f;
            idleCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }

    public void Play()
    {
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

    public void BackToModeSelection()
    {
        gameModeMenu.SetActive(true);
        careerMenu.SetActive(false);
        sandboxMenu.SetActive(false);
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

    public void BuildSandboxWorld()
    {
        cameraZoom.Zoom();
        screenFade.FadeOut();
        StartCoroutine(SandBoxBuild());
    }

    IEnumerator SandBoxBuild()
    {   
        yield return new WaitForSeconds(2);
        GameManager.Instance.GetComponent<SaveAndLoadManager>().saveData.stats.arcadeName = arcadeNameTextField.text;
        GameManager.Instance.GetComponent<SaveAndLoadManager>().CreateBaseSave();
        SceneManager.LoadScene("Level 3");
    }

    //public void SandboxBuild()
    //{
    //    GameManager.Instance.GetComponent<SaveAndLoadManager>().saveData.stats.arcadeName = arcadeNameTextField.text;
    //    GameManager.Instance.GetComponent<SaveAndLoadManager>().CreateBaseSave();
    //   // SceneManager.LoadScene(sceneToSwitchTo.name);
    //    SceneManager.LoadScene("Level 2");
    //}

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
