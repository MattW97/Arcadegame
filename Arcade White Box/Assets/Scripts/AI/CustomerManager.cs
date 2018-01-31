using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    public Customer[] customers;
    [SerializeField]
    private Transform spawnLocation;

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
        UpdateCustomers();
    }

    private void UpdateCustomers()
    {
        if (currentCustomers.Count > 0)
        {
            for (int i = currentCustomers.Count - 1; i >= 0; i--)
            {
                Customer customer = currentCustomers[i];
                Customer.CustomerStates currentState = customer.GetCurrentCustomerState();
                Transform customerTransform = customer.GetCustomerTransform();

                //customer.SetSpeedFactor(GetSpeedFactor());
                customer.StatTick();

                if(customer.WantsToLeave() && !customer.IsLeaving())
                {
                    customer.LeaveArcade();
                    customer.SetCurrentCustomerState(Customer.CustomerStates.Leaving);
                }

                if(Random.Range(0, 10000) <= 1)
                {
                    customer.DropTrash();
                }

                switch (currentState)
                {
                    case Customer.CustomerStates.Idle:

                        if (customer.HungerStat >= 100.0f)
                        {
                            Machine chosenFacility = FindNearestFacility(foodFacilities, customerTransform, customer);
                            if (chosenFacility)
                            {
                                chosenFacility.InUse = true;
                                customer.SetNewTarget(chosenFacility);
                                customer.SetCurrentCustomerState(Customer.CustomerStates.GotTarget);
                                customer.HungerStat = 0.0f;

                                print("HAS A NEW TARGET");
                            }
                        }
                        else if (customer.BladderStat >= 100.0f)
                        {
                            Machine chosenFacility = FindNearestFacility(toilets, customerTransform, customer);
                            if (chosenFacility)
                            {
                                chosenFacility.InUse = true;
                                customer.SetNewTarget(chosenFacility);
                                customer.SetCurrentCustomerState(Customer.CustomerStates.GotTarget);
                                customer.BladderStat = 0.0f;

                                print("HAS A NEW TARGET");
                            }
                        }
                        else
                        {
                            Machine chosenFacility = FindNearestFacility(gameMachines, customerTransform, customer);
                            if (chosenFacility)
                            {
                                chosenFacility.InUse = true;
                                customer.SetNewTarget(chosenFacility);
                                customer.SetCurrentCustomerState(Customer.CustomerStates.GotTarget);
                                customer.HappinessStat += 10.0f;

                                print("HAS A NEW TARGET");
                            }
                        }

                        break;

                    case Customer.CustomerStates.GotTarget:

                        print("STARTED MOVING");
                        customer.StartMoving();
                        customer.SetCurrentCustomerState(Customer.CustomerStates.Moving);

                        break;

                    case Customer.CustomerStates.Moving:

                        if (customer.ReachedTarget())
                        {
                            print("REACHED TARGET");
                            customer.SetCurrentCustomerState(Customer.CustomerStates.UsingFacility);
                            customer.BeginUsingFacility();
                        }

                        break;

                    case Customer.CustomerStates.Leaving:

                        if (customer.ReachedTarget())
                        {
                            print("LEFT");
                            currentCustomers.RemoveAt(i);
                            currentCustomers.TrimExcess();
                            Destroy(customer.gameObject);
                        }

                        break;
                }
            }
        }
    }

    private Machine FindNearestFacility(List<Machine> facilities, Transform customerTransform, Customer currentCustomer)
    {
        Machine nearest = null;

        if (facilities.Count == 0)
        {
            return null;
        }

        facilities.Sort(delegate (Machine a, Machine b) { return Vector3.Distance(customerTransform.position, a.transform.position).CompareTo(Vector3.Distance(customerTransform.position, b.transform.position)); });

        foreach (Machine facility in facilities)
        {
            //print(facility.name + facility.IsUsable());

            if (facility.IsUsable() && !currentCustomer.RepeatTarget(facility))
            {
                nearest = facility;
                //nearest = facilities[Random.Range(0, facilities.Count)];
                break;
            }
        }


        //print(nearest);
        return nearest;
    }

    public void SpawnCustomers(int amount)
    {
        for (int i = 0; i < amount; i++)
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
        for (int i = 0; i < currentCustomers.Count; i++)
        {
            currentCustomers[i].LeaveArcade();
        }

        //currentCustomers.Clear();
        //currentCustomers.TrimExcess();
    }

    public void ClearCustomerParent()
    {
        foreach (Customer customer in currentCustomers)
        {
            Destroy(customer.gameObject);
        }
    }

    public float GetSpeedFactor() { return gameTime.timeMultiplier; }
    public int GetNumberOfCustomers() { return currentCustomers.Count; }

    public void SetToilets(List<Machine> toilets) { this.toilets = toilets; }
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
        if (currentCustomers.Count != 0)
        {
            float average = 0.0f;

            foreach (Customer customer in currentCustomers)
            {
                average += customer.HappinessStat;
            }

            return average / currentCustomers.Count;
        }
        else
        {
            return 0;
        }
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
            float randomIncrease = Random.Range(0, 50);
            average += randomIncrease;
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
