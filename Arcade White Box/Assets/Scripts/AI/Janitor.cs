using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : Staff
{   
    private enum JanitorSate { Idle, Moving, Cleaning, Searching }

    private bool cleaning;
    private JanitorSate currentState;
    private CustomerManager customerManager;
    private Transform currentTarget;
    private IEnumerator cleaningUp;

    protected override void Awake()
    {
        base.Awake();

        currentState = JanitorSate.Idle;
        cleaning = false;
    }

    void Start()
    {
        customerManager = GameManager.Instance.SceneManagerLink.GetComponent<CustomerManager>();
    }

    protected override void Update()
    {
        if(currentState == JanitorSate.Searching)
        {
            //currentTarget = customerManager.GetDroppedTrash()[customerManager.GetDroppedTrash().Count - 1].transform;
            //customerManager.GetDroppedTrash().RemoveAt(customerManager.GetDroppedTrash().Count - 1);

            unitController.SetTarget(currentTarget);
            unitController.GetNewPath();
            currentState = JanitorSate.Moving;
        }
        else if(currentState == JanitorSate.Moving)
        {
            if(unitController.ReachedTarget)
            {
                currentState = JanitorSate.Cleaning;
                cleaningUp = CleaningUp();
                StartCoroutine(cleaningUp);
            }
        }
        else if(currentState == JanitorSate.Idle)
        {
            //if(customerManager.GetDroppedTrash().Count > 0)
            //{
            //    currentState = JanitorSate.Searching;
            ///}
        }

        base.Update();
    }

    private IEnumerator CleaningUp()
    {
        cleaning = true;
        yield return new WaitForSeconds(0.5f);
        currentState = JanitorSate.Idle;
        Destroy(currentTarget.gameObject);
        cleaning = false;
    }
}
