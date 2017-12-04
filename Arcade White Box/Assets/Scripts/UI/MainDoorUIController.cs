using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainDoorUIController : MonoBehaviour {

    [SerializeField] private GameObject startButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.ScriptHolderLink.GetComponent<LevelManager>().CheckCustomerSpawnParameters())
        {
            startButton.SetActive(true);
        }
        else if (!GameManager.Instance.ScriptHolderLink.GetComponent<LevelManager>().CheckCustomerSpawnParameters() && GameManager.Instance.ScriptHolderLink.GetComponent<CustomerManager>().GetNumberOfCustomers() == 0)
        {
            startButton.SetActive(false);
        }
	}
}
