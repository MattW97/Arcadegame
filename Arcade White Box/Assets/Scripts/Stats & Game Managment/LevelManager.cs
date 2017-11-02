using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public enum ArcadeOpeningStatus { Open, Closed }
    public enum ArcadeTimeStatus { Day, Night }
    private ArcadeOpeningStatus arcadeOpeningStatus = ArcadeOpeningStatus.Closed;
    private ArcadeTimeStatus arcadeTimeStatus = ArcadeTimeStatus.Day;

    private List<GameObject> allObjectsInLevel, previousAllObjectsInLevel;
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


    private int numOfCustomers;
    private bool openOnce, closedOnce, spawningCustomers;

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

    public List<GameObject> AllObjectsInLevel
    {
        get
        {
            return allObjectsInLevel;
        }

        set
        {
            allObjectsInLevel = value;
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

    // Use this for initialization
    void Start () {

        _timeLink = this.gameObject.GetComponent<TimeAndCalendar>();
        _playerLink = this.gameObject.GetComponent<PlayerManager>();
        _economyLink = this.gameObject.GetComponent<EconomyManager>();
        customerManager = this.GetComponent<CustomerManager>();
        openOnce = false;
        closedOnce = false;

        customerManager.SetSpawnLocation(transform);

        AllObjectsInLevel = new List<GameObject>();
        previousAllObjectsInLevel = new List<GameObject>();
        AllMachineObjects = new List<Machine>();
        AllGameMachines = new List<Machine>();
        AllToilets = new List<Machine>();
        AllFoodStalls = new List<Machine>();

        spawningCustomers = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (_timeLink.CurrentHour == openingHour && !openOnce)
        {
            arcadeOpeningStatus = ArcadeOpeningStatus.Open;
            print("Arcade is open!");
            openOnce = true;
            closedOnce = false;
            //openingScript
        }
        else if (_timeLink.CurrentHour == closingHour && !closedOnce)
        {
            arcadeOpeningStatus = ArcadeOpeningStatus.Closed;
            print("Arcade is closed!");
            closedOnce = true;
            openOnce = false;
            _economyLink.ClosingTime();
        }

        if(AllGameMachines.Count > 0 && AllToilets.Count > 0 && AllFoodStalls.Count > 0 && !spawningCustomers)
        {
            spawningCustomers = true;
            customerManager.InvokeRepeating("SpawnCustomer", customerSpawnRate, customerSpawnRate);
        }

        if(customerManager.GetCustomerNumber() >= MAXCUSTOMERS)
        {
            customerManager.CancelInvoke("SpawnCustomer");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            print(allMachineObjects.Count);
        }
	}

    public void AddObjectToLists(GameObject objectToAdd)
    {
        AllObjectsInLevel.Add(objectToAdd);

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
        AllObjectsInLevel.Remove(objectToRemove);

        if (objectToRemove.GetComponent<GameMachine>())
        {
            AllGameMachines.Remove(objectToRemove.GetComponent<GameMachine>());
            AllGameMachines.TrimExcess();
        }
        else if (objectToRemove.GetComponent<FoodMachine>())
        {
            AllFoodStalls.Remove(objectToRemove.GetComponent<FoodMachine>());
            AllFoodStalls.TrimExcess();
        }
        else if (objectToRemove.GetComponent<ServiceMachine>())
        {
            AllToilets.Remove(objectToRemove.GetComponent<ServiceMachine>());
            AllToilets.TrimExcess();
        }

        if (customerManager)
        {
            customerManager.SetGameMachines(AllGameMachines);
            customerManager.SetFoodStalls(AllFoodStalls);
            customerManager.SetToilets(AllToilets);
        }
    }

    public void InstantiateLevel(List<GameObject> allObjects)
    {
        previousAllObjectsInLevel = AllObjectsInLevel;
        AddToLists(allObjects);
        //CheckNewObjects();
    }

    private void AddToLists(List<GameObject> allObjects)
    {
        foreach (GameObject go in allObjects)
        {
            AddObjectToLists(go);
        }
    }

    private void CheckNewObjects()
    {
        for (int i = 0; i < allObjectsInLevel.Count; i++)
        {
            if (!previousAllObjectsInLevel.Contains(allObjectsInLevel[i]))
            {
                Instantiate(allObjectsInLevel[i]);
            }
        }
    }

}
