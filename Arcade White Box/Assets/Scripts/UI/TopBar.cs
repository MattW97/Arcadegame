using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour {

    [SerializeField]
    private Text arcadeName, cashText;

    private PlayerManager _playerLink;
    private EconomyManager _economyLink;

	// Use this for initialization
	void Start ()
    {
        _playerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
        _economyLink = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        arcadeName.text = _playerLink.ArcadeName;
        
	}
	
	// Update is called once per frame
	void Update () {
        cashText.text = _economyLink.CurrentCash.ToString();
	}
}
