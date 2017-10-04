using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : Entity {

    [SerializeField] private float buyCost; // how much the object costs to buy.
    [SerializeField] private GameObject selectionMesh; // the mesh that will flash when the player selects this object in the world.

    public float BuyCost
    {
        get
        {
            return buyCost;
        }

        set
        {
            buyCost = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
