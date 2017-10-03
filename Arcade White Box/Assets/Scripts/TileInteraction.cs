using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteraction : MonoBehaviour
{
    [SerializeField] private GameObject tileHighlighter;
    [SerializeField] private Material canPlace, cantPlace;
    [SerializeField] private GameObject[] tempObjects;

    private bool rotatingObject;
    private Transform highlighterTransform;
    private BasicObject currentSelectedObject;

    void Start()
    { 
        highlighterTransform = tileHighlighter.GetComponent<Transform>();

        currentSelectedObject = null;

        tileHighlighter.SetActive(false);
    }

    void Update()
    {
        TileSelection();
        ObjectInteraction();
    }

    private void TileSelection()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        { 
            if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
            {
                if (tileHighlighter.activeSelf == false)
                {
                    tileHighlighter.SetActive(true);
                }

                if (hitInfo.collider.gameObject.GetComponent<Tile>().GetTileType() == Tile.TileType.Passable)
                {
                    tileHighlighter.GetComponent<Renderer>().material = canPlace;

                    if (Input.GetMouseButtonDown(0))
                    {
                        NullSelectedObject();

                        GameObject newObject = Instantiate(tempObjects[0], hitInfo.collider.gameObject.transform.position, tempObjects[0].transform.rotation);
                        newObject.GetComponent<BasicObject>().PlacedOnTile = hitInfo.collider.gameObject.GetComponent<Tile>();
                        hitInfo.collider.gameObject.GetComponent<Tile>().SetTileType(Tile.TileType.Object);
                    }
                }
                else if (hitInfo.collider.gameObject.GetComponent<Tile>().GetTileType() == Tile.TileType.Impassable)
                {
                    tileHighlighter.GetComponent<Renderer>().material = cantPlace;
                }

                Vector3 highlighterPosition = hitInfo.collider.transform.position;
                highlighterPosition.y = highlighterPosition.y + 0.5f;
                highlighterTransform.position = highlighterPosition;
            }
            else if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Object") == 0)
            {
                tileHighlighter.SetActive(false);

                if(Input.GetMouseButtonDown(0))
                {
                    NullSelectedObject();

                    currentSelectedObject = hitInfo.collider.gameObject.GetComponent<BasicObject>();
                    currentSelectedObject.Selected = true;
                }
            }
        }
        else
        {
            if (tileHighlighter.activeSelf == true)
            {
                tileHighlighter.SetActive(false);
            }
        }
    }

    private void ObjectInteraction()
    {
        if(currentSelectedObject)
        {
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                currentSelectedObject.PlacedOnTile.SetTileType(Tile.TileType.Passable);
                Destroy(currentSelectedObject.gameObject);
                currentSelectedObject = null;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                currentSelectedObject.transform.eulerAngles += new Vector3(0.0f, 0.0f, 90.0f);
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                currentSelectedObject.transform.eulerAngles += new Vector3(0.0f, 0.0f, -90.0f);
            }
        }
    }

    private void NullSelectedObject()
    {
        if (currentSelectedObject)
        {
            currentSelectedObject.Selected = false;
            currentSelectedObject = null;
        }
    }
}
