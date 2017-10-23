using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    [SerializeField] private GameObject sceneManager, gridManager, economyManager;
    [SerializeField] Scene mainScene;

    private GameObject sceneManagerLink, gridManagerLink;
    private SettingManager settingManager;
    private SoundManager soundManager;
    private string sceneName;


    void Awake()
    {
        Instance = this;

        SettingManager = GetComponent<SettingManager>();

        SoundManager = GetComponent<SoundManager>();
    }

    void OnEnable()
    { 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene mainScene, LoadSceneMode mode)
    {
        // MAIN SCENE LOADING
        if (mainScene.name == "Main Scene")
        {
            Initialise();
            this.gameObject.GetComponent<SaveAndLoadManager>().Initialise();
        }
    }

    private void Initialise()
    {
        SceneManagerLink = Instantiate(sceneManager);
        GridManagerLink = Instantiate(gridManager);
    }

    public GameObject SceneManagerLink
    {
        get
        {
            return SceneManagerLink1;
        }

        set
        {
            SceneManagerLink1 = value;
        }
    }

    public string SceneName
    {
        get
        {
            return sceneName;
        }

        set
        {
            sceneName = value;
        }
    }

    public GameObject GridManagerLink
    {
        get
        {
            return GridManagerLink1;
        }

        set
        {
            GridManagerLink1 = value;
        }
    }

    public SettingManager SettingManager
    {
        get
        {
            return settingManager;
        }

        set
        {
            settingManager = value;
        }
    }

    public SoundManager SoundManager
    {
        get
        {
            return soundManager;
        }

        set
        {
            soundManager = value;
        }
    }

    public GameObject SceneManagerLink1
    {
        get
        {
            return sceneManagerLink;
        }

        set
        {
            sceneManagerLink = value;
        }
    }

    public GameObject GridManagerLink1
    {
        get
        {
            return gridManagerLink;
        }

        set
        {
            gridManagerLink = value;
        }
    }

}
