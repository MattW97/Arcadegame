using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : BaseAI
{
    [SerializeField] private CustomerStat.Stats weakness;

    public string prefabName;

    public enum CustomerStates { Idle, GotTarget, Moving, UsingFacility, Leaving, Left, Wandering, BeginningWander}

    private int weakStat;
    private float speedFactor, statCounter;
    private CustomerStates currentState;
    private IEnumerator usingFacilityWait;
    private Transform customerTransform;
    private Transform spawnLocation;
    private Unit unitController;
    private Machine currentTarget;
    private List<CustomerStat> customerStats;

    private CustomerStat bladderStat;
    private CustomerStat happinessStat;
    private CustomerStat hungerStat;
    private CustomerStat tirednessStat;
    private CustomerStat queasinessStat;

    private const float STAT_LIMIT = 100.0f;            //THE LIMIT A STAT CAN REACH
    private const float STAT_TICK_AMOUNT = 0.1f;        //THE AMOUNT THE STAT IS AFFECTED EACH TICK
    private const float STAT_TICK_RATE = 0.5f;          //HOW OFTEN THE STAT TICKS (IN SECONDS)

    private string customerName;

    void OnEnable() { EventManager.Save += OnSave; }

    void OnDisable() { EventManager.Save -= OnSave; }

    void Awake()
    {
        unitController = GetComponent<Unit>();
        customerTransform = GetComponent<Transform>();

        // INITIALISE ALL STATS, FIND WEAKNESS AND AJUST WEAKNESS VALUES -----------------
        bladderStat = new CustomerStat(CustomerStat.Stats.Bladder,          50.0f, 0.0f);
        happinessStat = new CustomerStat(CustomerStat.Stats.Happiness,      50.0f, 0.0f);
        hungerStat = new CustomerStat(CustomerStat.Stats.Hunger,            50.0f, 0.0f);
        tirednessStat = new CustomerStat(CustomerStat.Stats.Tiredness,      50.0f, 0.0f);
        queasinessStat = new CustomerStat(CustomerStat.Stats.Queasiness,    50.0f, 0.0f);

        customerStats = new List<CustomerStat>();
        customerStats.Add(bladderStat);
        customerStats.Add(happinessStat);
        customerStats.Add(hungerStat);
        customerStats.Add(tirednessStat);
        customerStats.Add(queasinessStat);

        for (int i = 0; i < customerStats.Count; i++)
        {
            if (customerStats[i].GetStatType() == weakness)
            {
                customerStats[i].StatValue = 100.0f;
                customerStats[i].Susceptibility = 15.0f;
                weakStat = i;
                //SEND NUDES
            }
        }
        // -------------------------------------------------------------------------------

        statCounter = 0.0f;
        currentState = CustomerStates.Idle;
        CustomerName = GameManager.Instance.GetComponent<NameGenerator>().GenerateName();
    }

    public void UpdateCustomer()
    {
        int i = 0;
        i++;
        print("I");
        unitController.SpeedFactor = speedFactor;

        if (currentState == CustomerStates.GotTarget)
        {
            unitController.StopCurrentPathing();
            unitController.SetTarget(currentTarget.GetUsePosition());
            unitController.GetNewPath();
            currentState = CustomerStates.Moving;
        }
        else if (currentState == CustomerStates.Moving)
        {
            if (unitController.ReachedTarget)
            {
                currentState = CustomerStates.UsingFacility;
                usingFacilityWait = UsingFacilityWait(currentTarget.UseTime);
                StartCoroutine(usingFacilityWait);
            }
        }
        else if (currentState == CustomerStates.Leaving)
        {
            if (unitController.ReachedTarget)
            {
                currentState = CustomerStates.Left;
            }
        }
        else if (currentState == CustomerStates.BeginningWander)
        {

        }
        else if (currentState == CustomerStates.Wandering)
        {
            if(unitController.ReachedTarget)
            {
                currentState = CustomerStates.Idle;
            }
        }
    }

    public void StatTick()
    {
        statCounter += Time.deltaTime * speedFactor;

        if (statCounter >= STAT_TICK_RATE)
        {
            if(happinessStat.StatValue > 0)
            {
                happinessStat.StatValue -= STAT_TICK_AMOUNT;
                tirednessStat.StatValue += STAT_TICK_AMOUNT;
                bladderStat.StatValue += STAT_TICK_AMOUNT;
                hungerStat.StatValue += STAT_TICK_AMOUNT;
            }

            statCounter = 0.0f;
        }
    }

    public void SetNewTarget(Machine newTarget)
    {
        this.currentTarget = newTarget;
        currentState = CustomerStates.GotTarget;
    }

    public void LeaveArcade()
    {
        if(currentState == CustomerStates.UsingFacility)
        {
            StopCoroutine(usingFacilityWait);
        }

        if(currentTarget)
        {
            currentTarget.InUse = false;
        }

        unitController.StopCurrentPathing();
        unitController.SetTarget(spawnLocation);
        unitController.GetNewPath();

        currentState = CustomerStates.Leaving;
    }

    public void DropTrash()
    {

    }

    public void Puke()
    {

    }

    public bool IsLeaving()
    {
        if(currentState == CustomerStates.Leaving || currentState == CustomerStates.Left)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool WantsToLeave()
    {
        if(happinessStat.StatValue <= 0.0f || tirednessStat.StatValue >= 100.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsBusy()
    {
        if (currentState == CustomerStates.Leaving)
        {
            return true;
        }
        else if (currentState == CustomerStates.GotTarget)
        {
            return true;
        }
        else if (currentState == CustomerStates.UsingFacility)
        {
            return true;
        }
        else if (currentState == CustomerStates.Moving)
        {
            return true;
        }
        else if (currentState == CustomerStates.Wandering)
        {
            return true;
        }
        else if (currentState == CustomerStates.BeginningWander)
        {
            return true;
        }
        else if (currentState == CustomerStates.Left)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Tile GetTileBelow()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(new Vector3(customerTransform.position.x, customerTransform.position.y + 1, customerTransform.position.z), -customerTransform.up, out hitInfo))
        {
            if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
            {
                return hitInfo.collider.GetComponent<Tile>();
            }
            else
            {
                print("THERE IS SOMETHING ELSE UNDER THIS CUSTOMER, SOMETINGZ FUCKD");
                return null;
            }
        }
        else
        {
            print("THERE IS NOTHING BELOW THIS CUSTOMER, SOMETINGZ FUCKD");
            return null;
        }
    }

    private IEnumerator UsingFacilityWait(float waitTime)
    {
        if (currentTarget is GameMachine)
        {
            GameMachine machine = (GameMachine)currentTarget;
            machine.IncreaseCurrentNumberOfPlayers();
        }

        currentTarget.OnUse();

        customerTransform.LookAt(currentTarget.transform);
        customerTransform.position = currentTarget.GetUsePosition().position;

        yield return new WaitForSeconds(waitTime / speedFactor);

        if (currentTarget is GameMachine)
        {
            GameMachine machine = (GameMachine)currentTarget;
            machine.DecreaseCurrentNumberOfPlayers();
        }

        currentTarget.InUse = false;

        currentState = CustomerStates.Idle;
    }

    public void SetCustomerNeeds(float bladder, float happiness, float hunger, float tiredness, float queasiness, int weakStat)
    {
        bladderStat = new CustomerStat(CustomerStat.Stats.Bladder, bladder, 0.0f);
        happinessStat = new CustomerStat(CustomerStat.Stats.Happiness, happiness, 0.0f);
        hungerStat = new CustomerStat(CustomerStat.Stats.Hunger, hunger, 0.0f);
        tirednessStat = new CustomerStat(CustomerStat.Stats.Tiredness, tiredness, 0.0f);
        queasinessStat = new CustomerStat(CustomerStat.Stats.Queasiness, queasiness, 0.0f);

        customerStats = new List<CustomerStat>();
        customerStats.Add(bladderStat);
        customerStats.Add(happinessStat);
        customerStats.Add(hungerStat);
        customerStats.Add(tirednessStat);
        customerStats.Add(queasinessStat);

        customerStats[weakStat].Susceptibility = 15.0f;
    }

    public float BladderStat        { get { return bladderStat.StatValue; } set { bladderStat.StatValue = value; } }
    public float HappinessStat      { get { return happinessStat.StatValue; } set { happinessStat.StatValue = value; } }
    public float HungerStat         { get { return hungerStat.StatValue; } set { hungerStat.StatValue = value; } }
    public float TirednessStat      { get { return tirednessStat.StatValue; } set { tirednessStat.StatValue = value; } }
    public float QueasinessStat     { get { return queasinessStat.StatValue; } set { queasinessStat.StatValue = value; } }

    public List<CustomerStat> GetCustomerStats()                    { return customerStats; }
    public CustomerStates GetCurrentCustomerState()                 { return currentState; }
    public Unit GetUnitController()                                 { return unitController;  }

    public void SetCurrentCustomerState(CustomerStates newState)    { currentState = newState; }
    public void SetSpawnLocation(Transform spawnLocation)           { this.spawnLocation = spawnLocation; }
    public float SpeedFactor                                        { get { return speedFactor; } set { speedFactor = value; } }

    public string CustomerName
    {
        get
        {
            return customerName;
        }

        set
        {
            customerName = value;
        }
    }

    private CustomerSaveable GetCustomerSaveable()
    {
        CustomerSaveable customerSave = new CustomerSaveable();

        customerSave.bladderStat = bladderStat.StatValue;
        customerSave.happinessStat = happinessStat.StatValue;
        customerSave.hungerStat = hungerStat.StatValue;
        customerSave.tirednessStat = tirednessStat.StatValue;
        customerSave.queasinessStat = queasinessStat.StatValue;

        customerSave.weakStat = this.weakStat;

        customerSave.PosX = this.customerTransform.position.x;
        customerSave.PosY = this.customerTransform.position.y;
        customerSave.PosZ = this.customerTransform.position.z;

        customerSave.RotX = this.customerTransform.rotation.x;
        customerSave.RotY = this.customerTransform.rotation.y;
        customerSave.RotZ = this.customerTransform.rotation.z;

        customerSave.prefabName = this.prefabName;

        return customerSave;
    }

    private void OnSave()
    {
        GameManager.Instance.GetComponent<SaveAndLoadManager>().saveData.customerSaveList.Add(GetCustomerSaveable());
    }
}

[System.Serializable]
public class CustomerSaveable
{
    public string prefabName;

    public float    hungerStat,
                    bladderStat,
                    happinessStat,
                    tirednessStat,
                    queasinessStat;

    public int weakStat;

    public float PosX, PosY, PosZ;
    public float RotX, RotY, RotZ;
}
