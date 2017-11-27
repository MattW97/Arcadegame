using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : Staff
{
    [SerializeField] private float cleanTime;
       
    private enum JanitorSate {Idle, GotTarget, Moving, Cleaning}

    private bool hasJob;
    private JanitorSate currentState;
    private CustomerManager customerManager;
    private Tile currentTile;
    private IEnumerator cleaningUp;

    protected override void Awake()
    {
        base.Awake();

        currentState = JanitorSate.Idle;
        hasJob = false;
    }

    void Start()
    {
        customerManager = GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>();
    }

    protected override void Update()
    {   
        if(currentState == JanitorSate.GotTarget)
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
        currentState = JanitorSate.GotTarget;
    }

    public bool HasJob()
    {
        return hasJob;
    }
}
