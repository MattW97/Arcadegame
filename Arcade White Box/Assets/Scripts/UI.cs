using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {

    public Animator animator;
    private bool Open = false;

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Open);
    }

    public void BuildOpen()
    {
        if (!Open)
        {
            Open = true;
            
            animator.SetBool("Build Open", true);
            //animator.SetBool("Edit Close", true);
            //animator.SetBool("Staff Close", true);
            //animator.SetBool("Stats Close", true);
            //animator.SetBool("Option Close", true);
        }

    }

    public void EditOpen()
    {
        animator.SetBool("Build Close", true);
        //animator.SetBool("Edit Open", true);
        //animator.SetBool("Staff Close", true);
        //animator.SetBool("Stats Close", true);
        //animator.SetBool("Option Close", true);
    }
    public void StaffOpen()
    {
        animator.SetBool("Build Close", true);
        animator.SetBool("Edit Close", true);
        animator.SetBool("Staff Open", true);
        animator.SetBool("Stats Close", true);
        animator.SetBool("Option Close", true);
    }

    public void StatsOpen()
    {
        animator.SetBool("Build Close", true);
        animator.SetBool("Edit Close", true);
        animator.SetBool("Staff Close", true);
        animator.SetBool("Stats Open", true);
        animator.SetBool("Option Close", true);
    }

    public void OptionsOpen()
    {
        animator.SetBool("Build Close", true);
        animator.SetBool("Edit Close", true);
        animator.SetBool("Staff Close", true);
        animator.SetBool("Stats Close", true);
        animator.SetBool("Option Open", true);
    }



    //if (Open)
    //{
    //    animator.SetBool("Open", false);
    //    animator.SetBool("Close", true);
    //    Open = true;
    //}
    //else
    //{
    //    animator.SetBool("Open", true);
    //    animator.SetBool("Close", false);
    //    Open = false;
    //}
    //Debug.Log("Test");
    //animator.Play("BuyMenu");
    // }

    public void BuildClose()
    {
        animator.SetBool("Build Open", false);
        animator.SetBool("Build Close", true);
        //Open = false;



    }
}
