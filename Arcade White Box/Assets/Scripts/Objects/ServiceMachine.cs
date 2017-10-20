using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceMachine : Machine {

    public enum ServiceMachineType { ATM, PrizeCollection, VendingMachine, Toilet }
    [SerializeField] private ServiceMachineType machineType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
