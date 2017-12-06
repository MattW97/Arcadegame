using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeManagementUI : MonoBehaviour {

    [SerializeField]
    private GameObject statsMain, staffMain, customerMain, arcadeItemsMain;

    [SerializeField] private GameObject statsUnselected, staffUnselected, customerUnselected, arcadeItemsUnselected, closedUnselected, statsSelected, staffSelected, customerSelected, arcadeItemsSelected, closeSelected;

    public void Start() {

        statsSelected.SetActive(false);
        staffSelected.SetActive(false);
        customerSelected.SetActive(false);
        arcadeItemsSelected.SetActive(false);

    }


    public void StatsMain()
    {
        ////////////////////// Panels ///////////////////////////////

        statsMain.SetActive(true);
        staffMain.SetActive(false);
        customerMain.SetActive(false);
        arcadeItemsMain.SetActive(false);

        //////////////////// Unselected /////////////////////////////

        statsUnselected.SetActive(false);
        staffUnselected.SetActive(true);
        customerUnselected.SetActive(true);
        arcadeItemsUnselected.SetActive(true);

        //////////////////// Selected /////////////////////////////

        statsSelected.SetActive(true);
        staffSelected.SetActive(false);
        customerSelected.SetActive(false);
        arcadeItemsSelected.SetActive(false);

    }

    public void StaffMain()
    {
        ////////////////////// Panels ///////////////////////////////

        statsMain.SetActive(false);
        staffMain.SetActive(true);
        customerMain.SetActive(false);
        arcadeItemsMain.SetActive(false);

        //////////////////// Unselected /////////////////////////////

        statsUnselected.SetActive(true);
        staffUnselected.SetActive(false);
        customerUnselected.SetActive(true);
        arcadeItemsUnselected.SetActive(true);

        //////////////////// Selected /////////////////////////////

        statsSelected.SetActive(false);
        staffSelected.SetActive(true);
        customerSelected.SetActive(false);
        arcadeItemsSelected.SetActive(false);


    }

    public void CustomerMain()
    {
        ////////////////////// Panels ///////////////////////////////

        statsMain.SetActive(false);
        staffMain.SetActive(false);
        customerMain.SetActive(true);
        arcadeItemsMain.SetActive(false);

        //////////////////// Unselected /////////////////////////////

        statsUnselected.SetActive(true);
        staffUnselected.SetActive(true);
        customerUnselected.SetActive(false);
        arcadeItemsUnselected.SetActive(true);

        //////////////////// Selected /////////////////////////////

        statsSelected.SetActive(false);
        staffSelected.SetActive(false);
        customerSelected.SetActive(true);
        arcadeItemsSelected.SetActive(false);

    }

    public void ArcadeItems()
    {
        ////////////////////// Panels ///////////////////////////////

        statsMain.SetActive(false);
        staffMain.SetActive(false);
        customerMain.SetActive(false);
        arcadeItemsMain.SetActive(true);

        //////////////////// Unselected /////////////////////////////

        statsUnselected.SetActive(true);
        staffUnselected.SetActive(true);
        customerUnselected.SetActive(true);
        arcadeItemsUnselected.SetActive(false);

        //////////////////// Selected /////////////////////////////

        statsSelected.SetActive(false);
        staffSelected.SetActive(false);
        customerSelected.SetActive(false);
        arcadeItemsSelected.SetActive(true);

    }
}
