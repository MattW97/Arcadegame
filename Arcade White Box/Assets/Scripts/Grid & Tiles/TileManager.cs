using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject tileParent;

    private Tile[] allTiles;

    void Start()
    {
        InitialiseTiles();
    }

    private void InitialiseTiles()
    {
        allTiles = tileParent.GetComponentsInChildren<Tile>();
        int counter = 0;   

        foreach(Tile newTile in allTiles)
        {
            newTile.SetID(counter);
            counter++;
        }
    }
}
