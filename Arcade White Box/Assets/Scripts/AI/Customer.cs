using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : BaseAI
{
    private int age, gender;
    private float foodNeed, drinkNeed, toiletNeed, excitementLvl, upsetLvl;
    private Unit pathingControl;

    public int Age
    {
        get
        {
            return age;
        }

        set
        {
            age = value;
        }
    }

    public float DrinkNeed
    {
        get
        {
            return drinkNeed;
        }

        set
        {
            drinkNeed = value;
        }
    }

    public float ExcitementLvl
    {
        get
        {
            return excitementLvl;
        }

        set
        {
            excitementLvl = value;
        }
    }

    public float FoodNeed
    {
        get
        {
            return foodNeed;
        }

        set
        {
            foodNeed = value;
        }
    }

    public int Gender
    {
        get
        {
            return gender;
        }

        set
        {
            gender = value;
        }
    }

    public float ToiletNeed
    {
        get
        {
            return toiletNeed;
        }

        set
        {
            toiletNeed = value;
        }
    }

    public float UpsetLvl
    {
        get
        {
            return upsetLvl;
        }

        set
        {
            upsetLvl = value;
        }
    }
}
