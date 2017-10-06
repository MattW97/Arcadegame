using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour {

    [SerializeField] private Text arcadeName;

    private PlayerManager _playerLink;

	// Use this for initialization
	void Start () {

        _playerLink = GameObject.Find("Game Manager").GetComponent<PlayerManager>();
        arcadeName.text = _playerLink.ArcadeName;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
