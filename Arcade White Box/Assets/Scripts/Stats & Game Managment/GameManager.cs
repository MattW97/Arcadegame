using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public GameObject levelManagerLink;


    // Use this for initialization
    void Awake ()
    {
        Instance = this;
        levelManagerLink = GameObject.Find("Level Manager");

        //DontDestroyOnLoad(this.gameObject);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject LevelManagerLink
    {
        get
        {
            return levelManagerLink;
        }
    }
}
