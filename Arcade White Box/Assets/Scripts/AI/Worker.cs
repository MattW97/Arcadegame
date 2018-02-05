using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Staff
{
    private enum WorkerState { Idle, HasJob, Moving}

    private bool hasJob;
    private Machine jobObject;
    private WorkerState currentState;

    protected override void Awake()
    {   
        base.Awake();

        currentState = WorkerState.Idle;
        jobObject = null;
    }

    protected override void Update()
    {   
        if(currentState == WorkerState.HasJob)
        {
            unitController.SetTarget(jobObject.GetUsePosition());
            unitController.GetNewPath();
            currentState = WorkerState.Moving;
        }
        else if(currentState == WorkerState.Moving)
        {
            if(unitController.ReachedTarget)
            {

            }
        }

        base.Update();
    }

    public void GiveNewJob(Machine jobObject)
    {
        this.jobObject = jobObject;
        currentState = WorkerState.HasJob;
    }

    public bool HasJob() { return hasJob; }
}
