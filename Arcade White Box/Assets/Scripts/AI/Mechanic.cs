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
    private IEnumerator repairing;

    protected override void Awake()
    {
        base.Awake();

        currentState = MechanicState.Idle;
        hasJob = false;
    }

    protected override void Update()
    {
        if(currentState == MechanicState.GotTarget)
        {
            unitController.SetTarget(currentMachine.transform);
            unitController.GetNewPath();
            currentState = MechanicState.Moving;
        }
        else if (currentState == MechanicState.Moving)
        {
            if (unitController.ReachedTarget)
            {
                StartCoroutine("repairing");
            }
        }

        base.Update();
    }

    private IEnumerator Repairing()
    {
        currentState = MechanicState.Repairing;
        yield return new WaitForSeconds(repairTime);
    }
}
