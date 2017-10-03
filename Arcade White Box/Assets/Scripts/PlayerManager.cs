using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private string playerName, arcadeName;
    [SerializeField] private float startingCash;

    [SerializeField] private Text cashText, earningsText, expensesText;

    private float currentCash, currentlyEarnedToday, currentExpenses;

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


    void Start () {

        CurrentCash = startingCash;
 
		
	}
	
	void Update () {
        UpdateCashText();
        UpdateEarningsText();
        UpdateExpensesText();
		
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
        if (CurrentCash - price > 0)
            return true;

        else
            return false;
    }

    public void OnPurchase(GameObject purchase)
    {
       BasicObject actualItem = purchase.gameObject.GetComponent<BasicObject>();
        CurrentCash -= actualItem.GameCost;
        CurrentExpenses += actualItem.RunningCost;
    }

    public void ClosingTime()
    {
        CurrentCash -= CurrentExpenses;
        CurrentlyEarnedToday = 0;
    }
 
    
}
