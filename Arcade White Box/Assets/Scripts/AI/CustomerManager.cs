﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{      
    [SerializeField] public Customer[] customers;
    [SerializeField] private GameObject[] customerTrash;

    private float levelSpeedFactor;
    private TimeAndCalendar gameTime;
    private Transform spawnLocation, trashParent;
    private List<Customer> currentCustomers;
    private List<Machine> foodFacilities;
    private List<Machine> gameMachines;
    private List<Machine> toilets;

    void Awake()
    {
        currentCustomers = new List<Customer>();
        foodFacilities = new List<Machine>();
        gameMachines = new List<Machine>();
        toilets = new List<Machine>();
    }

    void Start()
    {
        gameTime = GameManager.Instance.ScriptHolderLink.GetComponent<TimeAndCalendar>();
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
            int randomCustomer = Random.Range(0, customers.Length);
            Customer newCustomer = Instantiate(customers[randomCustomer], spawnLocation.position, Quaternion.identity) as Customer;
            newCustomer.prefabName = customers[randomCustomer].name;
            newCustomer.SetSpawnLocation(spawnLocation);
            newCustomer.SetManager(this);
            currentCustomers.Add(newCustomer);
        }
    }

    public void SpawnCustomer()
    {
        int randomCustomer = Random.Range(0, customers.Length);
        Customer newCustomer = Instantiate(customers[randomCustomer], spawnLocation.position, Quaternion.identity) as Customer;
        newCustomer.prefabName = customers[randomCustomer].name;
        newCustomer.SetSpawnLocation(spawnLocation);
        newCustomer.SetManager(this);
        currentCustomers.Add(newCustomer);
    }

    public void MassLeave()
    {
        for(int i = 0; i < currentCustomers.Count; i++)
        {
            currentCustomers[i].LeaveArcade();
        }

        currentCustomers.Clear();
    }

    public void ClearCustomerParent()
    {
        foreach (Customer customer in currentCustomers)
        {
            Destroy(customer.gameObject);
        }
    }

    public float GetSpeedFactor()
    {
        return gameTime.timeMultiplier;
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

    public int GetCustomerNumber()
    {
        return currentCustomers.Count;
    }

    public GameObject GetTrash()
    {
        return customerTrash[Random.Range(0, customerTrash.Length)];
    }

    public void SetSpawnLocation(Transform spawnLocation)
    {
        this.spawnLocation = spawnLocation;
    }

    public void SetToilets(List<Machine> toilets)
    {
        this.toilets = toilets;
    }

    public void SetFoodStalls(List<Machine> foodFacilities)
    {   
        this.foodFacilities = foodFacilities;
    }

    public void SetGameMachines(List<Machine> gameMachines)
    {
        this.gameMachines = gameMachines;
    }

    public int NumberOfCurrentCustomers()
    {
        return currentCustomers.Count;
    }

    public void LoadClearLists()
    {
        toilets.Clear();
        foodFacilities.Clear();
        gameMachines.Clear();
    }
}
