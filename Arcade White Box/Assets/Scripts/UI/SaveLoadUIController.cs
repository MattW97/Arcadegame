﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadUIController : MonoBehaviour {

    private SaveAndLoadManager _saveLoadManagerLink;

	// Use this for initialization
	void Start () {
        _saveLoadManagerLink = GameManager.Instance.GetComponent<SaveAndLoadManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSaveButtonPressed()
    {
        GameManager.Instance.GetComponent<SaveAndLoadManager>().UpdateGameData();
        _saveLoadManagerLink.SaveStat("UI Test");
        _saveLoadManagerLink.SaveScene("Scene test");
    }

    public void OnLoadButtonPressed()
    {
        _saveLoadManagerLink.LoadStats("UI Test");
        _saveLoadManagerLink.LoadScene("Scene test");
    }
}
