﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour {

    private LevelInteraction _tileInteractionLink;

    [SerializeField] private List<PlaceableObject> placeableObjectsList;
    [SerializeField] private List<Button> buttonList;
    [SerializeField] private GameObject _objectUILink;

	void Start ()
    {
        _tileInteractionLink = GameManager.Instance.SceneManagerLink.GetComponent<LevelInteraction>();
        setupButtons();
	}

    private void setupButtons()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            Button currentButton = buttonList[i];
            currentButton.GetComponent<Image>().sprite = placeableObjectsList[i].Icon;
            currentButton.GetComponent<ButtonStorage>().ObjectAssignedToThisButton = placeableObjectsList[i];
        }
    }

    public void buttonClicked(Button theButton)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i] == theButton)
            {
                _tileInteractionLink.ObjectToPlace = theButton.GetComponent<ButtonStorage>().ObjectAssignedToThisButton;
                _tileInteractionLink.ReassignObjectGhost(theButton.GetComponent<ButtonStorage>().ObjectAssignedToThisButton);
                //_tileInteractionLink.SwitchTileHighlighterMesh(theButton.GetComponent<ButtonStorage>().ObjectAssignedToThisButton);
                _objectUILink.SetActive(true);
                _objectUILink.GetComponent<PlacingObjectInteractionMenuUI>().GetCurrentObject(theButton.GetComponent<ButtonStorage>().ObjectAssignedToThisButton);

                _tileInteractionLink.NullSelectedObject();
            }
        }
        
        //this.gameObject.SetActive(false);
    }
}
