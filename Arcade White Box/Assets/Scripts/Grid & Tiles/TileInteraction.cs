using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInteraction : MonoBehaviour
{
    [SerializeField] private GameObject tileHighlighter;
    [SerializeField] private Material canPlace, cantPlace;
    [SerializeField] private GameObject[] tempObjects;
    [SerializeField] private PlaceableObject tempPlaceObject;

    private bool rotatingObject;
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

    void Start()
    { 
        highlighterTransform = tileHighlighter.GetComponent<Transform>();

        currentSelectedObject = null;

        tileHighlighter.SetActive(false);

        _playerLink = this.gameObject.GetComponent<PlayerManager>();
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

                            GameObject newObject = Instantiate(TempPlaceObject.gameObject, hitInfo.collider.gameObject.transform.position, TempPlaceObject.transform.rotation);

                            if (CheckIfMachineOrPlaceable(TempPlaceObject))
                            {
                                _playerLink.OnMachinePurchase(TempPlaceObject as Machine);
                            }
                            else if (!CheckIfMachineOrPlaceable(TempPlaceObject))
                            {
                                _playerLink.OnBuildingPartPurchase(TempPlaceObject);
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

                    currentSelectedObject = hitInfo.collider.gameObject.GetComponent<PlaceableObject>();
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

                if (CheckIfMachineOrPlaceable(currentSelectedObject))
                {
                    _playerLink.CurrentCash += currentSelectedObject.returnAmount();
                }
                else if (!CheckIfMachineOrPlaceable(currentSelectedObject))
                {
                    _playerLink.CurrentCash += currentSelectedObject.returnAmount();
                    _playerLink.CurrentExpenses -= currentSelectedObject.GetComponent<Machine>().RunningCost;
                }


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
}
