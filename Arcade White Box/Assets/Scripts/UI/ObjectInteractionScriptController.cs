using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionScriptController : MonoBehaviour {

    [SerializeField]
    private GameObject PlacedObjectInteractionMenu, PlacingObjectInteractionMenu, objectStatUIMenu, customerInfoMenu;

    private LevelInteraction _tileInteractionLink;

    private Animator placingAnim;
    private Animator placedAnim;

    // Use this for initialization
    void Start () {

        placedAnim = PlacedObjectInteractionMenu.GetComponent<Animator>();

        placingAnim = PlacingObjectInteractionMenu.GetComponent<Animator>();

        _tileInteractionLink = GameManager.Instance.SceneManagerLink.GetComponent<LevelInteraction>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if (_tileInteractionLink.CurrentSelectedObject == null)
        {
            placedAnim.SetBool("Placed", false);
            objectStatUIMenu.SetActive(false);
        }
        else
        {
            placingAnim.SetBool("Placing", false);
            placedAnim.SetBool("Placed", true);
            PlacedObjectInteractionMenu.GetComponent<PlacedObjectInteractionMenuUI>().UpdateUIBase(_tileInteractionLink.CurrentSelectedObject);
            if (_tileInteractionLink.CurrentSelectedObject.GetComponent<Machine>())
            {
                objectStatUIMenu.GetComponent<ObjectStatUIController>().UpdateUI(_tileInteractionLink.CurrentSelectedObject as Machine);
                CheckMachineNeedsRepair(_tileInteractionLink.CurrentSelectedObject as Machine);
            }

        }

        if (_tileInteractionLink.CurrentSelectedAI == null)
        {
            customerInfoMenu.SetActive(false);
            print("nothing");
        }
        else
        {
            if (_tileInteractionLink.CurrentSelectedAI.GetComponent<Customer>())
            {
                customerInfoMenu.SetActive(true);
                customerInfoMenu.GetComponent<CustomerUIInfoBox>().UpdateUI(_tileInteractionLink.CurrentSelectedAI as Customer);
            }
        }
		
	}

    public void OnAddButtonPressed()
    {
        PlacedObjectInteractionMenu.GetComponent<PlacedObjectInteractionMenuUI>().AddUseCostButtonPressed(_tileInteractionLink.CurrentSelectedObject as Machine);
    }

    public void OnDeductButtonPressed()
    {
        PlacedObjectInteractionMenu.GetComponent<PlacedObjectInteractionMenuUI>().DeductUseCostButtonPressed(_tileInteractionLink.CurrentSelectedObject as Machine);
    }

    public void OnRepairButtonPressed()
    {
        PlacedObjectInteractionMenu.GetComponent<PlacedObjectInteractionMenuUI>().RepairButtonPressed(_tileInteractionLink.CurrentSelectedObject as Machine);
    }

    private void CheckMachineNeedsRepair(Machine currentObject)
    {
        if (currentObject.machineStatus == Machine.MachineStatus.Broken)
        {
            PlacedObjectInteractionMenu.GetComponent<PlacedObjectInteractionMenuUI>().RepairButtonActive(true);
        }
        else
        {
            PlacedObjectInteractionMenu.GetComponent<PlacedObjectInteractionMenuUI>().RepairButtonActive(false);
        }
    }

}
