using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverCurrentObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private GameObject gameObjectToShowOrHide;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObjectToShowOrHide.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObjectToShowOrHide.SetActive(false);
    }

}
