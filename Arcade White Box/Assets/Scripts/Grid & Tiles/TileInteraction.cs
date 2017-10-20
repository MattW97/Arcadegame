using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInteraction : MonoBehaviour
{
    [SerializeField] private GameObject tileHighlighterPrefab;
    [SerializeField] private Material canPlace, cantPlace;
    [SerializeField] private GameObject[] tempObjects;
    [SerializeField] private PlaceableObject tempPlaceObject;

    private bool rotatingObject;
    private GameObject tileHighlighter;
    private Transform highlighterTransform;
    private PlaceableObject currentSelectedObject;

    private PlayerManager _playerLink;

    public PlaceableObject TempPlaceObject
    {
        get
        {
            return tempPlaceObject;
        }

        set
        {
            tempPlaceObject = value;
        }
    }

    public PlaceableObject CurrentSelectedObject
    {
        get
        {
            return currentSelectedObject;
        }

        set
        {
            currentSelectedObject = value;
        }
    }

    void Start()
    {
        tileHighlighter = Instantiate(tileHighlighterPrefab, Vector3.zero, Quaternion.identity);

        highlighterTransform = tileHighlighter.GetComponent<Transform>();

        CurrentSelectedObject = null;

        tileHighlighter.SetActive(false);

        _playerLink = GameManager.Instance.SceneManagerLink.GetComponent<PlayerManager>();
    }

    void Update()
    {
        TileSelection();
        ObjectInteraction();
    }

    private void TileSelection()
    {
        RaycastHit hitInfo;
        // raycast
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && !OverUI())
        { 
            if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
            {
                // tileHighlighter activation
                if (tileHighlighter.activeSelf == false)
                {
                    tileHighlighter.SetActive(true);
                }
                // placing objects check
                if (hitInfo.collider.gameObject.GetComponent<Tile>().GetTileType() == Tile.TileType.Passable)
                {
                    tileHighlighter.GetComponent<Renderer>().material = canPlace;

                    if (Input.GetMouseButtonDown(0))
                    {
                        NullSelectedObject();

                        if (_playerLink.CheckCanAfford(TempPlaceObject.BuyCost))
                            {

                            GameObject newObject = Instantiate(TempPlaceObject.gameObject, hitInfo.collider.gameObject.transform.position, tileHighlighter.transform.rotation);
                            GameManager.Instance.SceneManagerLink.GetComponent<LevelManager>().AddObjectToLists(newObject); 

                            if (CheckIfMachineOrPlaceable(TempPlaceObject))
                            {
                                GameManager.Instance.SceneManagerLink.GetComponent<LevelManager>().OnMachinePurchase(TempPlaceObject as Machine);
                            }
                            else if (!CheckIfMachineOrPlaceable(TempPlaceObject))
                            {
                                GameManager.Instance.SceneManagerLink.GetComponent<LevelManager>().OnBuildingPartPurchase(TempPlaceObject);
                            }
                            else
                            {
                                print("you done fucked up son");
                            }

                            newObject.GetComponent<PlaceableObject>().PlacedOnTile = hitInfo.collider.gameObject.GetComponent<Tile>();
                            hitInfo.collider.gameObject.GetComponent<Tile>().SetTileType(Tile.TileType.Object);
                        }
                        else
                        {
                            print("cant afford");
                            //put something here to tell the player they are poor
                        }
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
            // selecting object
            else if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Object") == 0)
            {
                tileHighlighter.SetActive(false);

                if(Input.GetMouseButtonDown(0))
                {
                    NullSelectedObject();

                    CurrentSelectedObject = hitInfo.collider.gameObject.GetComponent<PlaceableObject>();
                    CurrentSelectedObject.Selected = true;
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
        if(CurrentSelectedObject)
        {
            // remove this in full version or modify for hotkey
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                DestroyCurrentlySelectedObject();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                CurrentSelectedObject.transform.eulerAngles += new Vector3(0.0f, 90.0f, 0.0f);
                
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                CurrentSelectedObject.transform.eulerAngles += new Vector3(0.0f, -90.0f, 0.0f);
            }
        }
        if (!CurrentSelectedObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                tileHighlighter.transform.eulerAngles += new Vector3(0.0f, 90.0f, 0.0f);

            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                tileHighlighter.transform.eulerAngles += new Vector3(0.0f, -90.0f, 0.0f);
            }
        }
    }

    private void NullSelectedObject()
    {
        if (CurrentSelectedObject)
        {
            CurrentSelectedObject.Selected = false;
            CurrentSelectedObject = null;
        }
    }


    // returns true if @param derives from the machine class
    // returns false if @param derives from Placeable Object class
    // @param the object you wish to test
    private bool CheckIfMachineOrPlaceable(Entity objectToTest)
    {
        if (objectToTest is Machine)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private bool OverUI()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void switchTileHighlighterMesh(PlaceableObject objectToDrawMeshFrom)
    {
        Mesh newMesh = objectToDrawMeshFrom.GetComponentInChildren<MeshFilter>().sharedMesh;
        tileHighlighter.GetComponent<MeshFilter>().sharedMesh = newMesh;
    }

    public void DestroyCurrentlySelectedObject()
    {
        CurrentSelectedObject.PlacedOnTile.SetTileType(Tile.TileType.Passable);

        GameManager.Instance.SceneManagerLink.GetComponent<LevelManager>().RemoveObjectFromLists(CurrentSelectedObject.gameObject);

        Destroy(CurrentSelectedObject.gameObject);

        if (CheckIfMachineOrPlaceable(CurrentSelectedObject))
        {
            _playerLink.CurrentCash += CurrentSelectedObject.returnAmount();
        }
        else if (!CheckIfMachineOrPlaceable(CurrentSelectedObject))
        {
            _playerLink.CurrentCash += CurrentSelectedObject.returnAmount();
           // GameManager.Instance.SceneManagerLink.GetComponent<LevelManager>().
        }
        CurrentSelectedObject = null;
    }
}
