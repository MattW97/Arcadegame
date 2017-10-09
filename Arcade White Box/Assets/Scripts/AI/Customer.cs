using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : BaseAI
{   
    public enum CustomerState { Idle, GetFood, UseToilet, Wander, Leaving }

    private CustomerState currentState;
    private int age;
    private float excitementLvl, angerLvl;
    private CustomerNeed[] customerNeeds;
    private Unit unitController;
    private CustomerText customerText;

    void Start()
    {   
        unitController = GetComponent<Unit>();
        customerText = GetComponent<CustomerText>();

        // -- CREATING & ADDING NEEDS TO ARRAY --
        customerNeeds = new CustomerNeed[3];

        customerNeeds[0] = new CustomerNeed(CustomerNeed.NeedType.Food, Random.Range(0.0f, 50.0f));
        customerNeeds[1] = new CustomerNeed(CustomerNeed.NeedType.Toilet, Random.Range(0.0f, 50.0f));
        customerNeeds[2] = new CustomerNeed(CustomerNeed.NeedType.Excitement, Random.Range(0.0f, 50.0f));
        // --------------------------------------

        GetNextState();
    }

    private CustomerState GetNextState()
    {
        CustomerNeed need = GetHightestNeed();

        if((int)need.Need == (int)CustomerNeed.NeedType.Food)
        {
            print("Customer - " + Name1 + "s' current highest need is FOOD - " + need.NeedValue);
            customerText.ShowFoodText();
            return CustomerState.GetFood;
        }
        else if((int)need.Need == (int)CustomerNeed.NeedType.Toilet)
        {
            print("Customer - " + Name1 + "s' current highest need is TOILET - " + need.NeedValue);
            customerText.ShowToiletText();
            return CustomerState.UseToilet;
        }
        else if((int)need.Need == (int)CustomerNeed.NeedType.Excitement)
        {
            print("Customer - " + Name1 + "s' current highest need is EXCITEMENT - " + need.NeedValue);
            customerText.ShowExcitementText();
            return CustomerState.UseToilet;
        }
        else
        {
            print("NO NEED COULD BE FOUND FOR: " + Name1);
            return CustomerState.Idle;
        }
    }

    private CustomerNeed GetHightestNeed()
    {
        CustomerNeed greatestNeed = new CustomerNeed(CustomerNeed.NeedType.Test, 0.0f);

        foreach(CustomerNeed need in customerNeeds)
        {
            if(need.NeedValue > greatestNeed.NeedValue)
            {
                greatestNeed = need;
            }
        }

        return greatestNeed;
    }

    private void HungerIncrease()
    {

    }

    private void ToiletIncrease()
    {

    }

    public CustomerState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }

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

    public float AngerLvl
    {
        get
        {
            return angerLvl;
        }

        set
        {
            angerLvl = value;
        }
    }
}
