using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionScriptController : MonoBehaviour {

    [SerializeField]
    private GameObject PlacedObjectInteractionMenu, PlacingObjectInteractionMenu;

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
        }
        else
        {
            placingAnim.SetBool("Placing", false);
            placedAnim.SetBool("Placed", true);
            PlacedObjectInteractionMenu.GetComponent<PlacedObjectInteractionMenuUI>().UpdateUIBase(_tileInteractionLink.CurrentSelectedObject);
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
