using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : Staff
{
    [SerializeField] private float repairTime;


    private enum MechanicState {Idle, GotTarget, Moving, Repairing}

    private bool hasJob;
    private MechanicState currentState;
    private Machine currentMachine;

    protected override void Awake()
    {
        base.Awake();

        currentState = MechanicState.Idle;
        hasJob = false;
    }

    protected override void Update()
    {
        if (currentState == MechanicState.GotTarget)
        {
            unitController.SetTarget(currentMachine.GetUsePosition());
            unitController.GetNewPath();
            currentState = MechanicState.Moving;
        }
        else if (currentState == MechanicState.Moving)
        {
            if (unitController.ReachedTarget)
            {
                StartCoroutine("RepairMachine", currentMachine);
            }
        }

        base.Update();
    }

    private IEnumerator RepairMachine(Machine brokenMachine)
    {
        currentState = MechanicState.Repairing;
        brokenMachine.OnRepair();
        yield return new WaitForSeconds(repairTime);
        currentState = MechanicState.Idle;
    }


    public void AssignMachine(Machine newMachine)
    {
        currentMachine = newMachine;
        currentState = MechanicState.GotTarget;
    }



    public bool HasJob
    {
        get
        {
            return hasJob;
        }

        set
        {
            hasJob = value;
        }
    }
}
