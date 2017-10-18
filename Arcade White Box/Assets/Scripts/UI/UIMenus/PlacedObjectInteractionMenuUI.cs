using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedObjectInteractionMenuUI : MonoBehaviour {

    [SerializeField]
    private Text objectName, objectType, objectDailyCost, objectMaintainCost, objectGameCost;

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
        objectDailyCost.text = currentObject.RunningCost.ToString();
        objectMaintainCost.text = currentObject.MaintenanceCost.ToString();
        objectGameCost.text = currentObject.UseCost.ToString();
        icon.sprite = currentObject.Icon;
        interactableMachineMenu.SetActive(true);
    }

    private void UpdateUI(PlaceableObject currentObject)
    {
        interactableMachineMenu.SetActive(false);
        objectName.text = currentObject.Name1;
        icon.sprite = currentObject.Icon;
    }

    public void SellButtonPressed()
    {
        GameManager.Instance.SceneManagerLink.GetComponent<TileInteraction>().DestroyCurrentlySelectedObject();
    }

    public void AddUseCostButtonPressed(Machine currentObject)
    {
        currentObject.UseCost++;
        UpdateUIMachine(currentObject);
    }
    public void DeductUseCostButtonPressed(Machine currentObject)
    {
        currentObject.UseCost--;
        UpdateUIMachine(currentObject);
    }


}
