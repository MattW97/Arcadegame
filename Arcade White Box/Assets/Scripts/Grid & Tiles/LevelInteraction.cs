using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelInteraction : MonoBehaviour
{
    [SerializeField] private GameObject tileHighlighterPrefab;
    [SerializeField] private Material canPlace, cantPlace;

    public enum InteractionState {SelectionMode, PlacingMode, IdleMode}

    private bool rotatingObject;
    private GameObject tileHighlighter;
    private Transform highlighterTransform, objectParent;
    private Renderer highlighterRenderer;
    private PlaceableObject currentSelectedObject;
    private InteractionState currentState;
    private Ray interactionRay;
    private RaycastHit hitInfo;
    private PlaceableObject objectToPlace;
    private EconomyManager economyManager;
    private LevelManager levelManager;
    private PathingGridSetup pathingGridSetup;

    void Awake()
    {
        tileHighlighter = Instantiate(tileHighlighterPrefab, Vector3.zero, Quaternion.identity);
        highlighterTransform = tileHighlighter.GetComponent<Transform>();
        highlighterRenderer = tileHighlighter.GetComponent<Renderer>();

        CurrentSelectedObject = null;
        ObjectToPlace = null;

        tileHighlighter.SetActive(false);
        CurrentState = InteractionState.SelectionMode;
    }

    void Start()
    {
        economyManager = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        levelManager = GameManager.Instance.SceneManagerLink.GetComponent<LevelManager>();
        pathingGridSetup = GameManager.Instance.PathingGridManagerLink.GetComponent<PathingGridSetup>();

        objectParent = GameObject.Find("Level/Placed Objects").transform;
    }

    void Update()
    {
        if(CurrentState == InteractionState.SelectionMode)
        {
            SelectionMode();
        }
        else if(CurrentState == InteractionState.PlacingMode)
        {
            PlacingMode();
        }

        if(ObjectToPlace)
        {
            CurrentState = InteractionState.PlacingMode;
        }
        else
        {
            CurrentState = InteractionState.SelectionMode;
        }
    }

    private void SelectionMode()
    {
        if(!OverUI())
        {
            interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(interactionRay, out hitInfo))
            {
                if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
                {
                    if(!tileHighlighter.activeSelf)
                    {
                        tileHighlighter.SetActive(true);
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

        ObjectInteraction();
    }

    private void PlacingMode()
    {
        if (!OverUI())
        {
            interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(interactionRay, out hitInfo))
            {
                if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
                {
                    Tile placedOnTile = hitInfo.collider.gameObject.GetComponent<Tile>();

                    if(!tileHighlighter.activeSelf)
                    {
                        tileHighlighter.SetActive(true);
                    }

                    Vector3 highlighterPosition = hitInfo.collider.transform.position;
                    highlighterPosition.y = highlighterPosition.y + 0.5f;
                    highlighterTransform.position = highlighterPosition;

                    if (placedOnTile.GetTileType() == Tile.TileType.Passable)
                    {
                        highlighterRenderer.material = canPlace;

                        if(Input.GetMouseButtonDown(0))
                        {
                            NullSelectedObject();

                            if(economyManager.CheckCanAfford(ObjectToPlace.BuyCost))
                            {
                                InstantiateNewObject(ObjectToPlace, hitInfo.collider.gameObject.transform.position, highlighterTransform.rotation, placedOnTile);
                                ObjectToPlace = null;
                            }
                        }
                    }
                    else if(placedOnTile.GetTileType() == Tile.TileType.Impassable)
                    {
                        highlighterRenderer.material = cantPlace;
                    }
                }
            }
        }
    }

    private void InstantiateNewObject(PlaceableObject objectToPlace, Vector3 position, Quaternion rotation, Tile objectTile)
    {
        GameObject newObject = Instantiate(objectToPlace.gameObject, position, rotation, objectParent);
        levelManager.AddObjectToLists(newObject);
        pathingGridSetup.UpdateGrid();

        if (CheckIfMachineOrPlaceable(ObjectToPlace))
        {
            economyManager.OnMachinePurchase(ObjectToPlace as Machine);
        }
        else if (!CheckIfMachineOrPlaceable(ObjectToPlace))
        {
            economyManager.OnBuildingPartPurchase(ObjectToPlace);
        }

        PlaceableObject newPlaceableObject = newObject.GetComponent<PlaceableObject>();

        newPlaceableObject.PlacedOnTile = objectTile;
        newPlaceableObject.PrefabName = objectToPlace.name;
        objectTile.SetIfPlacedOn(true);
    }

    private void ObjectInteraction()
    {
        if (CurrentSelectedObject)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DestroyCurrentlySelectedObject();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                CurrentSelectedObject.transform.eulerAngles += new Vector3(0.0f, 90.0f, 0.0f);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                CurrentSelectedObject.transform.eulerAngles += new Vector3(0.0f, -90.0f, 0.0f);
            }
        }
        else
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            tileHighlighter.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyCurrentlySelectedObject()
    {
        CurrentSelectedObject.PlacedOnTile.SetTileType(Tile.TileType.Passable);

        GameManager.Instance.SceneManagerLink.GetComponent<LevelManager>().RemoveObjectFromLists(CurrentSelectedObject.gameObject);

        Destroy(CurrentSelectedObject.gameObject);

        if (CheckIfMachineOrPlaceable(CurrentSelectedObject))
        {
            GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().CurrentCash += CurrentSelectedObject.returnAmount();
        }
        else if (!CheckIfMachineOrPlaceable(CurrentSelectedObject))
        {
            GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().CurrentCash += CurrentSelectedObject.returnAmount();
        }

        CurrentSelectedObject = null;
    }

    public void ClearObjectParent()
    {
        foreach (Transform child in objectParent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SwitchTileHighlighterMesh(PlaceableObject objectToDrawMeshFrom)
    {
        Mesh newMesh = objectToDrawMeshFrom.GetComponentInChildren<MeshFilter>().sharedMesh;
        tileHighlighter.GetComponent<MeshFilter>().sharedMesh = newMesh;
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

    public PlaceableObject ObjectToPlace
    {
        get
        {
            return objectToPlace;
        }

        set
        {
            objectToPlace = value;
        }
    }

    public InteractionState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }
}
