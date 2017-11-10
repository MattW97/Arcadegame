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
    private Transform customerTransform;
    private Transform spawnLocation;
    private IEnumerator usingFacilityWait;
    private CustomerManager customerManager;
    private EconomyManager economyManager;
    private Machine machineNeed;

    public string prefabName;

    void OnEnable()
    {
        EventManager.Save += OnSave;
    }

    void OnDisable()
    {
        EventManager.Save -= OnSave;
    }

    void Start()
    {   
        unitController = GetComponent<Unit>();
        customerTransform = GetComponent<Transform>();
        economyManager = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();

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
        if (currentState == CustomerState.Idle)
        {
            CustomerNeed customerNeed = GetHightestNeed();

            if (customerNeed.NeedValue >= needLimit)
            {
                highestNeed = customerNeed;

                if ((int)customerNeed.Need == (int)CustomerNeed.NeedType.Food)
                {
                    machineNeed = FindNearestFacility(customerManager.GetFoodStalls());

                    DropTrash();
                }
                else if ((int)customerNeed.Need == (int)CustomerNeed.NeedType.Toilet)
                {
                    machineNeed = FindNearestFacility(customerManager.GetToilets());
                }
                else if ((int)customerNeed.Need == (int)CustomerNeed.NeedType.Excitement)
                {
                    machineNeed = FindNearestFacility(customerManager.GetGameMachines());
                }

                if(machineNeed != null)
                {
                    unitController.SetTarget(machineNeed.GetUsePosition());
                    unitController.GetNewPath();
                    machineNeed.InUse = true;
                    currentState = CustomerState.Moving;
                }
            }
        }
        else if(currentState == CustomerState.Moving)
        {   
            if(unitController.ReachedTarget)
            {   
                usingFacilityWait = UsingFacilitiesWait(machineNeed.UseTime);
                economyManager.MoneyEarnedFromArcade(machineNeed);
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
            return null;
        }

        Machine nearest = null;

        facilities.Sort(delegate(Machine a, Machine b){ return Vector3.Distance(customerTransform.position, a.transform.position).CompareTo(Vector3.Distance(customerTransform.position, b.transform.position)); });

        foreach(Machine facility in facilities)
        {
            if(!facility.InUse)
            {
                nearest = facility;
                break;
            }
        }

        return nearest;
    }

    private IEnumerator UsingFacilitiesWait(float waitTime)
    {
        usingFacility = true;
        highestNeed.NeedValue = 0.0f;
        customerTransform.LookAt(machineNeed.transform);
        currentState = CustomerState.UsingFacility;
        yield return new WaitForSeconds(waitTime / speedFactor);
        currentState = CustomerState.Idle;
        machineNeed.InUse = false;
        usingFacility = false;
    }

    private void DropTrash()
    {
        GameObject dropped = Instantiate(customerManager.GetTrash(), customerTransform.position, Quaternion.identity);
        customerManager.AddToDroppedTrash(dropped);
    }

    public void SetCustomerNeeds(float foodNeed, float toiletNeed, float exciteNeed)
    {
        customerNeeds = new CustomerNeed[3];

        customerNeeds[0] = new CustomerNeed(CustomerNeed.NeedType.Food, foodNeed);
        customerNeeds[1] = new CustomerNeed(CustomerNeed.NeedType.Toilet, toiletNeed);
        customerNeeds[2] = new CustomerNeed(CustomerNeed.NeedType.Excitement, exciteNeed);
    }

    public void SetManager(CustomerManager customerManager)
    {
        this.customerManager = customerManager;
    }

    private CustomerSaveable GetCustomerSaveable()
    {
        CustomerSaveable customerSave = new CustomerSaveable();
        customerSave.foodNeed = this.customerNeeds[0].NeedValue;
        customerSave.toiletNeed = this.customerNeeds[1].NeedValue;
        customerSave.excitementNeed = this.customerNeeds[2].NeedValue;
        customerSave.PosX = this.customerTransform.position.x;
        customerSave.PosY = this.customerTransform.position.y;
        customerSave.PosZ = this.customerTransform.position.z;
        customerSave.RotX = this.customerTransform.rotation.x;
        customerSave.RotY = this.customerTransform.rotation.y;
        customerSave.RotZ = this.customerTransform.rotation.z;
        customerSave.prefabName = this.prefabName;
        return customerSave;
    }

    private void OnSave()
    {
        GameManager.Instance.GetComponent<SaveAndLoadManager>().saveData.customerSaveList.Add(GetCustomerSaveable());
    }
}

[System.Serializable]
public class CustomerSaveable
{
    public string prefabName;
    public float toiletNeed, excitementNeed, foodNeed;
    public float PosX, PosY, PosZ;
    public float RotX, RotY, RotZ;
}
