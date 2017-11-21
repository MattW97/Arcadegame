using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEventManager : MonoBehaviour {

    private int advertisingPercentage;
    private int customerHappinessPercentage;
    private int customerExcitementPercentage;
    private int customerHungerPercentage;
    private int playerExpensesPercentage;
    private int playerRevenuePercentage;
    private int playerBuyingMachinesPercentage;

	// Use this for initialization
	void Start () {
        advertisingPercentage = 100;
        customerHappinessPercentage = 100;
        customerExcitementPercentage = 100;
        customerHungerPercentage = 100;
        playerExpensesPercentage = 100;
        playerRevenuePercentage = 100;
        playerBuyingMachinesPercentage = 100;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}

public class InGameEvent
{
    public string eventName;
    public string eventDescription;
    public int advertisingPercentage;
    public int customerHappinessPercentage;
    public int customerExcitementPercentage;
    public int customerHungerPercentage;
    public int playerExpensesPercentage;
    public int playerRevenuePercentage;
    public int playerBuyingMachinesPercentage;
}
