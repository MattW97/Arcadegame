using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcadeNameUpdate : MonoBehaviour {

    [SerializeField]
    private Text arcadeTextField;

	// Use this for initialization
	void Start () {
        Invoke("UpdateName", 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateName()
    {
        arcadeTextField.text = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>().ArcadeName;
    }
}
