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


    private void NewCustomersUpdate()
    {
        if (currentCustomers.Count > 0)
        {
            for (int i = currentCustomers.Count - 1; i >= 0; i--)
            {
                Customer customer = currentCustomers[i];
                Customer.CustomerStates currentCustomerState = customer.GetCurrentCustomerState();

                if (currentCustomerState == Customer.CustomerStates.Idle)
                {
                    if(customer.BladderStat >= 80.0f)
                    {

                    }
                    else if(customer.HungerStat >= 80.0f)
                    {

                    }
                    else
                    {

                    }
                }

                customer.UpdateCustomer();
            }
        }
    }

    private void UpdateCustomers()
    {
        if(currentCustomers.Count > 0)
        {
            for(int i = currentCustomers.Count - 1; i >= 0; i--)
            {
                Customer customer = currentCustomers[i];

                if (!customer.IsLeaving())
                {
                    customer.SpeedFactor = GetSpeedFactor();
                    customer.StatTick();

                    if (customer.WantsToLeave())
                    {
                        customer.LeaveArcade();
                        continue;
                    }

                    if (!customer.IsBusy())
                    {
                        if (customer.BladderStat >= 75.0f)
                        {
                            Machine availableMachine = FindNearestFacility(toilets, customer.transform);

                            if(availableMachine)
                            {
                                availableMachine.InUse = true;
                                customer.BladderStat = 25.0f;
                                customer.TirednessStat += 5.0f;
                                customer.SetNewTarget(availableMachine);
                            }
                            else
                            {
                                customer.HappinessStat -= 25.0f;
                                customer.SetCurrentCustomerState(Customer.CustomerStates.BeginningWander);
                            }
                        }
                        else if (customer.HungerStat >= 75.0f)
                        {
                            Machine availableMachine = FindNearestFacility(foodFacilities, customer.transform);

                            if (availableMachine)
                            {
                                availableMachine.InUse = true;
                                customer.HungerStat = 25.0f;
                                customer.TirednessStat += 5.0f;
                                customer.SetNewTarget(availableMachine);
                            }
                            else
                            {
                                customer.HappinessStat -= 25.0f;
                                customer.SetCurrentCustomerState(Customer.CustomerStates.BeginningWander);
                            }
                        }
                        else
                        {
                            Machine availableMachine = FindNearestFacility(gameMachines, customer.transform);

                            if (availableMachine)
                            {
                                availableMachine.InUse = true;
                                customer.TirednessStat += 5.0f;

                                if(availableMachine is GameMachine)
                                {
                                    float hap = availableMachine.GetComponent<GameMachine>().TotalHappiness();
                                    customer.HappinessStat += hap;
                                }

                                customer.SetNewTarget(availableMachine);
                            }
                            else
                            {
                                customer.HappinessStat -= 25.0f;
                                customer.SetCurrentCustomerState(Customer.CustomerStates.BeginningWander);
                            }
                        }
                    }
                }

                customer.UpdateCustomer();

                if(customer.GetCurrentCustomerState() == Customer.CustomerStates.Left)
                {
                    Destroy(customer.gameObject);
                    currentCustomers.RemoveAt(i);
                    currentCustomers.TrimExcess();
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
            if (facility.IsUsable())
            {
                nearest = facility;
                break;
            }
        }

        return nearest;
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
        currentCustomers.TrimExcess();
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

        return average / currentCustomers.Count;
    }

    public float GetAverageCustomerBladder()
    {
        float average = 0.0f;

        foreach (Customer customer in currentCustomers)
        {
            average += customer.BladderStat;
        }

        return average / currentCustomers.Count;
    }

    public float GetAverageCustomerHunger()
    {
        float average = 0.0f;

        foreach (Customer customer in currentCustomers)
        {
            average += customer.HungerStat;
        }

        return average / currentCustomers.Count;
    }

    public float GetAverageCustomerTiredness()
    {
        float average = 0.0f;

        foreach (Customer customer in currentCustomers)
        {
            average += customer.TirednessStat;
        }

        return average / currentCustomers.Count;
    }

    public float GetAverageCustomerQueasiness()
    {
        float average = 0.0f;

        foreach (Customer customer in currentCustomers)
        {
            average += customer.QueasinessStat;
        }

        return average / currentCustomers.Count;
    }

    // ---------------------------------------------------------------------------------------------
}
