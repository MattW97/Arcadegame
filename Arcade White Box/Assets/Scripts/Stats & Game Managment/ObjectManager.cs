﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private List<PlaceableObject> allPlaceableObjects;

    private GameObject tileParent;
    private Tile[] allTiles;

    void Awake()
    {
        tileParent = GameObject.Find("Level/Tiles");
        allTiles = tileParent.GetComponentsInChildren<Tile>();
    }

    public List<PlaceableObject> AllPlaceableObjects
    {
        get
        {
            return allPlaceableObjects;
        }

        set
        {
            allPlaceableObjects = value;
        }
    }

    public Tile[] AllTiles
    {
        get
        {
            return allTiles;
        }

        set
        {
            allTiles = value;
        }
    }
}