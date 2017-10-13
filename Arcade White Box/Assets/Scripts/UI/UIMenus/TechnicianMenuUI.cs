using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnicianMenuUI : MonoBehaviour
{

    private int noOfTechnicians;
    public int MAXNUMBEROFTECHNICIANS;

    [SerializeField]
    private Text noOfTechniciansTextField;

    [SerializeField]
    private Button minusButton, plusButton;

    public int NoOfTechnicians
    {
        get
        {
            return noOfTechnicians;
        }

        set
        {
            noOfTechnicians = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        noOfTechnicians = 0;
        CheckTechnicians();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseTechnicians()
    {
        NoOfTechnicians++;
        //hire a worker
        noOfTechniciansTextField.text = NoOfTechnicians.ToString();
        CheckTechnicians();
    }

    public void DecreaseTechnicians()
    {
        NoOfTechnicians--;
        //fire a worker
        noOfTechniciansTextField.text = NoOfTechnicians.ToString();
        CheckTechnicians();
    }

    private void CheckTechnicians()
    {
        if (NoOfTechnicians == 0)
        {
            minusButton.interactable = false;
            plusButton.interactable = true;
        }

        else if (NoOfTechnicians == MAXNUMBEROFTECHNICIANS)
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
