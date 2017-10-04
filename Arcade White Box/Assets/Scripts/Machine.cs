using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : PlaceableObject {

    [SerializeField] private float runningCost;  //amount deducted each day
    [SerializeField] private float maintenanceCost; //amount deducted upon machine breaking and needing repairs
    [SerializeField] private int percentReturnedUponSold; //PERCENTAGE amount returned upon being sold back to the manufacturer. 
    [SerializeField] private string description; // description of the machine Optional

    public float RunningCost
    {
        get
        {
            return runningCost;
        }

        set
        {
            runningCost = value;
        }
    }

    public float MaintenanceCost
    {
        get
        {
            return maintenanceCost;
        }

        set
        {
            maintenanceCost = value;
        }
    }

    public int PercentReturnedUponSold
    {
        get
        {
            return percentReturnedUponSold;
        }

        set
        {
            percentReturnedUponSold = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
