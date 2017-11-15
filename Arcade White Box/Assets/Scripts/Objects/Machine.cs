using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : PlaceableObject {

    public enum MachineStatus { Working, Broken }

    [SerializeField] private float useTime;         // Time it takes to use object, i.e. buy/eat food, use toilet, play game (in Seconds)
    [SerializeField] private float runningCost;     // amount deducted each day
    [SerializeField] private float useCost;         // how much it costs a user to use the machine. Includes playing if the machine is a game machine.
    [SerializeField] private float maintenanceCost; // amount deducted upon machine breaking and needing repair
    [SerializeField] private Transform usePosition; // The position the customers moves to when using the object
    [SerializeField] private float failurePercentage; // The chance the machine has to break on use
    [SerializeField] private float failurePercentageIncrease; // How much the failurePercentage increases by each IncreaseFailurePercentage() call.


    private MachineStatus machineStatus;
    private bool inUse;

    private void IncreaseFailurePercentage()
    {
        failurePercentage = failurePercentage + failurePercentageIncrease;
    }

  /// <summary>
  /// When the machine is used.
  /// </summary>
    protected virtual void OnUse()
    {
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

    public Transform GetUsePosition()
    {
        return usePosition;
    }
}
