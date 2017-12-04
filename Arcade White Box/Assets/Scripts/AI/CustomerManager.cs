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
        if(foodFacilities.Count > 0 && toilets.Count > 0 && foodFacilities.Count > 0)
        {
            UpdateCustomers();
        }
    }

    private void UpdateCustomers()
    {
        if(currentCustomers.Count > 0)
        {
            for(int i = currentCustomers.Count - 1; i >= 0; i--)
            {
                if (currentCustomers[i].GetCurrentCustomerState() != Customer.CustomerStates.Leaving)
                {
                    currentCustomers[i].SpeedFactor = GetSpeedFactor();
                    currentCustomers[i].StatTick();

                    if (currentCustomers[i].HappinessStat <= 0.0f || currentCustomers[i].TirednessStat >= 100.0f)
                    {
                        currentCustomers[i].LeaveArcade();
                        currentCustomers.RemoveAt(i);
                        continue;
                    }

                    if (!currentCustomers[i].IsBusy())
                    {
                        if (currentCustomers[i].BladderStat >= 75.0f)
                        {
                            Machine availableMachine = FindNearestFacility(toilets, currentCustomers[i].transform);

                            if (availableMachine)
                            {
                                currentCustomers[i].SetCurrentCustomerState(Customer.CustomerStates.GotTarget);

                                availableMachine.InUse = true;
                                currentCustomers[i].HappinessStat += 10.0f;
                                currentCustomers[i].BladderStat -= 25.0f;
                                currentCustomers[i].TirednessStat += 5.0f;
                                currentCustomers[i].SetNewTarget(availableMachine);
                            }
                            else
                            {
                                currentCustomers[i].HappinessStat -= 25.0f;
                                currentCustomers[i].SetCurrentCustomerState(Customer.CustomerStates.BeginningWander);
                            }
                        }
                        else if (currentCustomers[i].HungerStat >= 75.0f)
                        {
                            Machine availableMachine = FindNearestFacility(foodFacilities, currentCustomers[i].transform);

                            if (availableMachine)
                            {
                                currentCustomers[i].SetCurrentCustomerState(Customer.CustomerStates.GotTarget);

                                availableMachine.InUse = true;
                                currentCustomers[i].HappinessStat += 10.0f;
                                currentCustomers[i].HungerStat -= 25.0f;
                                currentCustomers[i].TirednessStat += 5.0f;
                                currentCustomers[i].SetNewTarget(availableMachine);
                            }
                            else
                            {
                                currentCustomers[i].HappinessStat -= 25.0f;
                                currentCustomers[i].SetCurrentCustomerState(Customer.CustomerStates.BeginningWander);
                            }
                        }
                        else
                        {
                            Machine availableMachine = FindNearestFacility(gameMachines, currentCustomers[i].transform);

                            if (availableMachine)
                            {
                                currentCustomers[i].SetCurrentCustomerState(Customer.CustomerStates.GotTarget);

                                availableMachine.InUse = true;
                                currentCustomers[i].HappinessStat += 10.0f;
                                currentCustomers[i].TirednessStat += 5.0f;
                                currentCustomers[i].SetNewTarget(availableMachine);
                            }
                            else
                            {
                                currentCustomers[i].HappinessStat -= 25.0f;
                                currentCustomers[i].SetCurrentCustomerState(Customer.CustomerStates.BeginningWander);
                            }
                        }
                    }
                }

                currentCustomers[i].CustomerUpdate();
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

    public float GetSpeedFactor()       { return gameTime.timeMultiplier; }
    public int GetNumberOfCustomers()   { return currentCustomers.Count; }

    public void SetToilets(List<Machine> toilets)           { this.toilets = toilets; }
    public void SetFoodStalls(List<Machine> foodFacilities) { this.foodFacilities = foodFacilities; }
    public void SetGameMachines(List<Machine> gameMachines) { this.gameMachines = gameMachines; }

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
