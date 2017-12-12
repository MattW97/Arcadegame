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
        GameManager.Instance.GetComponent<SaveAndLoadManager>().LoadBaseStats("Base");
		
	}
	
	void Update () {
		
	}



    private void DeductBankPayment()
    {
        dailyBankRepayment = amountOwedBank / bankRepaymentPercentage;

    }


 
    
}
