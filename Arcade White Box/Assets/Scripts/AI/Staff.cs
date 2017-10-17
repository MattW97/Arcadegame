using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : BaseAI
{
    public enum StaffType { Tech, Janitor, Worker, Chef}

    [SerializeField] private StaffType staffType;
    [SerializeField] private float hireCost, wageCost;

    private Unit unitController;

    void Awake()
    {
        unitController = GetComponent<Unit>();
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
