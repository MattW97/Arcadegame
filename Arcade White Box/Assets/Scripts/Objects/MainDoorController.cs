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
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenDoor();
        }
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
            anim.animatorecord
            anim.speed = -1;
            anim.SetTrigger("Play");
            doorOpen = false;
            print("1");
        }
        else
        {
            anim.speed = 1; 
            anim.SetTrigger("Play");
            doorOpen = true;
            print("2");
        }
        
    }


}
