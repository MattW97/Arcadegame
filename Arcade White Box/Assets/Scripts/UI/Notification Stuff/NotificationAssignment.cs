using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationAssignment : MonoBehaviour {

    private string[] posArray, negArray, specArray;
    private List<Image> allIconImages;

    [SerializeField]
    private GameObject notificationBanner;

    private bool notificationOpen, animationOpen;
    private Animator anim;

    [SerializeField]
    private Text displayTextField;



   // [SerializeField]
   // private Image displayImageField;

    [SerializeField]
    private float lengthOfNotification, timeBetweenNotifications;

	// Use this for initialization
	void Start () {
        posArray = this.gameObject.GetComponent<NotificationLoader>().PosArray;
        negArray = this.gameObject.GetComponent<NotificationLoader>().NegArray;
        specArray = this.gameObject.GetComponent<NotificationLoader>().SpecArray;
        anim = this.GetComponent<Animator>();
        animationOpen = false;
        notificationOpen = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.J))
        {
            randomPosNotification();
        }
	}

    private void randomImage()
    {
        // displayImageField = allIconImages[Random.Range(0, allIconImages.Count)];
    }

    private IEnumerator PositiveNotificationTrigger()
    {
        displayTextField.text = posArray[Random.Range(0, posArray.Length)];
        PlayAnimation();
        yield return new WaitForSeconds(lengthOfNotification);
        PlayAnimation();
        notificationOpen = false;

    }

    private IEnumerator NegativeNotificationTrigger()
    {
        displayTextField.text = negArray[Random.Range(0, negArray.Length)];
        PlayAnimation();
        yield return new WaitForSeconds(lengthOfNotification);
        PlayAnimation();
        notificationOpen = false;
    }

    private IEnumerator SpecialNotificationTrigger()
    {
        displayTextField.text = specArray[Random.Range(0, specArray.Length)];
        PlayAnimation();
        yield return new WaitForSeconds(lengthOfNotification);
        PlayAnimation();
        notificationOpen = false;
    }

    private void PlayAnimation()
    {
        if (animationOpen)
        {
            anim.SetTrigger("Open");
            animationOpen = false;
        }
        else
        {
            anim.SetTrigger("Open");
            animationOpen = true;
        }
    }

    public bool randomPosNotification()
    {
        if (!notificationOpen)
        {
            StartCoroutine(PositiveNotificationTrigger());
            notificationOpen = true;
            return true;
        }
        else
            return false;

    }

    public bool randomNegNotification()
    {
        if (!notificationOpen)
        {
            StartCoroutine(NegativeNotificationTrigger());
            notificationOpen = true;
            return true;
        }
        else
            return false;

    }

    public bool randomSpecNotification()
    {
        if (!notificationOpen)
        {
            StartCoroutine(SpecialNotificationTrigger());
            notificationOpen = true;
            return true;
        }
        else
            return false;

    }

}
