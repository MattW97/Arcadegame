using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionScriptController : MonoBehaviour {

    [SerializeField]
    private GameObject PlacedObjectInteractionMenu, PlacingObjectInteractionMenu;

    private TileInteraction _tileInteractionLink;

	// Use this for initialization
	void Start () {

        _tileInteractionLink = GameManager.Instance.SceneManagerLink.GetComponent<TileInteraction>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if (_tileInteractionLink.CurrentSelectedObject == null)
        {
            PlacedObjectInteractionMenu.SetActive(false);
        }
        else
        {
            PlacingObjectInteractionMenu.SetActive(false);
            PlacedObjectInteractionMenu.SetActive(true);
            PlacedObjectInteractionMenu.GetComponent<PlacedObjectInteractionMenuUI>().GetCurrentObject(_tileInteractionLink.CurrentSelectedObject);
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

}
