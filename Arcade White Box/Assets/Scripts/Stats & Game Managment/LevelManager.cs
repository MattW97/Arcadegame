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
    private List<Machine> allBrokenMachines;

    private List<BaseAI> allStaff;

    private TimeAndCalendar _timeLink;
    private PlayerManager _playerLink;
    private CustomerManager customerManager;
    private EconomyManager _economyLink;

    [SerializeField] private float customerSpawnRate, rentCost, startingCash;
    [SerializeField] private int MAXCUSTOMERS; 
    [SerializeField] private int openingHour, closingHour, preOpeningHour;
    [SerializeField] private MainDoorController mainDoors;

    private int numOfCustomers;
    private bool openOnce, closedOnce, spawningCustomers, preOpenBool, doorAnimBool;

    void Start () {

        _timeLink = this.gameObject.GetComponent<TimeAndCalendar>();
        _playerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
        _economyLink = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        customerManager = this.GetComponent<CustomerManager>();
        openOnce = false;
        closedOnce = false;
        preOpenBool = false;
        doorAnimBool = false; 

        AllMachineObjects = new List<Machine>();
        AllGameMachines = new List<Machine>();
        AllToilets = new List<Machine>();
        AllFoodStalls = new List<Machine>();
        AllBrokenMachines = new List<Machine>();
        

        spawningCustomers = false;
    }
	
	void Update ()
    {
        OpenClose();
        if (CheckCustomerSpawnParameters())
        {
            customerManager.InvokeRepeating("SpawnCustomer", customerSpawnRate, customerSpawnRate);
            spawningCustomers = true;
        }
        

        if (customerManager.GetNumberOfCustomers() >= MAXCUSTOMERS)
        {
            customerManager.CancelInvoke("SpawnCustomer");
            spawningCustomers = false;
        }

        if (arcadeStatus == ArcadeOpeningStatus.Closed && customerManager.GetNumberOfCustomers() == 0 && doorAnimBool)
        {
            mainDoors.CloseDoor();
            //_timeLink.StartTimerX10();
            doorAnimBool = false;
        }
    }

    public bool CheckCustomerSpawnParameters()
    {
        if (AllGameMachines.Count > 0 && AllToilets.Count > 0 && AllFoodStalls.Count > 0 && !spawningCustomers && mainDoors.doorOpen)
        {
            spawningCustomers = true;
            //customerManager.InvokeRepeating("SpawnCustomer", customerSpawnRate, customerSpawnRate);
            return true;
        }
        return false;
    }

    private void OpenClose()
    {
        // pre open time
        if (_timeLink.GetCurrentTime() == preOpeningHour && !preOpenBool)
        {
            _timeLink.StopTimer();
            _timeLink.StartTimer();
            preOpenBool = true;
            
            // create UI stuff to show next day
        }

        //open time
        else if (_timeLink.CurrentHour == openingHour && !openOnce)
        {
           
            arcadeStatus = ArcadeOpeningStatus.Open;
            mainDoors.OpenDoor();

            openOnce = true;
            preOpenBool = false;
            closedOnce = false;
            doorAnimBool = true;

        }
        //close time
        else if (_timeLink.CurrentHour == closingHour && !closedOnce)
        {
            if (_economyLink.Bankrupt == true)
            {
                _timeLink.StopTimer();
                _economyLink.bankruptcyUI.SetActive(true);
                _economyLink.bankruptcyUI.GetComponent<BankruptcyUIController>().Bankrupt();
            }
            customerManager.MassLeave();
            arcadeStatus = ArcadeOpeningStatus.Closed;
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

    public void MachineDailyReset()
    {
        foreach (Machine mach in AllMachineObjects)
        {
            mach.DailyReset();
        }
    }

    #region Getters/Setters
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

    public List<Machine> AllBrokenMachines
    {
        get
        {
            return allBrokenMachines;
        }

        set
        {
            allBrokenMachines = value;
        }
    }

    public int PreOpeningHour
    {
        get
        {
            return preOpeningHour;
        }

        set
        {
            preOpeningHour = value;
        }
    }

    #endregion Getters/Setters
}
