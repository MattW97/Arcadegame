using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour {

    [SerializeField] private Image icon;
    [SerializeField] private string Name;


    public Image Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public string Name1
    {
        get
        {
            return Name;
        }

        set
        {
            Name = value;
        }
    }


    //MOTHERFUCKING BASE CLASS
    //DONT TOUCH FOR FUCKS SAKES MATT

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
