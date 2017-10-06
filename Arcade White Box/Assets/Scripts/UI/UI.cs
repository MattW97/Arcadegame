using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    
    [SerializeField]
    private GameObject buyMenu, editMenu, staffMenu, statsMenu, optionsMenu;

    private Animator buildAnim, editAnim, staffAnim, statsAnim, optionsAnim;

    // Use this for initialization
    void Start () {

        buildAnim = buyMenu.GetComponent<Animator>();
        editAnim = editMenu.GetComponent<Animator>();
        staffAnim = staffMenu.GetComponent<Animator>();
        statsAnim = statsMenu.GetComponent<Animator>();
        optionsAnim = optionsMenu.GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void BuildOpen()
    {
       // closeEverything();
        buildAnim.SetTrigger("BuildTrigger");
    }

    public void EditOpen()
    {
        closeEverything();
        editAnim.SetTrigger("EditTrigger");
    }
    public void StaffOpen()
    {
        closeEverything();
        staffAnim.SetTrigger("StaffTrigger");
    }

    public void StatsOpen()
    {
        closeEverything();
        statsAnim.SetTrigger("StatsTrigger");
    }

    public void OptionsOpen()
    {
        closeEverything();
        optionsAnim.SetTrigger("OptionsTrigger");
    }



    public void BuildClose()
    {
        buildAnim.SetTrigger("CloseTrigger");
    }

    public void EditClose()
    {
        editAnim.SetTrigger("CloseTrigger");
    }

    public void StaffClose()
    {
        staffAnim.SetTrigger("CloseTrigger");
    }

    public void StatsClose()
    {
        statsAnim.SetTrigger("CloseTrigger");
    }

    public void OptionsClose()
    {
        optionsAnim.SetTrigger("CloseTrigger");
    }

    //closes all open menus
    private void closeEverything()
    {
        BuildClose();
        EditClose();
        StaffClose();
        StatsClose();
        OptionsClose();
    }
}
