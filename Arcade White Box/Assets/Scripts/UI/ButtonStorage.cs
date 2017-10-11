using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStorage : MonoBehaviour {

    public PlaceableObject objectAssignedToThisButton;

    public PlaceableObject ObjectAssignedToThisButton
    {
        get
        {
            return objectAssignedToThisButton;
        }

        set
        {
            objectAssignedToThisButton = value;
        }
    }

    // Use this for initialization
    void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
