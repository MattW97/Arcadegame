using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGhost : MonoBehaviour {

    private List<Tile> collidedTiles;
    private ObjectManager _objectManagerList;
    [SerializeField] private int numberOfTiles;

	// Use this for initialization
	void Start () {
        _objectManagerList = GameManager.Instance.SceneManagerLink.GetComponent<ObjectManager>();
        collidedTiles = new List<Tile>();
	}
	
	// Update is called once per frame
	void Update () {

        foreach (Tile tile in _objectManagerList.AllTiles)
        {
            if (!collidedTiles.Contains(tile))
            {
                tile.ToggleHighlighter(false);
            }
        }
     
        if ((Input.GetAxis("Mouse X") < 0) || (Input.GetAxis("Mouse X") > 0) || (Input.GetAxis("Mouse Y") > 0) || (Input.GetAxis("Mouse Y") < 0))
        {
            
        }
	}


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Tile")
        {
            collidedTiles.Add(collision.gameObject.GetComponent<Tile>());
            collision.gameObject.GetComponent<Tile>().ToggleHighlighter(true);
            Tile tile = collision.GetComponent<Tile>();
            if (tile.GetIsPlacedOn())
            {
                tile.ChangeHighlighterColour(1);
            }
            else
            {
                tile.ChangeHighlighterColour(0);
            }

        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Tile")
        {
            if (collidedTiles.Contains(collision.gameObject.GetComponent<Tile>()))
            {
                collidedTiles.Remove(collision.gameObject.GetComponent<Tile>());
                collision.gameObject.GetComponent<Tile>().ToggleHighlighter(false);
            }
            else
            {
                print("It got here!");
            }
        }
    }

    public void ClearAll()
    {
        foreach (Tile tile in collidedTiles)
        {
            tile.ToggleHighlighter(false);
        }
        collidedTiles.Clear();
    }

    public void OnPlaced(GameObject objectToPlace)
    {
        objectToPlace.GetComponent<PlaceableObject>().OccupiedTiles = new List<Tile>();
        foreach (Tile tile in collidedTiles)
        {
            tile.SetIfPlacedOn(true);
            tile.ToggleHighlighter(false);
            objectToPlace.GetComponent<PlaceableObject>().OccupiedTiles.Add(tile);
        }
        collidedTiles.Clear();
    }

    public List<Tile> GetCollidedTiles()
    {
        print(collidedTiles.Count);
        return collidedTiles;
    }
    public bool IsPlaceable()
    {
        foreach(Tile tile in collidedTiles)
        {
            if(tile.GetIsPlacedOn())
            {
                print("A tile you need is already occupied");
                return false;
            }
        }
        if(collidedTiles.Count == numberOfTiles)
        {
            return true;
        }
        print("You are not occupying the required amount of tiles!");
        return false;
    }

}
