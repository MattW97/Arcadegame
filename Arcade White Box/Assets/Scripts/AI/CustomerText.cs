using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerText : MonoBehaviour
{
    [SerializeField] private TextMesh customerText;
    [SerializeField] private float textAppearTime;

    private IEnumerator textCoroutine;

    void Start()
    {
        textCoroutine = ShowText("");
    }

    public void ShowFoodText()
    {
        StartCoroutine(ShowText("I'm starving over here!"));
    }

    public void ShowToiletText()
    {
        StartCoroutine(ShowText("I. Need. To. Poop."));
    }

    public void ShowExcitementText()
    {
        StartCoroutine(ShowText("I'm bored..."));
    }

    private IEnumerator ShowText(string newText)
    {
        customerText.text = newText;
        yield return new WaitForSeconds(textAppearTime);
        customerText.text = "";
        yield return null;
    }
}
