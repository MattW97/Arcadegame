using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStatUIController : MonoBehaviour {

    [SerializeField]
    private Text dailyRevenueText, allTimeRevenueText, expensesText, dailyCustomersText, allTimeCustomersText, machineBreakdownText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateUI(Machine currentObject)
    {
        if (currentObject is Machine)
        {
            dailyRevenueText.text = currentObject.dailyRevenue.ToString();
            allTimeRevenueText.text = currentObject.allTimeRevenue.ToString();
            expensesText.text = currentObject.allTimeExpenses.ToString();
            dailyCustomersText.text = currentObject.dailyCustomers.ToString();
            allTimeCustomersText.text = currentObject.allTimeCustomers.ToString();
            machineBreakdownText.text = currentObject.noOfBreakdowns.ToString();
        }
    }
}
