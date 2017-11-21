using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : PlaceableObject {

    public enum MachineStatus { Working, Broken }

    [SerializeField] private float useTime;                   // Time it takes to use object, i.e. buy/eat food, use toilet, play game (in Seconds)
    [SerializeField] private float runningCost;               // amount deducted each day
    [SerializeField] private float defaultUseCost;            // how much it costs a user to use the machine. Includes playing if the machine is a game machine.
    [SerializeField] private float maintenanceCost;           // amount deducted upon machine breaking and needing repair
    [SerializeField] private Transform usePosition;           // The position the customers moves to when using the object
    [SerializeField] private float failurePercentage;         // The chance the machine has to break on use
    [SerializeField] private float failurePercentageIncrease; // How much the failurePercentage increases by each IncreaseFailurePercentage() call.

    [SerializeField] [Range(0, 1000)] private int minimumUseCost;
    [SerializeField] [Range(0, 1000)] private int maximumUseCost;

    private MachineStatus machineStatus;
    private bool inUse;

    private int numberOfCustomersToday;
    private int numberOfCustomersEver;

    private float amountEarnedToday;
    private float amountEarnedEver;

    private float maintainenceToday;
    private float maintainenceEver;

    private float expensesEver;



    private void IncreaseFailurePercentage()
    {
        failurePercentage = failurePercentage + failurePercentageIncrease;
    }

  /// <summary>
  /// When the machine is used.
  /// </summary>
    protected virtual void OnUse()
    {
        NumberOfCustomersToday++;
        NumberOfCustomersEver++;
        AmountEarnedToday += defaultUseCost;
        AmountEarnedEver += defaultUseCost;
        int roll = Random.Range(1, 100);
        if (roll <= failurePercentage)
        {
            machineStatus = MachineStatus.Broken;
        }
    }

    /// <summary>
    /// When the machine is repaired
    /// </summary>
    protected void OnRepair()
    {
        MaintainenceToday += maintenanceCost;
        MaintainenceEver += maintenanceCost;
        GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().OnMachineBreakdown(this);
        machineStatus = MachineStatus.Working;
    }

    protected override void Awake()
    {
        base.Awake();
        machineStatus = MachineStatus.Working;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected virtual void DailyReset()
    {
        IncreaseFailurePercentage();
        NumberOfCustomersToday = 0;
        AmountEarnedToday = 0;
        MaintainenceToday = 0;
        expensesEver += runningCost;
    }

    #region Getters/Setters
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
            return defaultUseCost;
        }

        set
        {
            defaultUseCost = value;
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

    public int MinimumUseCost
    {
        get
        {
            return minimumUseCost;
        }

        set
        {
            minimumUseCost = value;
        }
    }

    public int MaximumUseCost
    {
        get
        {
            return maximumUseCost;
        }

        set
        {
            maximumUseCost = value;
        }
    }

    public int NumberOfCustomersToday
    {
        get
        {
            return numberOfCustomersToday;
        }

        set
        {
            numberOfCustomersToday = value;
        }
    }

    public int NumberOfCustomersEver
    {
        get
        {
            return numberOfCustomersEver;
        }

        set
        {
            numberOfCustomersEver = value;
        }
    }

    public float AmountEarnedToday
    {
        get
        {
            return amountEarnedToday;
        }

        set
        {
            amountEarnedToday = value;
        }
    }

    public float AmountEarnedEver
    {
        get
        {
            return amountEarnedEver;
        }

        set
        {
            amountEarnedEver = value;
        }
    }

    public float MaintainenceToday
    {
        get
        {
            return maintainenceToday;
        }

        set
        {
            maintainenceToday = value;
        }
    }

    public float MaintainenceEver
    {
        get
        {
            return maintainenceEver;
        }

        set
        {
            maintainenceEver = value;
        }
    }

    public float ExpensesEver
    {
        get
        {
            return expensesEver;
        }

        set
        {
            expensesEver = value;
        }
    }

    public Transform GetUsePosition()
    {
        return usePosition;
    }

    #endregion Getters/Setters
}
