using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{

    [SerializeField]
    private GameObject gameTab, servicesTab, constructionTab, decorationTab; // optionsMenu;

    private bool tabOpen;
    private Animator anim;

    //[SerializeField]
    //private GameObject joystickMenu, friesMenu, stoolMenu, weirdThingAtTheBottomMenu, advisorMenu;

    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameTab()
    {
        
        PlayAnimation();
        gameTab.SetActive(true);
        servicesTab.SetActive(false);
        constructionTab.SetActive(false);
        decorationTab.SetActive(false);
        
    }

    public void ServicesTab()
    {
        PlayAnimation();
        servicesTab.SetActive(true);
        gameTab.SetActive(false);
        constructionTab.SetActive(false);
        decorationTab.SetActive(false);
    }

    public void ConstructionTab()
    {
        PlayAnimation();
        constructionTab.SetActive(true);
        gameTab.SetActive(false);
        servicesTab.SetActive(false);
        decorationTab.SetActive(false);        
    }

    public void DecorationTab()
    {
        PlayAnimation();
        decorationTab.SetActive(true);
        gameTab.SetActive(false);
        servicesTab.SetActive(false);
        constructionTab.SetActive(false);
    }

    //public void OptionsMenu()
    //{
    //    if (optionsMenu.activeInHierarchy)
    //    {
    //        optionsMenu.SetActive(false);
    //    }
    //    else
    //    {
    //        optionsMenu.SetActive(true);
    //    }
    //}

    //public void JoyStickMenu()
    //{
    //    if (optionsMenu.activeInHierarchy)
    //    {
    //        optionsMenu.SetActive(false);
    //    }
    //    else
    //    {
    //        optionsMenu.SetActive(true);
    //    }
    //}

    //public void FriesMenu()
    //{
    //    if (friesMenu.activeInHierarchy)
    //    {
    //        friesMenu.SetActive(false);
    //    }
    //    else
    //    {
    //        friesMenu.SetActive(true);
    //    }
    //}

    //public void StoolMenu()
    //{
    //    if (stoolMenu.activeInHierarchy)
    //    {
    //        stoolMenu.SetActive(false);
    //    }
    //    else
    //    {
    //        stoolMenu.SetActive(true);
    //    }
    //}

    //public void WeirdThingMenu()
    //{
    //    if (weirdThingAtTheBottomMenu.activeInHierarchy)
    //    {
    //        weirdThingAtTheBottomMenu.SetActive(false);
    //    }
    //    else
    //    {
    //        weirdThingAtTheBottomMenu.SetActive(true);
    //    }
    //}

    //public void AdvisorMenu()
    //{
    //    if (advisorMenu.activeInHierarchy)
    //    {
    //        advisorMenu.SetActive(false);
    //    }
    //    else
    //    {
    //        advisorMenu.SetActive(true);
    //    }
    //}

    //public void DeactivateAllBuildSubMenus()
    //{
    //    joystickMenu.SetActive(false);
    //    friesMenu.SetActive(false);
    //    stoolMenu.SetActive(false);
    //    weirdThingAtTheBottomMenu.SetActive(false);

    //}

    public void DeactivateAllMenus()
    {
        anim.SetBool("Open", false);

    }
    private void PlayAnimation()
    {
            anim.SetBool("Open", true);
            tabOpen = true;
    }
}