using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefMenuUI : MonoBehaviour
{

    private int noOfChefs;
    public int MAXNUMBEROFCHEFS;

    [SerializeField]
    private Text noOfChefsTextField;

    [SerializeField]
    private Button minusButton, plusButton;

    public int NoOfChefs
    {
        get
        {
            return noOfChefs;
        }

        set
        {
            noOfChefs = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        noOfChefs = 0;
        CheckChefs();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseWorkers()
    {
        NoOfChefs++;
        //hire a chef
        noOfChefsTextField.text = NoOfChefs.ToString();
        CheckChefs();
    }

    public void DecreaseWorkers()
    {
        NoOfChefs--;
        //fire a chef
        noOfChefsTextField.text = NoOfChefs.ToString();
        CheckChefs();
    }

    private void CheckChefs()
    {
        if (NoOfChefs == 0)
        {
            minusButton.interactable = false;
            plusButton.interactable = true;
        }

        else if (NoOfChefs == MAXNUMBEROFCHEFS)
        {
            minusButton.interactable = true;
            plusButton.interactable = false;
        }
        else
        {
            minusButton.interactable = true;
            plusButton.interactable = true;
        }
    }
}
