using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{      
    [SerializeField] private Customer[] customers;

    private float levelSpeedFactor;
    private Transform spawnLocation;
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
            int randomCustomer = Random.Range(0, customers.Length);
            Customer newCustomer = Instantiate(customers[randomCustomer], spawnLocation.position, Quaternion.identity) as Customer;
            newCustomer.SetSpawnLocation(spawnLocation);
            newCustomer.SetManager(this);
            currentCustomers.Add(newCustomer);
        }
    }

    public void SpawnCustomer()
    {
        int randomCustomer = Random.Range(0, customers.Length);
        Customer newCustomer = Instantiate(customers[randomCustomer], spawnLocation.position, Quaternion.identity) as Customer;
        newCustomer.SetSpawnLocation(spawnLocation);
        newCustomer.SetManager(this);
        currentCustomers.Add(newCustomer);
        print(currentCustomers.Count);
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
        return 1;
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
}
