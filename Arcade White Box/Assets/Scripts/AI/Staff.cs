using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : BaseAI
{
    public enum StaffType { Engineer, Builder, Vendor, Janitor, InspectionGuy, Entertainment}

    [SerializeField] private StaffType staffType;
    [SerializeField] private float hireCost, wageCost;

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
