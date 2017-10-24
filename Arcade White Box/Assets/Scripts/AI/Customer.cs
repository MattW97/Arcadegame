using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : BaseAI
{   
    [SerializeField] private float needsTickAmount, needsTickRate, needLimit;

    private enum CustomerState { Idle, Moving, UsingFacility, Leaving }

    private bool usingFacility;
    private float needsCounter, speedFactor;
    private CustomerState currentState;
    private CustomerNeed[] customerNeeds;
    private CustomerNeed highestNeed;
    private Unit unitController;
    private CustomerText customerText;
    private Transform customerTransform, doorPosition;
    private Transform spawnLocation;
    private IEnumerator usingFacilityWait;
    private CustomerManager customerManager;
    private Machine machineNeed;

    void Start()
    {   
        unitController = GetComponent<Unit>();
        customerText = GetComponent<CustomerText>();
        customerTransform = GetComponent<Transform>();

        customerNeeds = new CustomerNeed[3];

        customerNeeds[0] = new CustomerNeed(CustomerNeed.NeedType.Food, Random.Range((needsTickRate * 2.0f) + 1.0f, needLimit - (needsTickRate * 2.0f)));
        customerNeeds[1] = new CustomerNeed(CustomerNeed.NeedType.Toilet, Random.Range((needsTickRate * 2.0f) + 1.0f, needLimit - (needsTickRate * 2.0f)));
        customerNeeds[2] = new CustomerNeed(CustomerNeed.NeedType.Excitement, Random.Range((needsTickRate * 2.0f) + 1.0f, needLimit - (needsTickRate * 2.0f)));

        customerNeeds[Random.Range(0, customerNeeds.Length)].NeedValue = needLimit;

        needsCounter = needsTickAmount;
        usingFacility = false;
        currentState = CustomerState.Idle;
    }

    void Update()
    {
        if(currentState == CustomerState.Idle)
        {
            CustomerNeed customerNeed = GetHightestNeed();

            if (customerNeed.NeedValue >= needLimit)
            {
                highestNeed = customerNeed;

                if ((int)customerNeed.Need == (int)CustomerNeed.NeedType.Food)
                {
                    machineNeed = FindNearestFacility(customerManager.GetFoodStalls());
                    unitController.SetTarget(machineNeed.GetUsePosition());
                }
                else if ((int)customerNeed.Need == (int)CustomerNeed.NeedType.Toilet)
                {
                    machineNeed = FindNearestFacility(customerManager.GetToilets());
                    unitController.SetTarget(machineNeed.GetUsePosition());
                }
                else if ((int)customerNeed.Need == (int)CustomerNeed.NeedType.Excitement)
                {
                    machineNeed = FindNearestFacility(customerManager.GetGameMachines());
                    unitController.SetTarget(machineNeed.GetUsePosition());
                }

                unitController.GetNewPath();
                currentState = CustomerState.Moving;
            }
        }
        else if(currentState == CustomerState.Moving)
        {   
            if(unitController.ReachedTarget)
            {   
                usingFacilityWait = UsingFacilitiesWait(machineNeed.UseTime);
                StartCoroutine(usingFacilityWait);
            }
        }
        else if(currentState == CustomerState.Leaving)
        {
            if (unitController.ReachedTarget)
            {   
                Destroy(gameObject);
            }
        }

        NeedsTick();

        speedFactor = customerManager.GetSpeedFactor();
        unitController.SpeedFactor = speedFactor;
    }

    public void LeaveArcade()
    {   
        if(usingFacility)
        {
            StopCoroutine(usingFacilityWait);
        }

        unitController.StopCurrentPathing();
        unitController.SetTarget(spawnLocation);
        unitController.GetNewPath();

        currentState = CustomerState.Leaving;
    }

    public void SetSpawnLocation(Transform spawnLocation)
    {
        this.spawnLocation = spawnLocation;
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
        needsCounter -= Time.deltaTime * speedFactor;
        
        if(needsCounter <= 0.0f)
        {
            foreach (CustomerNeed need in customerNeeds)
            {
                need.NeedValue += needsTickRate * speedFactor;
            }

            needsCounter = needsTickAmount;
        }
    }

    private Machine FindNearestFacility(List<Machine> facilities)
    {
        if (facilities.Count == 0)
        {
            print("THERE ARE NO TOILETS YA NONCE!");
            return null;
        }

        Machine nearest = facilities[0];

        foreach(Machine facility in facilities)
        {
            if(Vector3.Distance(customerTransform.position, facility.GetComponent<Transform>().position) <= (Vector3.Distance(customerTransform.position, nearest.GetComponent<Transform>().position)))
            {
                nearest = facility;
            }
        }

        return nearest;
    }

    private IEnumerator UsingFacilitiesWait(float waitTime)
    {
        usingFacility = true;
        highestNeed.NeedValue = 0.0f;
        currentState = CustomerState.UsingFacility;
        yield return new WaitForSeconds(waitTime / speedFactor);
        currentState = CustomerState.Idle;
        usingFacility = false;
    }

    public void SetManager(CustomerManager customerManager)
    {
        this.customerManager = customerManager;
    }
}
