﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : Entity {

    [SerializeField] protected float buyCost; // how much the object costs to buy.
    [SerializeField] protected GameObject selectionMesh; // the mesh that will flash when the player selects this object in the world.
    [SerializeField] protected int percentReturnedUponSold; //PERCENTAGE amount returned upon being sold back to the manufacturer. 
    [SerializeField] protected string description; // description of the machine Optional

     
    protected bool selected;
    protected Tile placedOnTile;
    protected Transform selectionTransform;

    protected Vector3 position;
    protected Vector3 rotation;

    private string prefabName;
    private int tile_ID;

    protected virtual void Awake()
    {
        selectionTransform = selectionMesh.GetComponent<Transform>();
        selectionMesh.SetActive(false);
        Selected = false;
        if (percentReturnedUponSold == 0)
        {
            percentReturnedUponSold = 1;
        }
    }

    void OnEnable()
    {
        EventManager.Save += OnSave;
    }

    void OnDisable()
    {
        EventManager.Save -= OnSave;
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
    }


    public float returnAmount()
    {
        print(BuyCost);
        print(percentReturnedUponSold);
        print(((buyCost - percentReturnedUponSold) / buyCost) * 100);
        return (((buyCost - percentReturnedUponSold) / buyCost) * 100);
    }

    public virtual PlaceableObjectSaveable GetPlaceableObjectSaveable()
    {
        Transform tempTran = this.gameObject.GetComponent<Transform>();
        PlaceableObjectSaveable save = new PlaceableObjectSaveable();
        save.prefabName = this.PrefabName;
        save.tile_ID = this.tile_ID;
        save.PosX = tempTran.position.x;
        save.PosY = tempTran.position.y;
        save.PosZ = tempTran.position.z;
        save.RotX = tempTran.rotation.eulerAngles.x;
        save.RotY = tempTran.rotation.eulerAngles.y;
        save.RotZ = tempTran.rotation.eulerAngles.z;

        return save;
    }

    private void OnSave()
    {
        GameManager.Instance.GetComponent<SaveAndLoadManager>().saveData.placeableSaveList.Add(GetPlaceableObjectSaveable());
    }

    #region Getters/Setters
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
            else
            {
                selectionMesh.SetActive(true);
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

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public string PrefabName
    {
        get
        {
            return prefabName;
        }

        set
        {
            prefabName = value;
        }
    }
    #endregion Getters/Setters
}

[System.Serializable]
public class PlaceableObjectSaveable
{
    public string prefabName;
    public int tile_ID;
    public float PosX, PosY, PosZ;
    public float RotX, RotY, RotZ;
}
