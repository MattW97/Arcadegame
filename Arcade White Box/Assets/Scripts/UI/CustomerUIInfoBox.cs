using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUIInfoBox : MonoBehaviour {

    [SerializeField]
    private Image happinessBar, hungerBar, fatigueBar, bladderBar;

    [SerializeField]
    private Text name;

    public void UpdateUI(Customer currentCustomer)
    {
        happinessBar.fillAmount = currentCustomer.HappinessStat / 100;
        hungerBar.fillAmount = currentCustomer.HungerStat / 100;
        fatigueBar.fillAmount = currentCustomer.TirednessStat / 100;
        bladderBar.fillAmount = currentCustomer.BladderStat / 100;
        name.text = currentCustomer.CustomerName;
    }
}
