using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private List<PlaceableObject> allPlaceableObjects;
    private List<string> placeableObjectNames;

    private GameObject tileParent;
    private Tile[] allTiles;

    void Awake()
    {
        tileParent = GameObject.Find("Level/Tiles");
        allTiles = tileParent.GetComponentsInChildren<Tile>();
        CreateNameList();
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

    public List<string> PlaceableObjectNames
    {
        get
        {
            return placeableObjectNames;
        }

        set
        {
            placeableObjectNames = value;
        }
    }

    private void CreateNameList()
    {
        placeableObjectNames = new List<string>();
        foreach (PlaceableObject obj in allPlaceableObjects)
        {
            placeableObjectNames.Add(obj.PrefabName);
        }
    }
}
