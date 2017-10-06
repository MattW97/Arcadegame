using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : BaseAI {

    public enum StaffType { Engineer, Builder, Vendor, Janitor, InspectionGuy, Entertainment}

    [SerializeField] private StaffType staffType; // which type of staff

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
