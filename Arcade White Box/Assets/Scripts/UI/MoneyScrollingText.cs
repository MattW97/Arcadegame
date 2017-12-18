using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyScrollingText : MonoBehaviour {

    [SerializeField] private Color[] colors;
    [SerializeField] private Text text;
    [SerializeField] private float fadeTime;

    private Color fadedColour;
    private IEnumerator fadeOut;

	// Use this for initialization
	void Start () {
        fadeOut = FadeOut();
        StartCoroutine(fadeOut);
        fadedColour = Color.green;
        fadedColour.a = 0;
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position += new Vector3(0, 0.2f);
        
		
	}

    public void AssignText(float amount, bool positive)
    {
        text.text = amount.ToString();
        if (positive)
            text.color = Color.green;
        else
            text.color = Color.black;
    }

    private IEnumerator FadeOut()
    {
        //ugly while, Update would be ideal
        while (text.color.a > 0)
        {
            text.color = Color.Lerp(text.color, fadedColour, fadeTime * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
