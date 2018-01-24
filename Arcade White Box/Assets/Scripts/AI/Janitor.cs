using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : Staff
{
    [SerializeField] private float cleanTime;
       
    private enum JanitorSate {Idle, HasJob, Moving, Cleaning}

    private bool hasJob;
    private Tile currentTile;
    private JanitorSate currentState;
    private IEnumerator cleaningUp;

    protected override void Awake()
    {
        base.Awake();

        currentState = JanitorSate.Idle;
        hasJob = false;
    }

    protected override void Update()
    {   
        if(currentState == JanitorSate.HasJob)
        {
            unitController.SetTarget(currentTile.transform);
            unitController.GetNewPath();
            currentState = JanitorSate.Moving;
        }
        else if(currentState == JanitorSate.Moving)
        {
            if (unitController.ReachedTarget)
            {
                StartCoroutine("CleaningUp");
            }
        }

        base.Update();
    }

    private IEnumerator CleaningUp()
    {
        currentState = JanitorSate.Cleaning;
        yield return new WaitForSeconds(cleanTime);
        currentTile.CleanTrash();
        currentState = JanitorSate.Idle;
        hasJob = false;
    }

    public void SetNewTarget(Tile newTile)
    {
        currentTile = newTile;
        hasJob = true;
        currentState = JanitorSate.HasJob;
    }

    public bool HasJob() { return hasJob; }
}
 