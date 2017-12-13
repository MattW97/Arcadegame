using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : PlaceableObject {

    public enum MachineStatus { Working, Broken }

    [SerializeField] private float useTime;                                 // Time it takes to use object, i.e. buy/eat food, use toilet, play game (in Seconds)
    [SerializeField] private float runningCost;                             // amount deducted each day
    [SerializeField] private float useCost, minUseCost, maxUseCost;          // how much it costs a user to use the machine. Includes playing if the machine is a game machine, also min and max use cost.
    [SerializeField] private float maintenanceCost;                         // amount deducted upon machine breaking and needing repair
    [SerializeField] private Transform usePosition;                         // The position the customers moves to when using the object
    [SerializeField] private float failurePercentage;                       // The chance the machine has to break on use
    [SerializeField] private float failurePercentageIncrease;               // How much the failurePercentage increases by each IncreaseFailurePercentage() call.
    [SerializeField] [Range(0.0f, 100.0f)]protected float statAdjustment;   // Customer stat boost OnUse() (Stat increased dependent on the machine this is attached to)b 

    public MachineStatus machineStatus;
    private bool inUse;
    private float baseFailurePercentage;

    public float dailyRevenue, allTimeRevenue, dailyExpenses, allTimeExpenses;
    public int dailyCustomers, allTimeCustomers, noOfBreakdowns;

    private void IncreaseFailurePercentage()
    {
        failurePercentage = failurePercentage + failurePercentageIncrease;
    }

    /// <summary>
    /// When the machine is used.
    /// </summary>
    public virtual void OnUse()
    {
        dailyRevenue += UseCost;
        allTimeRevenue += UseCost;
        dailyCustomers++;
        allTimeCustomers++;
        int roll = Random.Range(1, 100);
        if (roll <= failurePercentage)
        {
            machineStatus = MachineStatus.Broken;
            OnMachineBreak();
        }
    }

    /// <summary>
    /// When the machine is repaired
    /// </summary>
    public void OnRepair()
    {
        if (GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().CheckCanAfford(maintenanceCost))
        {
            GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().OnMachineBreakdown(this);
            dailyExpenses += maintenanceCost;
            allTimeExpenses += maintenanceCost;
            machineStatus = MachineStatus.Working;
            failurePercentage = baseFailurePercentage;
        }
        else
        { 
         //return some kind of error, idk what, tell the player they are poor af
        }
    }

    public virtual void DailyReset()
    {
        dailyExpenses = 0;
        dailyRevenue = 0;
        dailyCustomers = 0;
        IncreaseFailurePercentage();
    }

    protected override void Awake()
    {
        base.Awake();
        machineStatus = MachineStatus.Working;
    }

    void Start()
    {
        baseFailurePercentage = failurePercentage;
    }


    protected override void Update()
    {
        base.Update();
        
    }

    protected void OnMachineBreak()
    {
        // play broken animation stuff
        noOfBreakdowns++;
    }
    protected bool IncreaseUseCost()
    {
        if (UseCost != MaxUseCost)
        {
            UseCost++;
            return true;
        }
        else
        return false;

    }

    protected bool DecreaseUseCost()
    {
        if (UseCost != MinUseCost)
        {
            UseCost--;
            return true;
        }
        else
            return false;
    }

    public bool IsUsable()
    {
        if (!inUse && machineStatus != MachineStatus.Broken)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Getters/Setters

    public float GetStatAdjustment()
    {
        return statAdjustment;
    }

    public float RunningCost
    {
        get
        {
            return runningCost;
        }

        set
        {
            runningCost = value;
        }
    }

    public float MaintenanceCost
    {
        get
        {
            return maintenanceCost;
        }

        set
        {
            maintenanceCost = value;
        }
    }

    public float UseCost
    {
        get
        {
            return useCost;
        }

        set
        {
            useCost = value;
        }
    }

    public bool InUse
    {
        get
        {
            return inUse;
        }

        set
        {
            inUse = value;
        }
    }

    public float UseTime
    {
        get
        {
            return useTime;
        }

        set
        {
            useTime = value;
        }
    }

    public float MinUseCost
    {
        get
        {
            return minUseCost;
        }

        set
        {
            minUseCost = value;
        }
    }

    public float MaxUseCost
    {
        get
        {
            return maxUseCost;
        }

        set
        {
            maxUseCost = value;
        }
    }

    public Transform GetUsePosition()
    {
        return usePosition;
    }
    #endregion Getters/Setters
}
