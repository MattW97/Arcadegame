﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : BaseAI
{
    [SerializeField]
    private CustomerStat.Stats weakness;

    public string prefabName;

    public enum CustomerStates { Idle, GotTarget, Moving, UsingFacility, Leaving, Wandering }

    private int weakStat;
    private float speedFactor, statCounter;
    private CustomerStates currentState;
    private IEnumerator usingFacilityWait;
    private Transform customerTransform;
    private Transform spawnLocation;
    private Unit unitController;
    private Machine currentTarget;
    private List<CustomerStat> customerStats;

    private CustomerStat bladderStat;
    private CustomerStat happinessStat;
    private CustomerStat hungerStat;
    private CustomerStat hygieneStat;
    private CustomerStat queasinessStat;

    private const float STAT_LIMIT = 100.0f;            //THE LIMIT A STAT CAN REACH
    private const float STAT_TICK_AMOUNT = 0.1f;        //THE AMOUNT THE STAT INCREASES WITH EACH TICK
    private const float STAT_TICK_RATE = 0.5f;          //HOW OFTEN THE STAT TICKS (IN SECONDS)

    void OnEnable() { EventManager.Save += OnSave; }

    void OnDisable() { EventManager.Save -= OnSave; }

    void Awake()
    {
        unitController = GetComponent<Unit>();
        customerTransform = GetComponent<Transform>();

        // INITIALISE ALL STATS, FIND WEAKNESS AND AJUST WEAKNESS VALUES -----------------
        bladderStat = new CustomerStat(CustomerStat.Stats.Bladder, 50.0f, 0.0f);
        happinessStat = new CustomerStat(CustomerStat.Stats.Happiness, 50.0f, 0.0f);
        hungerStat = new CustomerStat(CustomerStat.Stats.Hunger, 50.0f, 0.0f);
        hygieneStat = new CustomerStat(CustomerStat.Stats.Hygiene, 50.0f, 0.0f);
        queasinessStat = new CustomerStat(CustomerStat.Stats.Queasiness, 50.0f, 0.0f);

        customerStats = new List<CustomerStat>();
        customerStats.Add(bladderStat);
        customerStats.Add(happinessStat);
        customerStats.Add(hungerStat);
        customerStats.Add(hygieneStat);
        customerStats.Add(queasinessStat);

        for (int i = 0; i < customerStats.Count; i++)
        {
            if (customerStats[i].GetStatType() == weakness)
            {
                customerStats[i].StatValue = 100.0f;
                customerStats[i].Susceptibility = 15.0f;
                weakStat = i;
            }
        }
        // -------------------------------------------------------------------------------

        statCounter = 0.0f;
        currentState = CustomerStates.Idle;
    }

    void Update()
    {
        unitController.SpeedFactor = speedFactor;

        if (currentState == CustomerStates.GotTarget)
        {
            unitController.SetTarget(currentTarget.GetUsePosition());
            unitController.GetNewPath();
            currentState = CustomerStates.Moving;
        }
        else if (currentState == CustomerStates.Moving)
        {
            if (unitController.ReachedTarget)
            {
                currentState = CustomerStates.UsingFacility;
                usingFacilityWait = UsingFacilityWait(currentTarget.UseTime);
                StartCoroutine(usingFacilityWait);
            }
        }
        else if (currentState == CustomerStates.Leaving)
        {
            if (unitController.ReachedTarget)
            {
                Destroy(gameObject);
            }
        }
        else if (currentState == CustomerStates.Wandering)
        {

        }
    }

    public void TickStats()
    {
        statCounter += Time.deltaTime * speedFactor;

        if (statCounter >= STAT_TICK_RATE)
        {
            foreach (CustomerStat stat in customerStats)
            {
                if (stat.StatValue < STAT_LIMIT)
                {
                    stat.StatValue += STAT_TICK_AMOUNT;
                }
                else if (stat.StatValue > STAT_LIMIT)
                {
                    stat.StatValue = STAT_LIMIT;
                }
            }

            statCounter = 0.0f;
        }
    }

    public void SetNewTarget(Machine newTarget)
    {
        this.currentTarget = newTarget;
        currentState = CustomerStates.GotTarget;
    }

    public void LeaveArcade()
    {
        if (currentState == CustomerStates.UsingFacility)
        {
            StopCoroutine(usingFacilityWait);
        }

        unitController.StopCurrentPathing();
        unitController.SetTarget(spawnLocation);
        unitController.GetNewPath();

        currentState = CustomerStates.Leaving;
    }

    public void DropTrash()
    {

    }

    public void Puke()
    {

    }

    public bool IsBusy()
    {
        if (currentState == CustomerStates.Leaving || currentState == CustomerStates.Moving || currentState == CustomerStates.UsingFacility || currentState == CustomerStates.GotTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Tile GetTileBelow()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(new Vector3(customerTransform.position.x, customerTransform.position.y + 1, customerTransform.position.z), -customerTransform.up, out hitInfo))
        {
            if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
            {
                return hitInfo.collider.GetComponent<Tile>();
            }
            else
            {
                print("THERE IS SOMETHING ELSE UNDER THIS CUSTOMER, SOMETINGZ FUCKD");
                return null;
            }
        }
        else
        {
            print("THERE IS NOTHING BELOW THIS CUSTOMER, SOMETINGZ FUCKD");
            return null;
        }
    }

    private IEnumerator UsingFacilityWait(float waitTime)
    {
        if (currentTarget is GameMachine)
        {
            GameMachine machine = (GameMachine)currentTarget;
            machine.IncreaseCurrentNumberOfPlayers();
        }

        currentTarget.InUse = true;
        currentTarget.OnUse();

        customerTransform.LookAt(currentTarget.transform);
        customerTransform.position = currentTarget.GetUsePosition().position;

        yield return new WaitForSeconds(waitTime / speedFactor);

        currentTarget.InUse = false;

        if (currentTarget is GameMachine)
        {
            GameMachine machine = (GameMachine)currentTarget;
            machine.DecreaseCurrentNumberOfPlayers();
        }

        currentState = CustomerStates.Idle;
    }

    public void SetCustomerNeeds(float bladder, float happiness, float hunger, float hygiene, float queasiness, int weakStat)
    {
        bladderStat = new CustomerStat(CustomerStat.Stats.Bladder, bladder, 0.0f);
        happinessStat = new CustomerStat(CustomerStat.Stats.Happiness, happiness, 0.0f);
        hungerStat = new CustomerStat(CustomerStat.Stats.Hunger, hunger, 0.0f);
        hygieneStat = new CustomerStat(CustomerStat.Stats.Hygiene, hygiene, 0.0f);
        queasinessStat = new CustomerStat(CustomerStat.Stats.Queasiness, queasiness, 0.0f);

        customerStats = new List<CustomerStat>();
        customerStats.Add(bladderStat);
        customerStats.Add(happinessStat);
        customerStats.Add(hungerStat);
        customerStats.Add(hygieneStat);
        customerStats.Add(queasinessStat);

        customerStats[weakStat].Susceptibility = 15.0f;
    }

    public void SetSpawnLocation(Transform spawnLocation) { this.spawnLocation = spawnLocation; }

    public float SpeedFactor { get { return speedFactor; } set { speedFactor = value; } }

    public float BladderStat        { get { return bladderStat.StatValue; } set { bladderStat.StatValue = value; } }
    public float HappinessStat      { get { return happinessStat.StatValue; } set { happinessStat.StatValue = value; } }
    public float HungerStat         { get { return hungerStat.StatValue; } set { hungerStat.StatValue = value; } }
    public float HygieneStat        { get { return hygieneStat.StatValue; } set { hygieneStat.StatValue = value; } }
    public float QueasinessStat     { get { return queasinessStat.StatValue; } set { queasinessStat.StatValue = value; } }

    public List<CustomerStat> GetCustomerStats() { return customerStats; }

    private CustomerSaveable GetCustomerSaveable()
    {
        CustomerSaveable customerSave = new CustomerSaveable();

        customerSave.bladderStat = bladderStat.StatValue;
        customerSave.happinessStat = happinessStat.StatValue;
        customerSave.hungerStat = hungerStat.StatValue;
        customerSave.hygieneStat = hygieneStat.StatValue;
        customerSave.queasinessStat = queasinessStat.StatValue;

        customerSave.weakStat = this.weakStat;

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

    public float    hungerStat,
                    bladderStat,
                    happinessStat,
                    hygieneStat,
                    queasinessStat;

    public int weakStat;

    public float PosX, PosY, PosZ;
    public float RotX, RotY, RotZ;
}
