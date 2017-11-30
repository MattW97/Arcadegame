using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public enum ArcadeOpeningStatus { Open, Closed }
    private ArcadeOpeningStatus arcadeStatus = ArcadeOpeningStatus.Closed;

    private List<Machine> allMachineObjects;
    private List<Machine> allGameMachines;
    private List<Machine> allToilets;
    private List<Machine> allFoodStalls;

    private List<BaseAI> allStaff;

    private TimeAndCalendar _timeLink;
    private PlayerManager _playerLink;
    private CustomerManager customerManager;
    private EconomyManager _economyLink;

    [SerializeField] private float customerSpawnRate, rentCost, startingCash;
    [SerializeField] private int MAXCUSTOMERS; 
    [SerializeField] private int openingHour, closingHour;
    [SerializeField] private int preOpeningTime;
    [SerializeField] private MainDoorController mainDoors;

    private int numOfCustomers;
    private bool openOnce, closedOnce, spawningCustomers, preOpenBool;

    void Start () {

        _timeLink = this.gameObject.GetComponent<TimeAndCalendar>();
        _playerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
        _economyLink = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        customerManager = this.GetComponent<CustomerManager>();
        openOnce = false;
        closedOnce = false;
        preOpenBool = false; 

        AllMachineObjects = new List<Machine>();
        AllGameMachines = new List<Machine>();
        AllToilets = new List<Machine>();
        AllFoodStalls = new List<Machine>();

        spawningCustomers = false;
    }
	
	void Update ()
    {
        OpenClose();
        if (CheckCustomerSpawnParameters())
        {
            customerManager.InvokeRepeating("SpawnCustomer", customerSpawnRate, customerSpawnRate);
        }
        

        if (customerManager.GetCustomerNumber() >= MAXCUSTOMERS)
        {
            customerManager.CancelInvoke("SpawnCustomer");
        }
    }

    public bool CheckCustomerSpawnParameters()
    {
        if (AllGameMachines.Count > 0 && AllToilets.Count > 0 && AllFoodStalls.Count > 0 && !spawningCustomers)
        {
            spawningCustomers = true;
            //customerManager.InvokeRepeating("SpawnCustomer", customerSpawnRate, customerSpawnRate);
            return true;
        }
        return false;
    }

    private void OpenClose()
    {
        if (_timeLink.GetCurrentTime() == preOpeningTime && !preOpenBool)
        {
            _timeLink.StopTimer();
            preOpenBool = true;
            // create UI stuff to show next day
        }
        else if (_timeLink.CurrentHour == openingHour && !openOnce)
        {
            arcadeStatus = ArcadeOpeningStatus.Open;
            mainDoors.OpenDoor();
           // _timeLink.StopTimer();

            openOnce = true;
            closedOnce = false;
            
        }
        else if (_timeLink.CurrentHour == closingHour && !closedOnce)
        {
            arcadeStatus = ArcadeOpeningStatus.Closed;
            mainDoors.CloseDoor();
            // _timeLink.StopTimer();
            // enable UI element to show data of the day

            closedOnce = true;
            openOnce = false;
            _economyLink.ClosingTime();
        }
    }


    public void AddObjectToLists(GameObject objectToAdd)
    {
        if(objectToAdd.GetComponent<GameMachine>())
        {
            AllGameMachines.Add(objectToAdd.GetComponent<GameMachine>());
        }
        else if(objectToAdd.GetComponent<FoodMachine>())
        {
            AllFoodStalls.Add(objectToAdd.GetComponent<FoodMachine>());
        }
        else if (objectToAdd.GetComponent<ServiceMachine>())
        {
            AllToilets.Add(objectToAdd.GetComponent<ServiceMachine>());
        }

        if(customerManager)
        {
            customerManager.SetGameMachines(AllGameMachines);
            customerManager.SetFoodStalls(AllFoodStalls);
            customerManager.SetToilets(AllToilets);
        }
    }

    public void RemoveObjectFromLists(GameObject objectToRemove)
    {
        if (objectToRemove.GetComponent<GameMachine>())
        {
            AllGameMachines.Remove(objectToRemove.GetComponent<GameMachine>());
            AllGameMachines.TrimExcess();
        }
        else if (objectToRemove.GetComponent<FoodMachine>())
        {
            AllFoodStalls.Remove(objectToRemove.GetComponent<FoodMachine>());
            AllGameMachines.TrimExcess();
        }
        else if (objectToRemove.GetComponent<ServiceMachine>())
        {
            AllToilets.Remove(objectToRemove.GetComponent<ServiceMachine>());
            AllGameMachines.TrimExcess();
        }

        if (customerManager)
        {
            customerManager.SetGameMachines(AllGameMachines);
            customerManager.SetFoodStalls(AllFoodStalls);
            customerManager.SetToilets(AllToilets);
        }
    }

    public float StartingCash
    {
        get
        {
            return startingCash;
        }

        set
        {
            startingCash = value;
        }
    }
    public float RentCost
    {
        get
        {
            return rentCost;
        }

        set
        {
            rentCost = value;
        }
    }
    public List<Machine> AllMachineObjects
    {
        get
        {
            return allMachineObjects;
        }

        set
        {
            allMachineObjects = value;
        }
    }
    public List<Machine> AllGameMachines
    {
        get
        {
            return allGameMachines;
        }

        set
        {
            allGameMachines = value;
        }
    }
    public List<Machine> AllToilets
    {
        get
        {
            return allToilets;
        }

        set
        {
            allToilets = value;
        }
    }
    public List<Machine> AllFoodStalls
    {
        get
        {
            return allFoodStalls;
        }

        set
        {
            allFoodStalls = value;
        }
    }
}
