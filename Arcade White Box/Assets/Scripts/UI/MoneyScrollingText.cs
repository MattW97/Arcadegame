using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyScrollingText : MonoBehaviour {

    [SerializeField] private Color[] colors;
    [SerializeField] private Text text;
    [SerializeField] private float fadeTime;

    private Color fadedGreenColour, fadedRedColour;
    private IEnumerator fadeOut;

	// Use this for initialization
	void Start () {
        
        StartCoroutine(fadeOut);
        fadedGreenColour = Color.green;
        fadedGreenColour.a = 0;
        fadedRedColour = Color.green;
        fadedRedColour.a = 0;
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position += new Vector3(0, 0.2f);
        
		
	}

    public void AssignText(float amount, bool positive)
    {
        if (positive)
        {
            text.text = "+$" + amount.ToString();
            text.color = Color.green;
            fadeOut = FadeOut(fadedGreenColour);
        }
        else
        {
            text.text = "-$" + amount.ToString();
            text.color = Color.red;
            fadeOut = FadeOut(fadedRedColour);
        }
    }

    private IEnumerator FadeOut(Color color)
    {
        while (text.color.a > 0)
        {
            text.color = Color.Lerp(text.color, color, fadeTime * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
