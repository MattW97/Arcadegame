using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour {

    [SerializeField]
    private Text arcadeName, cashText, populationText, happinessText;

    private PlayerManager _playerLink;
    private EconomyManager _economyLink;
    private CustomerManager _customerManagerLink;

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
}
