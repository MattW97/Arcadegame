using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfitMenuUI : MonoBehaviour {

    [SerializeField]
    private GameObject pieGraphObject;

    [SerializeField]
    private Text profitText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateGraph()
    {
        profitText.text = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().GetTotalProfits().ToString();
        pieGraphObject.GetComponent<PieGraph>().SetNewValues(GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().GetProfitArray());
        pieGraphObject.GetComponent<PieGraph>().MakeGraph();
    }
}
