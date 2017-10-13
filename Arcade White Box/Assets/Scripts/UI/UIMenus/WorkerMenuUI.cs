using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerMenuUI : MonoBehaviour {

    private int noOfWorkers;
    public int MAXNUMBEROFWORKERS;

    [SerializeField]
    private Text noOfWorkersTextField;

    [SerializeField]
    private Button minusButton, plusButton;

    public int NoOfWorkers
    {
        get
        {
            return noOfWorkers;
        }

        set
        {
            noOfWorkers = value;
        }
    }

    // Use this for initialization
    void Start () {
        noOfWorkers = 0;
        CheckWorkers();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void IncreaseWorkers()
    {
        NoOfWorkers++;
        //hire a worker
        noOfWorkersTextField.text = NoOfWorkers.ToString();
        CheckWorkers();
    }

    public void DecreaseWorkers()
    {
        NoOfWorkers--;
        //fire a worker
        noOfWorkersTextField.text = NoOfWorkers.ToString();
        CheckWorkers();
    }

    private void CheckWorkers()
    {
        if (NoOfWorkers == 0)
        {
            minusButton.interactable = false;
            plusButton.interactable = true;
        }

        else if (NoOfWorkers == MAXNUMBEROFWORKERS)
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
