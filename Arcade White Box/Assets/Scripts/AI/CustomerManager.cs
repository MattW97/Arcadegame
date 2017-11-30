using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{      
    [SerializeField] public Customer[] customers;
    [SerializeField] private Transform spawnLocation;

    private float levelSpeedFactor;
    private TimeAndCalendar gameTime;
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

        if(foodFacilities.Count > 0 && toilets.Count > 0 && foodFacilities.Count > 0)
        {
            UpdateCustomers();
        }
    }

    private void UpdateCustomers()
    {
        foreach(Customer customer in currentCustomers)
        {
            if (customer.GetCurrentCustomerState() != Customer.CustomerStates.Leaving)
            {
                customer.SpeedFactor = GetSpeedFactor();
                customer.StatDecayTick();

                if(customer.HappinessStat <= 0.0f || customer.TirednessStat >= 100.0f)
                {
                    print("LEAVING");
                    customer.LeaveArcade();
                }

                if (!customer.IsBusy())
                {
                    if (customer.BladderStat >= 75.0f)
                    {
                        Machine availableMachine = FindNearestFacility(toilets, customer.transform);
                        
                        if(availableMachine)
                        {
                            availableMachine.InUse = true;
                            customer.SetNewTarget(availableMachine);
                            print("TARGET SENT");
                        }
                        else
                        {
                            customer.HappinessStat -= 25.0f;
                            customer.SetCurrentCustomerState(Customer.CustomerStates.BeginningWander);
                            print("WANDERING");
                        }
                    }
                    else if(customer.HungerStat >= 75.0f)
                    {
                        Machine availableMachine = FindNearestFacility(foodFacilities, customer.transform);

                        if (availableMachine)
                        {
                            availableMachine.InUse = true;
                            customer.SetNewTarget(availableMachine);
                            print("TARGET SENT");
                        }
                        else
                        {
                            customer.HappinessStat -= 25.0f;
                            customer.SetCurrentCustomerState(Customer.CustomerStates.BeginningWander);
                            print("WANDERING");
                        }
                    }
                }
                else
                {

                }
            }
        }
    }

    private Machine FindNearestFacility(List<Machine> facilities, Transform customerTransform)
    {
        Machine nearest = null;

        if (facilities.Count == 0)
        {
            return null;
        }

        facilities.Sort(delegate (Machine a, Machine b) { return Vector3.Distance(customerTransform.position, a.transform.position).CompareTo(Vector3.Distance(customerTransform.position, b.transform.position)); });

        foreach (Machine facility in facilities)
        {
            if (!facility.InUse)
            {
                nearest = facility;
                break;
            }
        }

        return nearest;
    }

    private CustomerStat GetHighestStat(List<CustomerStat> customerStats)
    {
        CustomerStat highestStat = new CustomerStat();

        foreach (CustomerStat stat in customerStats)
        {
            if (stat.StatValue > highestStat.StatValue)
            {
                highestStat = stat;
            }
        }

        return highestStat;
    }

    public void SpawnCustomers(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            SpawnCustomer();
        }
    }

    public void SpawnCustomer()
    {
        int randomCustomer = Random.Range(0, customers.Length);
        Customer newCustomer = Instantiate(customers[randomCustomer], spawnLocation.position, Quaternion.identity) as Customer;
        newCustomer.prefabName = customers[randomCustomer].name;
        newCustomer.SetSpawnLocation(spawnLocation);
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

    public int GetCustomerNumber()
    {
        return currentCustomers.Count;
    }

    public void SetToilets(List<Machine> toilets)           { this.toilets = toilets; }
    public void SetFoodStalls(List<Machine> foodFacilities) { this.foodFacilities = foodFacilities; }
    public void SetGameMachines(List<Machine> gameMachines) { this.gameMachines = gameMachines; }

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

    // ---------------------------------------------------------------------------------------------
    // AVERAGE CUSTOMER STATS FOR CURRENT IN-ARCADE CUSTOMERS --------------------------------------
    // ---------------------------------------------------------------------------------------------
    
    public float GetAverageCustomerHappiness()
    {
        float average = 0.0f;

        foreach(Customer customer in currentCustomers)
        {
            average += customer.HappinessStat;
        }

        return average;
    }

    public float GetAverageCustomerBladder()
    {
        float average = 0.0f;

        foreach (Customer customer in currentCustomers)
        {
            average += customer.BladderStat;
        }

        return average;
    }

    public float GetAverageCustomerHunger()
    {
        float average = 0.0f;

        foreach (Customer customer in currentCustomers)
        {
            average += customer.HungerStat;
        }

        return average;
    }

    public float GetAverageCustomerTiredness()
    {
        float average = 0.0f;

        foreach (Customer customer in currentCustomers)
        {
            average += customer.TirednessStat;
        }

        return average;
    }

    public float GetAverageCustomerQueasiness()
    {
        float average = 0.0f;

        foreach (Customer customer in currentCustomers)
        {
            average += customer.QueasinessStat;
        }

        return average;
    }

    // ---------------------------------------------------------------------------------------------
}
