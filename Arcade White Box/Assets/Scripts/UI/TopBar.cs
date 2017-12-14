using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class TopBar : MonoBehaviour {

    [SerializeField]
    private Text arcadeName, cashText, populationText, happinessText;

    private PlayerManager _playerLink;
    private EconomyManager _economyLink;
    private CustomerManager _customerManagerLink;

    string folderPath = "/Screenshots";

    // Use this for initialization
    void Start ()
    {
        _playerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
        _economyLink = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        _customerManagerLink = GameManager.Instance.ScriptHolderLink.GetComponent<CustomerManager>();
        arcadeName.text = _playerLink.ArcadeName;
        
	}
	
	// Update is called once per frame
	void Update () {
        cashText.text = "£" + _economyLink.CurrentCash.ToString();
        populationText.text = _customerManagerLink.GetNumberOfCustomers().ToString();
        happinessText.text = Mathf.Round(_customerManagerLink.GetAverageCustomerHappiness()).ToString() + " / 100";
	}

    //public void InGameScreenshot()
    //{
    //    string date = System.DateTime.Now.ToShortDateString().ToString();
    //    date = date.Replace("/", "-");
    //    date = date.Replace(" ", "_");
    //    date = date.Replace(":", "-");

    //    if (!System.IO.Directory.Exists(Application.persistentDataPath + folderPath))
    //        System.IO.Directory.CreateDirectory(Application.persistentDataPath + folderPath);

    //    Application.CaptureScreenshot(Application.persistentDataPath + folderPath + date);


    //    //string date = System.DateTime.Now.ToShortDateString().ToString();
    //    //date = date.Replace("/", "-");
    //    //date = date.Replace(" ", "_");
    //    //date = date.Replace(":", "-");

    //    //Application.CaptureScreenshot("Screenshot_" + System.DateTime.Now.ToShortDateString().ToString() + ".png");


    //    ////Application.CaptureScreenshot(Application.dataPath + "/ScreenShots/SS_" + date + ".png");

    //    ////Application.CaptureScreenshot("Screenshot.png " + System.DateTime.Now.ToShortDateString());
    //    //print(Application.dataPath + "/ScreenShots/SS_" + date + ".png");
    //}
}
