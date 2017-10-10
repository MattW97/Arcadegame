using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private string playerName, arcadeName;
    [SerializeField] private float startingCash;

    [SerializeField] private Text cashText, earningsText, expensesText;

    private float currentCash, currentlyEarnedToday, currentExpenses;

    private MainMenu Main;

    public float CurrentCash
    {
        get
        {
            return currentCash;
        }

        set
        {
            currentCash = value;
        }
    }

    public float CurrentlyEarnedToday
    {
        get
        {
            return currentlyEarnedToday;
        }

        set
        {
            currentlyEarnedToday = value;
        }
    }

    public float CurrentExpenses
    {
        get
        {
            return currentExpenses;
        }

        set
        {
            currentExpenses = value;
        }
    }

    public string PlayerName
    {
        get
        {
            return playerName;
        }

        set
        {
            playerName = value;
        }
    }

    public string ArcadeName
    {
        get
        {
            return Main.newName;
            //return arcadeName;
        }

        set
        {
            Main.newName = value;
            //arcadeName = value;
        }
    }

    void Start () {

        print("PLAYER MANAGER");
        CurrentCash = startingCash;
		
	}
	
	void Update () {
        //UpdateCashText();
        //UpdateEarningsText();
       // UpdateExpensesText();
		
	}

    private void UpdateCashText()
    {
        cashText.text = CurrentCash.ToString();
    }

    private void UpdateEarningsText()
    {
        earningsText.text = CurrentlyEarnedToday.ToString();
    }

    private void UpdateExpensesText()
    {
        expensesText.text = CurrentExpenses.ToString();
    }

    public bool CheckCanAfford(float price)
    {
        if (CurrentCash - price > -1)
            return true;

        else
            return false;
    }

    // when a machine has been purchased;
    public void OnMachinePurchase(Machine purchase)
    {
        CurrentCash -= purchase.BuyCost;
        currentExpenses += purchase.RunningCost;
    }

    public void OnBuildingPartPurchase(PlaceableObject purchase)
    {
        CurrentCash -= purchase.BuyCost;
    }

    public void ClosingTime()
    {
        CurrentCash -= CurrentExpenses;
        CurrentlyEarnedToday = 0;
    }
 
    
}
