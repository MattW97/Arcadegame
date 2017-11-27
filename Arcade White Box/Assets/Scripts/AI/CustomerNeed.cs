using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNeed
{
    public enum NeedType { Test, Food, Toilet, Excitement, Happiness, Sadness, Puke}

    private NeedType need;
    private float needValue;
    private float needSusceptibility;

    public CustomerNeed() {}

    public CustomerNeed(NeedType type, float value, float susceptibility)
    {
        need = type;
        needValue = value;
        needSusceptibility = susceptibility;
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

    public float NeedSusceptibility
    {
        get
        {
            return needSusceptibility;
        }

        set
        {
            needSusceptibility = value;
        }
    }
}
