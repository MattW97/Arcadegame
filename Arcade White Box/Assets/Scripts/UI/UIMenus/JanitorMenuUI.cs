using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JanitorMenuUI : MonoBehaviour
{

    private int noOfJanitors;
    public int MAXNUMBEROFJANITORS;

    [SerializeField]
    private Text noOfJanitorsTextField;

    [SerializeField]
    private Button minusButton, plusButton;

    public int NoOfJanitors
    {
        get
        {
            return noOfJanitors;
        }

        set
        {
            noOfJanitors = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        noOfJanitors = 0;
        CheckJanitors();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseJanitors()
    {
        NoOfJanitors++;
        //hire a worker
        noOfJanitorsTextField.text = NoOfJanitors.ToString();
        CheckJanitors();
    }

    public void DecreaseJanitors()
    {
        NoOfJanitors--;
        //fire a worker
        noOfJanitorsTextField.text = NoOfJanitors.ToString();
        CheckJanitors();
    }

    private void CheckJanitors()
    {
        if (NoOfJanitors == 0)
        {
            minusButton.interactable = false;
            plusButton.interactable = true;
        }

        else if (NoOfJanitors == MAXNUMBEROFJANITORS)
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
