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
        _saveLoadManagerLink.SaveData("UI Test");
    }

    public void OnLoadButtonPressed()
    {
        _saveLoadManagerLink.LoadData("UI Test");
    }
}
