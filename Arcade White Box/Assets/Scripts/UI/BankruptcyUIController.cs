using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BankruptcyUIController : MonoBehaviour {

    [SerializeField]
    private GameObject bankruptcyParent, bankruptcyWarning, bankrupt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnShowStatsButtonPressed()
    {
        // show stats
    }

    public void OnCloseButtonPressed()
    {
        bankruptcyParent.SetActive(false);
        print("Close");
    }

    public void OnBackToMainMenuButtonPressed()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Warning()
    {
        bankruptcyWarning.SetActive(true);
        bankrupt.SetActive(false);
    }

    public void Bankrupt()
    {
        bankruptcyWarning.SetActive(false);
        bankrupt.SetActive(true);
    }
}
