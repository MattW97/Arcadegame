using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacingObjectInteractionMenuUI : MonoBehaviour {

    [SerializeField]
    private Text objectName, objectCost, objectDailyCost, objectMaintainCost, objectGameCost, objectDescription;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private GameObject interactableMachineMenu;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetCurrentObject(PlaceableObject currentObject)
    {

        if (currentObject is Machine)
        {
            UpdateUIMachine(currentObject as Machine);
        }
        else if (currentObject is PlaceableObject)
        {
            UpdateUI(currentObject);
        }
    }

    private void UpdateUIMachine(Machine currentObject)
    {
        objectName.text = currentObject.Name1;
        objectCost.text = currentObject.BuyCost.ToString();
        objectDailyCost.text = currentObject.RunningCost.ToString();
        objectMaintainCost.text = currentObject.MaintenanceCost.ToString();
        //objectGameCost.text = currentObject.UseCost.ToString();
        objectDescription.text = currentObject.Description;
        icon.sprite = currentObject.Icon;
        interactableMachineMenu.SetActive(true);
    }

    private void UpdateUI(PlaceableObject currentObject)
    {
        interactableMachineMenu.SetActive(false);
        objectName.text = currentObject.Name1;
        objectCost.text = currentObject.BuyCost.ToString();
        objectDescription.text = currentObject.Description;
        icon.sprite = currentObject.Icon;
    }

    public void DisableThisGameObject()
    {
        this.gameObject.SetActive(false);
    }
}
