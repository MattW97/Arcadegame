using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour {

    [SerializeField] private Text arcadeName;

    private PlayerManager _playerLink;

	// Use this for initialization
	void Start ()
    {
        print("TOP BAR");
        _playerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
        //arcadeName.text = _playerLink.ArcadeName;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
