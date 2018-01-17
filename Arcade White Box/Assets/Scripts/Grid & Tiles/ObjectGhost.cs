using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGhost : MonoBehaviour {

    private List<Tile> collidedTiles;

	// Use this for initialization
	void Start () {

        collidedTiles = new List<Tile>();
	}
	
	// Update is called once per frame
	void Update () {
        if (collidedTiles.Count > 4)
        {
            collidedTiles.Clear();
        }
	}


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Tile")
        {
            collidedTiles.Add(collision.gameObject.GetComponent<Tile>());
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Tile" && collidedTiles.Contains(collision.gameObject.GetComponent<Tile>()))
        {
            collidedTiles.Remove(collision.gameObject.GetComponent<Tile>());
        }
    }

    public void OnPlaced()
    {
        foreach (Tile tile in collidedTiles)
        {
            tile.SetIfPlacedOn(true);
        }
        collidedTiles.Clear();
    }
    public bool IsPlaceable()
    {
        foreach(Tile tile in collidedTiles)
        {
            if(tile.GetIsPlacedOn())
            {
                return false;
            }
        }
        if(collidedTiles.Count == 4)
        {
            return true;
        }
        return false;
    }

}
