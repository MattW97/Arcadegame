using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
