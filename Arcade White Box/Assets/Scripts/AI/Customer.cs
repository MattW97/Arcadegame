using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : BaseAI
{   
    public enum CustomerState { Idle, GetFood, UseToilet, Wander, Leaving }

    [SerializeField] private float needsTickRate;
    [SerializeField] private Transform[] testFacilities;

    private float needsCounter;
    private float foodSensitivity, toiletSensitivity, excitmentSensitivity;
    private CustomerNeed[] customerNeeds;
    private Unit unitController;
    private CustomerText customerText;
    private Transform customerTransform;

    void Start()
    {   
        unitController = GetComponent<Unit>();
        customerText = GetComponent<CustomerText>();
        customerTransform = GetComponent<Transform>();

        // -- CREATING & ADDING NEEDS TO ARRAY --
        customerNeeds = new CustomerNeed[3];

        customerNeeds[0] = new CustomerNeed(CustomerNeed.NeedType.Food, Random.Range(0.0f, 30.0f));
        customerNeeds[1] = new CustomerNeed(CustomerNeed.NeedType.Toilet, Random.Range(0.0f, 30.0f));
        customerNeeds[2] = new CustomerNeed(CustomerNeed.NeedType.Excitement, Random.Range(0.0f, 30.0f));
        // --------------------------------------

        // ----- SETTING SENSITIVITY VALUES -----
        foodSensitivity = Random.Range(1.0f, 10.0f);
        toiletSensitivity = Random.Range(1.0f, 10.0f);
        excitmentSensitivity = Random.Range(1.0f, 10.0f);
        // --------------------------------------

        needsCounter = needsTickRate;

        GetNextState();
    }

    void Update()
    {
        NeedsTick();

        if(Input.GetKeyDown(KeyCode.K))
        {
            unitController.SetTarget(FindNearestFacility(testFacilities));
            unitController.GetNewPath();
        }
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

    private void NeedsTick()
    {
        needsCounter -= Time.deltaTime;
        
        if(needsCounter <= 0.0f)
        {
            // FOOD
            customerNeeds[0].NeedValue += foodSensitivity * 1.1f;
            print("FOOD LVLS - " + customerNeeds[0].NeedValue);

            // TOILET
            customerNeeds[1].NeedValue += toiletSensitivity * 1.1f;
            print("TOILET LVLS - " + customerNeeds[1].NeedValue);

            // EXCITEMENT
            customerNeeds[2].NeedValue += excitmentSensitivity * 1.1f;
            print("EXCITEMENT LVLS - " + customerNeeds[2].NeedValue);

            GetNextState();
            needsCounter = needsTickRate;
        }
    }

    private Transform FindNearestFacility(Transform[] facilities)
    {
        Transform nearest = facilities[0];

        foreach(Transform facility in facilities)
        {
            if(Vector3.Distance(customerTransform.position, facility.position) <= (Vector3.Distance(customerTransform.position, nearest.position)))
            {
                nearest = facility;
            }
        }

        print(nearest.name);
        return nearest;
    }
}
