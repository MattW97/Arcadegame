using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private string playerName, arcadeName;
    [SerializeField] private float bankLoanAmount;
    [SerializeField] private int bankRepaymentPercentage;

    private bool beenBankrupt;
    private float currentCash, currentlyEarnedToday, currentExpenses;
    private float amountOwedBank, dailyBankRepayment;
    private MainMenu Main;

    private EconomyManager _economyLink;



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
            //return Main.newName;
            return arcadeName;
        }

        set
        {
            //Main.newName = value;
            arcadeName = value;
        }
    }

    public bool BeenBankrupt
    {
        get
        {
            return beenBankrupt;
        }

        set
        {
            beenBankrupt = value;
        }
    }

    void Start () {

        _economyLink = this.GetComponent<EconomyManager>();
        BeenBankrupt = false;
        GameManager.Instance.GetComponent<SaveAndLoadManager>().LoadStats("Base");
		
	}
	
	void Update () {

        if (_economyLink.CurrentCash < 0)
        {
            //Bankruptcy();
        }
		
	}

    //private void Bankruptcy()
    //{
    //    if (BeenBankrupt)
    //    {
    //        //game over
    //        // switch to cutscene scene showing out of business 
    //        // explain to the player why they have failed
    //    }
    //    else
    //    {
    //        // set advisor note about bank giving you a loan 
    //        print("The bank has bailed you out. They will not do so again this year, if you go bankrupt again, you will lose your arcade!");
    //        amountOwedBank = CurrentCash;
    //        print("The bank has covered your expenses of " + -CurrentCash + " They have also credited you with " + bankLoanAmount + " ");
    //        amountOwedBank = -amountOwedBank;
    //        amountOwedBank += bankLoanAmount;
    //        print("You must pay the bank " + amountOwedBank);
    //        CurrentCash = 0;
    //        CurrentCash += bankLoanAmount;
    //        BeenBankrupt = true;
    //    }

    //}

    private void DeductBankPayment()
    {
        dailyBankRepayment = amountOwedBank / bankRepaymentPercentage;

    }


 
    
}
