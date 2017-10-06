using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour {

    private TileInteraction _tileInteractionLink;

    [SerializeField] private List<PlaceableObject> placeableObjectsList;
    [SerializeField] private List<Button> buttonList;

	// Use this for initialization
	void Start () {
        _tileInteractionLink = GameObject.Find("Game Manager").GetComponent<TileInteraction>();
        setupButtons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void setupButtons()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            Button currentButton = buttonList[i];
            //currentButton.image = placeableObjectsList[i].Icon;
            currentButton.GetComponent<ButtonStorage>().ObjectAssignedToThisButton = placeableObjectsList[i];
        }
    }

    public void buttonClicked(Button theButton)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i] == theButton)
            {
                _tileInteractionLink.TempPlaceObject = theButton.GetComponent<ButtonStorage>().ObjectAssignedToThisButton;
                print(_tileInteractionLink.TempPlaceObject.name);
            }
        }
        this.gameObject.SetActive(false);
    }
}
