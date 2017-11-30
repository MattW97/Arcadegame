using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStat
{
    public enum Stats {Hunger, Bladder, Happiness, Tiredness, Queasiness}

    private Stats statType;
    private float statValue;
    private float susceptibility;

    public CustomerStat() {}

    public CustomerStat(Stats type, float value, float susceptibility)
    {
        this.statType = type;
        this.statValue = value;
        this.susceptibility = susceptibility;
    }
    
    public Stats GetStatType() { return statType; }

    public float StatValue      { get { return statValue; } set { statValue = value; } }
    public float Susceptibility { get { return susceptibility; } set { susceptibility = value; } }
}
