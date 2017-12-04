using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerNumberUpdater : MonoBehaviour {

    [SerializeField]
    private Text textField;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       textField.text = GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>().GetNumberOfCustomers().ToString();
		
	}
}
