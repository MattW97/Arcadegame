using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeManagementUI : MonoBehaviour {

    [SerializeField]
    private GameObject statsMain, staffMain, customerMain, arcadeItemsMain, playerImage;

    [SerializeField]
    private GameObject statsUnselected, staffUnselected, customerUnselected, arcadeItemsUnselected, closedUnselected, statsSelected, staffSelected, customerSelected, arcadeItemsSelected, closeSelected;

    public void Start() {

        playerImage.SetActive(false);
        statsMain.SetActive(false);
        staffMain.SetActive(false);
        customerMain.SetActive(false);
        arcadeItemsMain.SetActive(false);
        statsUnselected.SetActive(false);
        staffUnselected.SetActive(false);
        customerUnselected.SetActive(false);
        arcadeItemsUnselected.SetActive(false);
        closedUnselected.SetActive(false);
        statsSelected.SetActive(false);
        staffSelected.SetActive(false);
        customerSelected.SetActive(false);
        arcadeItemsSelected.SetActive(false);
        closeSelected.SetActive(false);

    }


    public void StatsMain()
    {
        ////////////////////// Panels ///////////////////////////////

        playerImage.SetActive(true);
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

        //////////////////// Close /////////////////////////////

        closedUnselected.SetActive(true);

    }

    public void StaffMain()
    {
        ////////////////////// Panels ///////////////////////////////

        playerImage.SetActive(true);
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

        //////////////////// Close /////////////////////////////

        closedUnselected.SetActive(true);

    }

    public void CustomerMain()
    {
        ////////////////////// Panels ///////////////////////////////

        playerImage.SetActive(true);
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

        //////////////////// Close /////////////////////////////

        closedUnselected.SetActive(true);

    }

    public void ArcadeItems()
    {
        ////////////////////// Panels ///////////////////////////////

        playerImage.SetActive(true);
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

        //////////////////// Close /////////////////////////////

        closedUnselected.SetActive(true);

    }
    public void CloseAll()
    {
        playerImage.SetActive(false);
        statsMain.SetActive(false);
        staffMain.SetActive(false);
        customerMain.SetActive(false);
        arcadeItemsMain.SetActive(false);
        statsUnselected.SetActive(false);
        staffUnselected.SetActive(false);
        customerUnselected.SetActive(false);
        arcadeItemsUnselected.SetActive(false);
        closedUnselected.SetActive(false);
        statsSelected.SetActive(false);
        staffSelected.SetActive(false);
        customerSelected.SetActive(false);
        arcadeItemsSelected.SetActive(false);
        closeSelected.SetActive(false);

    }
}
