using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{

    //[SerializeField]
    //private GameObject gameTab, servicesTab, constructionTab, decorationTab; // optionsMenu;

    [SerializeField]
    private GameObject gameHalfTab, gameFullTab, servicesHalfTab, servicesFullTab, constructionHalfTab, constructionFullTab,
                       decorationHalfTab, decorationFullTab, placeholder, gameMain, serviceMain, constructionMain, decorationMain;

    [SerializeField]
    private GameObject gameScroll, serviceScroll, constructionScroll, decorationScroll;

    [SerializeField]
    private GameObject placingObject, placedObject;

    private bool tabOpen;

    private Animator anim;
    private Animator placingAnim;

    // Use this for initialization
    void Start()
    {

        anim = this.GetComponent<Animator>();

        placingAnim = placingObject.GetComponent<Animator>();

        placeholder.SetActive(true);
        gameFullTab.SetActive(false);
        servicesFullTab.SetActive(false);
        constructionFullTab.SetActive(false);
        decorationFullTab.SetActive(false);

        //////////////////// Main Tabs /////////////////////////////

        gameMain.SetActive(false);
        serviceMain.SetActive(false);
        constructionMain.SetActive(false);
        decorationMain.SetActive(false);
    }
    
    public void GameTab()
    {

        PlayAnimation();

        //////////////////// Main Tabs /////////////////////////////

        gameMain.SetActive(true);
        serviceMain.SetActive(false);
        constructionMain.SetActive(false);
        decorationMain.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        gameHalfTab.SetActive(false);
        servicesHalfTab.SetActive(true);
        constructionHalfTab.SetActive(true);
        decorationHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        gameFullTab.SetActive(true);
        servicesFullTab.SetActive(false);
        constructionFullTab.SetActive(false);
        decorationFullTab.SetActive(false);

    }

    public void ServicesTab()
    {
        PlayAnimation();

        //////////////////// Main Tabs /////////////////////////////

        gameMain.SetActive(false);
        serviceMain.SetActive(true);
        constructionMain.SetActive(false);
        decorationMain.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        gameHalfTab.SetActive(true);
        servicesHalfTab.SetActive(false);
        constructionHalfTab.SetActive(true);
        decorationHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        gameFullTab.SetActive(false);
        servicesFullTab.SetActive(true);
        constructionFullTab.SetActive(false);
        decorationFullTab.SetActive(false);
    }

    public void ConstructionTab()
    {
        PlayAnimation();

        //////////////////// Main Tabs /////////////////////////////

        gameMain.SetActive(false);
        serviceMain.SetActive(false);
        constructionMain.SetActive(true);
        decorationMain.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        gameHalfTab.SetActive(true);
        servicesHalfTab.SetActive(true);
        constructionHalfTab.SetActive(false);
        decorationHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        gameFullTab.SetActive(false);
        servicesFullTab.SetActive(false);
        constructionFullTab.SetActive(true);
        decorationFullTab.SetActive(false);
    }

    public void DecorationTab()
    {
        PlayAnimation();

        //////////////////// Main Tabs /////////////////////////////

        gameMain.SetActive(false);
        serviceMain.SetActive(false);
        constructionMain.SetActive(false);
        decorationMain.SetActive(true);

        //////////////////// Half Tabs /////////////////////////////

        gameHalfTab.SetActive(true);
        servicesHalfTab.SetActive(true);
        constructionHalfTab.SetActive(true);
        decorationHalfTab.SetActive(false);

        //////////////////// Full Tabs /////////////////////////////

        gameFullTab.SetActive(false);
        servicesFullTab.SetActive(false);
        constructionFullTab.SetActive(false);
        decorationFullTab.SetActive(true);
    }

    public void DeactivateAllMenus()
    {
        anim.SetBool("Open", false);

        //////////////////// Half Tabs /////////////////////////////

        gameHalfTab.SetActive(true);
        servicesHalfTab.SetActive(true);
        constructionHalfTab.SetActive(true);
        decorationHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        gameFullTab.SetActive(false);
        servicesFullTab.SetActive(false);
        constructionFullTab.SetActive(false);
        decorationFullTab.SetActive(false);

        placingAnim.SetBool("Placing", false);
        //placingObject.SetActive(false);

        placeholder.SetActive(true);

    }

    private void PlayAnimation()
    {
            anim.SetBool("Open", true);
            tabOpen = true;
    }
        
}