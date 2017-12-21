using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour {


    private PlayerManager _playerLink;
    private LevelManager _levelLink;
    private CustomerManager _customerLink;
    private TimeAndCalendar _timeAndCalendarLink;

    private float currentCash;
    private float rentCost;
    private float profitEntranceFees, profitDailyGamesMachines, profitDailyFoodStalls, profitDailyOther;
    private float expensesTodaysPurchases, expensesStaffWages, expensesGamesMachineDailyCost, expensesGamesMachineMaintenance, expensesFoodStallsDailyCost, expensesFoodStallsMaintenance, expensesServiceMachineDailyCost, expensesServiceMachineMaintenanceCost;

    private bool bankrupt;
    public GameObject bankruptcyUI;
    public GameObject UI;


    // Use this for initialization
    void Start () {
        rentCost = GameManager.Instance.ScriptHolderLink.GetComponent<LevelManager>().RentCost;
        _playerLink = this.GetComponent<PlayerManager>();
        _levelLink = GameManager.Instance.ScriptHolderLink.GetComponent<LevelManager>();
        _customerLink = GameManager.Instance.ScriptHolderLink.GetComponent<CustomerManager>();
        _timeAndCalendarLink = GameManager.Instance.ScriptHolderLink.GetComponent<TimeAndCalendar>();
        CurrentCash = _levelLink.StartingCash;
        Bankrupt = false;
        UI = GameObject.Find("UIInGame");
        bankruptcyUI = GameObject.Find("UIInGame/Bankruptcy");
        bankruptcyUI.SetActive(false);
        
	}
	
	// Update is called once per frame
	void Update () {

        if (currentCash <= 0 && !Bankrupt)
        {
            bankruptcyUI.SetActive(true);
            bankruptcyUI.GetComponent<BankruptcyUIController>().Warning();
            print("Bankrupt");
            _timeAndCalendarLink.StopTimer();
            Bankrupt = true;
        }
        else if(currentCash > 0)
        {
            Bankrupt = false;
        }
        if (currentCash % 1 != 0)
        {
            currentCash = Mathf.RoundToInt(currentCash);
        }
		
	}

    public bool CheckCanAfford(float cost)
    {
        float a = currentCash;
        if (0 <= (a -= cost))
            return true;
        else
            return false;
        
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

    public List<float> GetProfitArray()
    {
        List<float> profitArray = new List<float>();
        profitArray.Add(profitEntranceFees);
        profitArray.Add(profitDailyGamesMachines);
        profitArray.Add(profitDailyFoodStalls);
        profitArray.Add(profitDailyOther);

        return profitArray;
    }

    public void OnStaffHire(Staff staffMember)
    {
        expensesStaffWages += staffMember.WageCost;
    }

    public void OnStaffFire(Staff staffMember)
    {
        expensesStaffWages -= staffMember.WageCost;
    }

    public void OnMachineRepair(Machine brokenMachine)
    {
        MoneySpent(brokenMachine.MaintenanceCost);
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
        MoneySpent(purchase.BuyCost);
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
        MoneySpent(purchase.BuyCost);
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

    public float GetTotalProfits()
    {
        float exp = 0.0f;
        exp += profitEntranceFees;
        exp += profitDailyGamesMachines;
        exp += profitDailyFoodStalls;
        exp += profitDailyOther;
        return exp;
    }

    public void ClosingTime()
    {
        CurrentCash -= (GetTotalExpenses() - expensesTodaysPurchases);
        //CurrentCash -= dailyBankRepayment;
        //CurrentlyEarnedToday = 0;
        expensesServiceMachineMaintenanceCost = 0;
        expensesGamesMachineMaintenance = 0;
        expensesFoodStallsMaintenance = 0;
    }


    public void MoneyEarned(Machine objectSpentOn)
    {
        float num = objectSpentOn.UseCost;
        if (objectSpentOn is GameMachine)
        {
            profitDailyGamesMachines += num;
        }
        else if (objectSpentOn is FoodMachine)
        {
            profitDailyFoodStalls += num;
        }
        else
        {
            profitDailyOther += num;
        }
        CurrentCash += num;
        UI.GetComponent<ScrollingMoneyTextController>().CreatePositiveText(num);
    }
    public void MoneyEarned(float amount)
    {
        currentCash -= amount;
        UI.GetComponent<ScrollingMoneyTextController>().CreatePositiveText(amount);
    }

    public void MoneySpent(float amount)
    {
        CurrentCash -= amount;
        UI.GetComponent<ScrollingMoneyTextController>().CreateNegativeText(amount);
    }

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

    public bool Bankrupt
    {
        get
        {
            return bankrupt;
        }

        set
        {
            bankrupt = value;
        }
    }

}
