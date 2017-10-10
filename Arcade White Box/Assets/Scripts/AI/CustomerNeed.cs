using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNeed
{
    public enum NeedType { Test, Food, Toilet, Excitement }

    private NeedType need;
    private float needValue;

    public CustomerNeed(NeedType type, float value)
    {
        Need = type;
        needValue = value;
    }

    public float NeedValue
    {
        get
        {
            return needValue;
        }

        set
        {
            needValue = value;
        }
    }

    public NeedType Need
    {
        get
        {
            return need;
        }

        set
        {
            need = value;
        }
    }
}
