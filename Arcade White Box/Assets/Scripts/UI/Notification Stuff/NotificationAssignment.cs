using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationAssignment : MonoBehaviour {

    private string[] posArray, negArray, specArray;
    private List<Image> allIconImages;

    [SerializeField]
    private GameObject notificationBanner;

    [SerializeField]
    private Text displayTextField;

   // [SerializeField]
   // private Image displayImageField;

    [SerializeField]
    private float lengthOfNotification, timeBetweenNotifications;

	// Use this for initialization
	void Start () {
        posArray = this.GetComponent<NotificationLoader>().PosArray;
        negArray = this.GetComponent<NotificationLoader>().NegArray;
        specArray = this.GetComponent<NotificationLoader>().SpecArray;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void randomImage()
    {
        // displayImageField = allIconImages[Random.Range(0, allIconImages.Count)];
    }

    public void positiveNotificationTrigger()
    {
        displayTextField.text = posArray[Random.Range(0, posArray.Length)];
        //randomImage();
    }

    public void negativeNotificationTrigger()
    {
        displayTextField.text = negArray[Random.Range(0, negArray.Length)];
       // randomImage();
    }

    public void specialNotificationTrigger()
    {
        // might to specific messages here. Will have to figure out a way of numbering them. Possibly just use the array ordering?
        displayTextField.text = specArray[Random.Range(0, specArray.Length)];
        // maybe a special image here? warning image/event based?
    }
}
