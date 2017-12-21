using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingMoneyTextController : MonoBehaviour {

    [SerializeField] private GameObject parent;
    [SerializeField] private Text scrollingText;

    [SerializeField] [Range(0.0f, 10000.0f)] private float minimumEarningsToShow;
    [SerializeField] [Range(0.0f, 10000.0f)] private float minimumExpensesToShow;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CreatePositiveText(100);
        }
	}

    public void CreatePositiveText(float amount)
    {
        if (amount >= minimumEarningsToShow)
        {
            Text newText = Instantiate(scrollingText, scrollingText.transform.position, scrollingText.transform.rotation) as Text;
            newText.transform.SetParent(parent.transform, false);
            newText.GetComponent<MoneyScrollingText>().AssignText(amount, true);
        }
    }

    public void CreateNegativeText(float amount)
    {
        if (amount >= minimumExpensesToShow)
        {
            Text newText = Instantiate(scrollingText, scrollingText.transform.position, scrollingText.transform.rotation) as Text;
            newText.transform.SetParent(parent.transform, false);
            newText.GetComponent<MoneyScrollingText>().AssignText(amount, false);
        }
    }
}
