using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public enum ArcadeOpeningStatus { Open, Closed }
    private ArcadeOpeningStatus arcadeStatus = ArcadeOpeningStatus.Closed;

    private List<PlaceableObject> allObjectsInLevel;
    private List<Machine> allMachineObjects; //change to type machineObject

    private List<BaseAI> allStaff;

    private TimeAndCalendar _timeLink;
    private PlayerManager _playerLink;

    [SerializeField] private float rentCost;
    [SerializeField] private int openingHour, closingHour;


    private int numOfCustomers, MAXNUMBEROFCUSTOMERS;
    private bool openOnce, closedOnce;

	// Use this for initialization
	void Start () {

        _timeLink = this.gameObject.GetComponent<TimeAndCalendar>();
        _playerLink = this.gameObject.GetComponent<PlayerManager>();
        openOnce = false;
        closedOnce = false;
		
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

        else if(_timeLink.CurrentHour == closingHour && !closedOnce)
        {
            arcadeStatus = ArcadeOpeningStatus.Closed;
            print("Arcade is closed!");
            closedOnce = true;
            openOnce = false;
            ClosingTime();
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
    }
}
