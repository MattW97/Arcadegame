using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AdvisorMenu : MonoBehaviour {

    [SerializeField]
    private Text advisorText;
    [SerializeField]
    private Image advisorIcon;
    [SerializeField]
    private float advisorPopupLength;
    [SerializeField]
    private GameObject advisorMenu;
    public Sprite testImage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(SetupAdvisor("this is a test", testImage));
        }
	}



    private void SetAdvisorText(string text)
    {
        advisorText.text = text;
    }

    private void SetAdvisorIcon(Sprite image)
    {
        advisorIcon.sprite = image;
    }

    public IEnumerator SetupAdvisor(string advisorText, Sprite advisorImage)
    {
        SetAdvisorText(advisorText);
        SetAdvisorIcon(advisorImage);
        advisorMenu.SetActive(true);
        yield return new WaitForSeconds(advisorPopupLength);
        advisorMenu.SetActive(false);
    }
}
