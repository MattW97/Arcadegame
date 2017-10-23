using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpensesMenuUI : MonoBehaviour {



    [SerializeField]
    private GameObject pieGraphObject;

    [SerializeField]
    private Text expensesText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateGraph()
    {
        expensesText.text = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().GetTotalExpenses().ToString();
        pieGraphObject.GetComponent<PieGraph>().SetNewValues(GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().GetExpensesArray());
        pieGraphObject.GetComponent<PieGraph>().MakeGraph();
    }
}
