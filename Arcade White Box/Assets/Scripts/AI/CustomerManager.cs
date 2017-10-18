using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
       
    [SerializeField] private Customer customer;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform[] toilets, foodStalls, gameMachines;

    private List<Customer> currentCustomers;

    void Awake()
    {
        instance = this;

        currentCustomers = new List<Customer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            MassLeave();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SpawnCustomers(1);
        }
    }

    public void SpawnCustomers(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Customer newCustomer = Instantiate(customer, spawnLocation.position, Quaternion.identity) as Customer;
            newCustomer.SetSpawnLocation(spawnLocation);
            currentCustomers.Add(newCustomer);
        }
    }

    public void MassLeave()
    {
        for(int i = 0; i < currentCustomers.Count; i++)
        {
            currentCustomers[i].LeaveArcade();
        }

        currentCustomers.Clear();
    }

    public Transform[] GetToilets()
    {
        return toilets;
    }

    public Transform[] GetFoodStalls()
    {
        return foodStalls;
    }

    public Transform[] GetGameMachines()
    {
        return gameMachines;
    }
}
