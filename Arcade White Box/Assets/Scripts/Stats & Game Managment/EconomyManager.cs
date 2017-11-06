using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour {


    private PlayerManager _playerLink;
    private LevelManager _levelLink;
    private CustomerManager _customerLink;

    private float currentCash;
    private float rentCost;
    private float profitEntranceFees, profitDailyGamesMachines, profitDailyFoodStalls, profitDailyOther;
    private float expensesTodaysPurchases, expensesStaffWages, expensesGamesMachineDailyCost, expensesGamesMachineMaintenance, expensesFoodStallsDailyCost, expensesFoodStallsMaintenance, expensesServiceMachineDailyCost, expensesServiceMachineMaintenanceCost;

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

    // Use this for initialization
    void Start () {
        rentCost = this.GetComponent<LevelManager>().RentCost;
        _playerLink = this.GetComponent<PlayerManager>();
        _levelLink = this.GetComponent<LevelManager>();
        _customerLink = this.GetComponent<CustomerManager>();
        CurrentCash = _levelLink.StartingCash;
        
	}
	
	// Update is called once per frame
	void Update () {
		
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
        CurrentCash -= purchase.BuyCost;
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
        CurrentCash -= purchase.BuyCost;
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
        expensesServiceMachineMaintenanceCost = 0;
        expensesGamesMachineMaintenance = 0;
        expensesFoodStallsMaintenance = 0;

        //CurrentCash -= CurrentExpenses;
        //CurrentCash -= dailyBankRepayment;
        //CurrentlyEarnedToday = 0;
    }

    public bool CheckCanAfford(float price)
    {
        if (CurrentCash - price > -1)
            return true;

        else
            return false;
    }

    public void MoneyEarnedFromArcade(Machine objectSpentOn)
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
    }

}
