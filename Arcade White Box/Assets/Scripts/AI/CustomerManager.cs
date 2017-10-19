using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
       
    [SerializeField] private Customer customer;
    [SerializeField] private Transform spawnLocation;

    private float levelSpeedFactor;
    private List<Customer> currentCustomers;
    private List<Machine> foodFacilities;
    private List<Machine> gameMachines;
    private List<Machine> toilets;

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            levelSpeedFactor = 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            levelSpeedFactor = 2.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            levelSpeedFactor = 4.0f;
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

    public float GetSpeedFactor()
    {
        return levelSpeedFactor;
    }

    public List<Machine> GetToilets()
    {
        return toilets;
    }

    public List<Machine> GetFoodStalls()
    {
        return foodFacilities;
    }

    public List<Machine> GetGameMachines()
    {
        return gameMachines;
    }
}
