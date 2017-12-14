﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : BaseAI
{
    public enum StaffType { Tech, Janitor, Worker, Chef}

    [SerializeField] public StaffType staffType;
    [SerializeField] private float hireCost, wageCost;

    protected float speedFactor;
    protected Unit unitController;
    protected StaffManager staffManager;

    protected virtual void Awake()
    {
        unitController = GetComponent<Unit>();
        staffManager = GameManager.Instance.ScriptHolderLink.GetComponent<StaffManager>();
    }

    protected virtual void Update()
    {
        speedFactor = staffManager.GetSpeedFactor();
        unitController.SpeedFactor = speedFactor;
    }

    public float HireCost
    {
        get
        {
            return hireCost;
        }

        set
        {
            hireCost = value;
        }
    }

    public float WageCost
    {
        get
        {
            return wageCost;
        }

        set
        {
            wageCost = value;
        }
    }

}
