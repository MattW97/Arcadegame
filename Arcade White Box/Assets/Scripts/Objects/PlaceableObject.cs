using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : Entity {

    [SerializeField] protected float buyCost; // how much the object costs to buy.
    [SerializeField] protected GameObject selectionMesh; // the mesh that will flash when the player selects this object in the world.
    [SerializeField] protected int percentReturnedUponSold; //PERCENTAGE amount returned upon being sold back to the manufacturer. 

    protected bool selected;
    protected Tile placedOnTile;
    protected Transform selectionTransform;

    protected virtual void Awake()
    {
        selectionTransform = selectionMesh.GetComponent<Transform>();
        selectionMesh.SetActive(false);
        Selected = false;
    }

    protected virtual void Update()
    {
        if (Selected)
        {
            PulseScale();
        }
    }

    protected void OnMouseOver()
    {
        selectionMesh.SetActive(true);
    }

    protected void OnMouseExit()
    {
        if (!Selected)
        {
            selectionMesh.SetActive(false);
        }
    }

    protected void PulseScale()
    {
        selectionTransform.localScale = new Vector3(Mathf.PingPong(Time.time, 0.2f) + 1.0f, Mathf.PingPong(Time.time, 0.2f) + 1.0f, Mathf.PingPong(Time.time, 0.2f) + 1.0f);
        print("hello");
    }

    public bool Selected
    {
        get
        {
            return selected;
        }

        set
        {
            selected = value;

            if (!selected)
            {
                selectionTransform.localScale = new Vector3(1, 1, 1);
                selectionMesh.SetActive(false);
            }
        }
    }

    public Tile PlacedOnTile
    {
        get
        {
            return placedOnTile;
        }

        set
        {
            placedOnTile = value;
        }
    }

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

    public float returnAmount()
    {
        return (BuyCost / percentReturnedUponSold);
    }
}
