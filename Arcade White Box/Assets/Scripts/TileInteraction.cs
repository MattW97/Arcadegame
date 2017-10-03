using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteraction : MonoBehaviour
{
    [SerializeField] private GameObject tileHighlighter;
    [SerializeField] private Material canPlace, cantPlace, testerMat;
    [SerializeField] private GameObject[] tempObjects;

    private Transform highlighterTransform;
    private PlayerManager _playerLink;


    void Start()
    { 
        highlighterTransform = tileHighlighter.GetComponent<Transform>();

        _playerLink = GameObject.Find("Game Manager").GetComponent<PlayerManager>();
        tileHighlighter.SetActive(false);
    }

    void Update()
    {
        TileSelection();
    }

    private void TileSelection()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        { 

            // tile
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
                       GameObject newObject = Instantiate(tempObjects[0], hitInfo.collider.gameObject.transform.position, tempObjects[0].transform.rotation);
                        newObject.GetComponent<BasicObject>().setTile(hitInfo.collider.gameObject.GetComponent<Tile>());
                        //set tile as occupied
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

            // object
            if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Object") == 0)
            {
                if (Input.GetMouseButton(1))
                {
                    hitInfo.collider.gameObject.GetComponent<BasicObject>().Cleanup();
                    Destroy(hitInfo.collider.gameObject);
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

    private void switchModel(GameObject currentlySelectedObject)
    {

        if (tileHighlighter.transform.childCount > 0)
        {
            for (int i = 0; i < tileHighlighter.transform.childCount; i++)
            {
                Destroy(tileHighlighter.transform.GetChild(i));
            }
        }

        GameObject childObject = Instantiate(currentlySelectedObject);
        childObject.transform.parent = tileHighlighter.transform;
        childObject.GetComponent<Renderer>().material = testerMat;
        
    }
}
