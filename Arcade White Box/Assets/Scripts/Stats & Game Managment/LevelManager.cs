using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public enum ArcadeOpeningStatus { Open, Closed }
    private ArcadeOpeningStatus arcadeStatus = ArcadeOpeningStatus.Closed;

    private List<GameObject> allObjectsInLevel;
    private List<Machine> allMachineObjects;
    private List<Machine> allGameMachines;
    private List<Machine> allToilets;
    private List<Machine> allFoodStalls; 

    private List<BaseAI> allStaff;

    private TimeAndCalendar _timeLink;
    private PlayerManager _playerLink;
    private CustomerManager customerManager;

    [SerializeField] private float customerSpawnRate, rentCost;
    [SerializeField] private int MAXCUSTOMERS; // dont change this matt
    [SerializeField] private int openingHour, closingHour;


    private int numOfCustomers;
    private bool openOnce, closedOnce, spawningCustomers;

    private float profitEntranceFees, profitGamesMachines, profitFoodStalls, profitOther;
    private float expensesTodaysPurchases, expensesStaffWages, expensesGamesMachineDailyCost, expensesGamesMachineMaintenance, expensesFoodStallsDailyCost, expensesFoodStallsMaintenance, expensesServiceMachineDailyCost, expensesServiceMachineMaintenanceCost;

    // Use this for initialization
    void Start () {

        _timeLink = this.gameObject.GetComponent<TimeAndCalendar>();
        _playerLink = this.gameObject.GetComponent<PlayerManager>();
        customerManager = this.GetComponent<CustomerManager>();
        openOnce = false;
        closedOnce = false;

        customerManager.SetSpawnLocation(transform);

        allObjectsInLevel = new List<GameObject>();
        allMachineObjects = new List<Machine>();
        allGameMachines = new List<Machine>();
        allToilets = new List<Machine>();
        allFoodStalls = new List<Machine>();

        spawningCustomers = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (_timeLink.CurrentHour == openingHour && !openOnce)
        {
            arcadeStatus = ArcadeOpeningStatus.Open;
            print("Arcade is open!");
            openOnce = true;
            closedOnce = false;
            //openingScript
        }
        else if (_timeLink.CurrentHour == closingHour && !closedOnce)
        {
            arcadeStatus = ArcadeOpeningStatus.Closed;
            print("Arcade is closed!");
            closedOnce = true;
            openOnce = false;
            ClosingTime();
        }

        if(allGameMachines.Count > 0 && allToilets.Count > 0 && allFoodStalls.Count > 0 && !spawningCustomers)
        {
            spawningCustomers = true;
            customerManager.InvokeRepeating("SpawnCustomer", customerSpawnRate, customerSpawnRate);
        }

        if(customerManager.GetCustomerNumber() >= MAXCUSTOMERS)
        {
            customerManager.CancelInvoke("SpawnCustomer");
        }
	}

    private float TotalExpenses()
    {
        float cost = 0 + rentCost;
        foreach(Machine obj in allMachineObjects)
        {
            cost += obj.RunningCost;
        }

        foreach (BaseAI ai in allStaff)
        {   
            if(ai is Staff)
            {
                cost += (ai as Staff).WageCost;
            }
        }
        return cost;
    }

    private void AddToEarnedToday(float amount)
    {
        _playerLink.CurrentlyEarnedToday += amount;
    }

    private void ClosingTime()
    {
        _playerLink.ClosingTime();
        expensesServiceMachineMaintenanceCost = 0;
        expensesGamesMachineMaintenance = 0;
        expensesFoodStallsMaintenance = 0;
    }

    public List<float> GetExpensesArray()
    {
        List<float> expensesArray = new List<float>();
        expensesArray.Add(rentCost);
        expensesArray.Add(expensesTodaysPurchases);
        expensesArray.Add(expensesStaffWages);
        expensesArray.Add(expensesGamesMachineDailyCost);
        expensesArray.Add(expensesGamesMachineMaintenance);
        expensesArray.Add(expensesFoodStallsDailyCost);
        expensesArray.Add(expensesFoodStallsMaintenance);
        expensesArray.Add(expensesServiceMachineDailyCost);
        expensesArray.Add(expensesServiceMachineMaintenanceCost);

        return expensesArray;
    }

    public void OnStaffHire(Staff staffMember)
    {
        expensesStaffWages += staffMember.WageCost;
    }

    public void OnStaffFire(Staff staffMember)
    {
        expensesStaffWages -= staffMember.WageCost;
    }

    public void OnMachineBreakdown(Machine brokenMachine)
    {
        if (brokenMachine is GameMachine)
        {
            expensesGamesMachineMaintenance += brokenMachine.MaintenanceCost;
        }

        else if (brokenMachine is ServiceMachine)
        {
            expensesServiceMachineMaintenanceCost += brokenMachine.MaintenanceCost;
        }

        else if (brokenMachine is FoodMachine)
        {
            expensesFoodStallsMaintenance += brokenMachine.MaintenanceCost;
        }
    }

    public void OnMachinePurchase(Machine purchase)
    {
        _playerLink.CurrentCash -= purchase.BuyCost;
        expensesTodaysPurchases += purchase.BuyCost;
        if (purchase is GameMachine)
        {
            expensesGamesMachineDailyCost += purchase.RunningCost;
        }
        else if (purchase is ServiceMachine)
        {
            expensesServiceMachineDailyCost += purchase.RunningCost;
        }
        else if (purchase is FoodMachine)
        {
            expensesFoodStallsDailyCost += purchase.RunningCost;
        }
    }

    public void OnBuildingPartPurchase(PlaceableObject purchase)
    {
        _playerLink.CurrentCash -= purchase.BuyCost;
        expensesTodaysPurchases += purchase.BuyCost;
    }

    public float GetTotalExpenses()
    {
        float exp = 0.0f;
        exp += rentCost;
        exp += expensesTodaysPurchases;
        exp += expensesStaffWages;
        exp += expensesFoodStallsDailyCost;
        exp += expensesFoodStallsMaintenance;
        exp += expensesGamesMachineDailyCost;
        exp += expensesGamesMachineMaintenance;
        exp += expensesServiceMachineDailyCost;
        exp += expensesServiceMachineMaintenanceCost;

        return exp;

    }

    public void AddObjectToLists(GameObject objectToAdd)
    {
        allObjectsInLevel.Add(objectToAdd);

        if(objectToAdd.GetComponent<GameMachine>())
        {
            allGameMachines.Add(objectToAdd.GetComponent<GameMachine>());
        }
        else if(objectToAdd.GetComponent<FoodMachine>())
        {
            allFoodStalls.Add(objectToAdd.GetComponent<FoodMachine>());
        }
        else if (objectToAdd.GetComponent<ServiceMachine>())
        {
            allToilets.Add(objectToAdd.GetComponent<ServiceMachine>());
        }

        if(customerManager)
        {
            customerManager.SetGameMachines(allGameMachines);
            customerManager.SetFoodStalls(allFoodStalls);
            customerManager.SetToilets(allToilets);
        }
    }

    public void RemoveObjectFromLists(GameObject objectToRemove)
    {
        allObjectsInLevel.Remove(objectToRemove);

        if (objectToRemove.GetComponent<GameMachine>())
        {
            allGameMachines.Remove(objectToRemove.GetComponent<GameMachine>());
            allGameMachines.TrimExcess();
        }
        else if (objectToRemove.GetComponent<FoodMachine>())
        {
            allFoodStalls.Remove(objectToRemove.GetComponent<FoodMachine>());
            allGameMachines.TrimExcess();
        }
        else if (objectToRemove.GetComponent<ServiceMachine>())
        {
            allToilets.Remove(objectToRemove.GetComponent<ServiceMachine>());
            allGameMachines.TrimExcess();
        }

        if (customerManager)
        {
            customerManager.SetGameMachines(allGameMachines);
            customerManager.SetFoodStalls(allFoodStalls);
            customerManager.SetToilets(allToilets);
        }
    }
}
