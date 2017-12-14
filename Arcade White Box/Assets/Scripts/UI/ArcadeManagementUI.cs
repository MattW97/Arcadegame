using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcadeManagementUI : MonoBehaviour {

    [SerializeField]
    private GameObject statsMain, staffMain, customerMain, arcadeItemsMain, playerImage, staffTest, contentParent;

    [SerializeField]
    private GameObject statsUnselected, staffUnselected, customerUnselected, arcadeItemsUnselected, closedUnselected,
                       statsSelected, staffSelected, customerSelected, arcadeItemsSelected, closeSelected;

    [SerializeField]
    private GameObject workerDescription, technicianDescription, chefDescription, janitorDescription;

    [SerializeField]
    private Image happinessBar, fatigueBar, hungerBar, bladderBar, queasinessBar;

    [SerializeField]
    private Text totalSpeciesText;

    private CustomerManager customerManager;

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
        workerDescription.SetActive(false);
        technicianDescription.SetActive(false);
        chefDescription.SetActive(false);
        janitorDescription.SetActive(false);

        customerManager = GameManager.Instance.ScriptHolderLink.GetComponent<CustomerManager>();
    }

    #region Show On Button Selection Methods

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

    public void WorkerDescription()
    {
        workerDescription.SetActive(true);
        technicianDescription.SetActive(false);
        chefDescription.SetActive(false);
        janitorDescription.SetActive(false);
    }
    public void TechnicianDescription()
    {
        workerDescription.SetActive(false);
        technicianDescription.SetActive(true);
        chefDescription.SetActive(false);
        janitorDescription.SetActive(false);
    }
    public void ChefDescription()
    {
        workerDescription.SetActive(false);
        technicianDescription.SetActive(false);
        chefDescription.SetActive(true);
        janitorDescription.SetActive(false);
    }
    public void JanitorDescription()
    {
        workerDescription.SetActive(false);
        technicianDescription.SetActive(false);
        chefDescription.SetActive(false);
        janitorDescription.SetActive(true);
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

    #endregion

    #region  Stat Bar Update Methods

    public void Update()
    {
        if (customerMain.activeSelf == true)
        {
            totalSpeciesText.text = customerManager.GetNumberOfCustomers().ToString();
            happinessBar.fillAmount = customerManager.GetAverageCustomerHappiness() / 1000;
            hungerBar.fillAmount = customerManager.GetAverageCustomerHunger() / 100;
            fatigueBar.fillAmount = customerManager.GetAverageCustomerTiredness() / 100;
            bladderBar.fillAmount = customerManager.GetAverageCustomerBladder() / 100;
            queasinessBar.fillAmount = customerManager.GetAverageCustomerQueasiness() / 100;
        }
    }

    #endregion


    public void ListTest()
    {
        Janitor newjanitor = GameManager.Instance.ScriptHolderLink.GetComponent<StaffManager>().SpawnJanitor();

        GameObject newObject = Instantiate(staffTest, staffTest.transform.position, staffTest.transform.rotation);
        newObject.transform.SetParent(contentParent.transform, false);
        newObject.GetComponent<StaffDetails>().AssignStaffMember(newjanitor);
    }
}
