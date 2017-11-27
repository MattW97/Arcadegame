using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStat
{
    public enum Stats {Hunger, Bladder, Happiness, Hygiene, Queasiness}

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

    public float StatValue { get; set; }
    public float Susceptibility { get; set; }
}
