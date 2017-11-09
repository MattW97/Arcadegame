using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeManagementUI : MonoBehaviour {

    [SerializeField]
    private GameObject statsMain, staffMain, customerMain, arcadeItemsMain, statsHalfTab, staffHalfTab, customerHalfTab, arcadeItemsHalfTab, statsFullTab, staffFullTab, customerFullTab, arcadeItemsFullTab;

    public void Start() {

        statsFullTab.SetActive(false);
        staffFullTab.SetActive(false);
        customerFullTab.SetActive(false);
        arcadeItemsFullTab.SetActive(false);

    }


    public void StatsMain()
    {
        ////////////////////// Panels ///////////////////////////////

        statsMain.SetActive(true);
        staffMain.SetActive(false);
        customerMain.SetActive(false);
        arcadeItemsMain.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        statsHalfTab.SetActive(false);
        staffHalfTab.SetActive(true);
        customerHalfTab.SetActive(true);
        arcadeItemsHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        statsFullTab.SetActive(true);
        staffFullTab.SetActive(false);
        customerFullTab.SetActive(false);
        arcadeItemsFullTab.SetActive(false);

    }

    public void StaffMain()
    {
        ////////////////////// Panels ///////////////////////////////

        statsMain.SetActive(false);
        staffMain.SetActive(true);
        customerMain.SetActive(false);
        arcadeItemsMain.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        statsHalfTab.SetActive(true);
        staffHalfTab.SetActive(false);
        customerHalfTab.SetActive(true);
        arcadeItemsHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        statsFullTab.SetActive(false);
        staffFullTab.SetActive(true);
        customerFullTab.SetActive(false);
        arcadeItemsFullTab.SetActive(false);


    }

    public void CustomerMain()
    {
        ////////////////////// Panels ///////////////////////////////

        statsMain.SetActive(false);
        staffMain.SetActive(false);
        customerMain.SetActive(true);
        arcadeItemsMain.SetActive(false);

        //////////////////// Half Tabs /////////////////////////////

        statsHalfTab.SetActive(true);
        staffHalfTab.SetActive(true);
        customerHalfTab.SetActive(false);
        arcadeItemsHalfTab.SetActive(true);

        //////////////////// Full Tabs /////////////////////////////

        statsFullTab.SetActive(false);
        staffFullTab.SetActive(false);
        customerFullTab.SetActive(true);
        arcadeItemsFullTab.SetActive(false);

    }

    public void ArcadeItems()
    {
        ////////////////////// Panels ///////////////////////////////

        statsMain.SetActive(false);
        staffMain.SetActive(false);
        customerMain.SetActive(false);
        arcadeItemsMain.SetActive(true);

        //////////////////// Half Tabs /////////////////////////////

        statsHalfTab.SetActive(true);
        staffHalfTab.SetActive(true);
        customerHalfTab.SetActive(true);
        arcadeItemsHalfTab.SetActive(false);

        //////////////////// Full Tabs /////////////////////////////

        statsFullTab.SetActive(false);
        staffFullTab.SetActive(false);
        customerFullTab.SetActive(false);
        arcadeItemsFullTab.SetActive(true);

    }
}
