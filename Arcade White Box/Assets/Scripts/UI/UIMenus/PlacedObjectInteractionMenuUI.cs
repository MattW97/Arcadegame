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
    private GameObject interactableMachineMenu, minusButton, plusButton, statsObject;

    [SerializeField]
    private Button showStatsButton, repairButton; 


    private Animator placedAnimation;

    // Use this for initialization
    void Start () {

        placedAnimation = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        
		
	}

    public void UpdateUIBase(PlaceableObject currentObject)
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

        placedAnimation.SetBool("Placed", true);

        interactableMachineMenu.SetActive(true);
    }

    private void UpdateUI(PlaceableObject currentObject)
    {
        interactableMachineMenu.SetActive(false);
        objectName.text = currentObject.Name1;
        icon.sprite = currentObject.Icon;
    }

    public void RepairButtonActive(bool needsRepair)
    {
        if (needsRepair)
        {
            repairButton.interactable = true;
            repairButton.GetComponentInChildren<Text>().color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        }
        else
        {
            repairButton.interactable = false;
            repairButton.GetComponentInChildren<Text>().color = new Color32(0x7E, 0x78, 0x78, 0xFF);
        }
    }

    public void RepairButtonPressed(Machine currentObject)
    {
        currentObject.OnRepair();
    }

    public void StatsButtonPressed()
    {
        if (!statsObject.activeInHierarchy)
        {
            statsObject.SetActive(true);
            showStatsButton.GetComponentInChildren<Text>().text = "Hide Stats";
        }
        else
        {
            statsObject.SetActive(false);
            showStatsButton.GetComponentInChildren<Text>().text = "Show Stats";
        }
    }

    public void SellButtonPressed()
    {
        GameManager.Instance.SceneManagerLink.GetComponent<LevelInteraction>().DestroyCurrentlySelectedObject();
    }

    public void AddUseCostButtonPressed(Machine currentObject)
    {
        currentObject.UseCost++;
        UpdateUIMachine(currentObject);
        ButtonCheck(currentObject);
    }
    public void DeductUseCostButtonPressed(Machine currentObject)
    {
        currentObject.UseCost--;
        UpdateUIMachine(currentObject);
        ButtonCheck(currentObject);
    }

    private void ButtonCheck(Machine currentObject)
    {
        if (currentObject.UseCost == currentObject.MinUseCost)
        {
            minusButton.SetActive(false);
        }

        else if (currentObject.UseCost == currentObject.MaxUseCost)
        {
            plusButton.SetActive(false);
        }
        else
        {
            plusButton.SetActive(true);
            minusButton.SetActive(true);
        }
    }


}
