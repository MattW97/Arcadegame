using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : PlaceableObject {

    [SerializeField] private float runningCost;  //amount deducted each day
    [SerializeField] private float maintenanceCost; //amount deducted upon machine breaking and needing repair
    [SerializeField] private float useCost; //how much it costs a user to use the machine. Includes playing if the machine is a game machine.

    private bool inUse;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

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

    public float UseCost
    {
        get
        {
            return useCost;
        }

        set
        {
            useCost = value;
        }
    }

    public bool InUse
    {
        get
        {
            return inUse;
        }

        set
        {
            inUse = value;
        }
    }
}
