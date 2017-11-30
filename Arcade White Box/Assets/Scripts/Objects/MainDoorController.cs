using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoorController : MonoBehaviour {

    private bool doorOpen;
    private Animator anim;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        doorOpen = false;
    }

    void Update()
    {
        
    }

    public void OpenDoor()
    {
        PlayAnimation();
    }

    public void CloseDoor()
    {
        // Tell all customers to leave
        // Check if all customers have left
        PlayAnimation();
    }

    public void OnButtonPressed()
    {
        if (doorOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    private void PlayAnimation()
    {
        if (doorOpen)
        {
            anim.SetTrigger("UnPlay");
            doorOpen = false;
            print("1");
        }
        else
        {
            anim.SetTrigger("Play");
            doorOpen = true;
            print("2");
        }
        
    }


}
