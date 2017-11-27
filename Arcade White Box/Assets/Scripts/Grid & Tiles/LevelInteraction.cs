using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelInteraction : MonoBehaviour
{
    [SerializeField] private GameObject tileHighlighterPrefab, wallHighlighter;
    [SerializeField] private Material canPlace, cantPlace;
    [SerializeField] private int noOfTileHighlighters, wallLengthLimit;
    [SerializeField] private LayerMask wallLayerMask;

    public enum InteractionState {SelectionMode, PlacingMode, WallPlacingMode, IdleMode}

    private bool rotatingObject;
    private GameObject tileHighlighter;
    private Transform highlighterTransform, objectParent;
    private List<GameObject> tileHighlighterList;
    private Renderer highlighterRenderer;
    private PlaceableObject currentSelectedObject;
    private InteractionState currentState;
    private Ray interactionRay;
    private Tile placedOnTile;
    private Tile startWallTile;
    private Tile endWallTile;
    private RaycastHit hitInfo;
    private PlaceableObject objectToPlace;
    private EconomyManager economyManager;
    private LevelManager levelManager;
    private PathingGridSetup pathingGridSetup;
    private List<GameObject> wallHighlighters;
    private GameObject wallHighligherParent;

    void Awake()
    {
        tileHighlighter = Instantiate(tileHighlighterPrefab, Vector3.zero, Quaternion.identity);
        highlighterTransform = tileHighlighter.GetComponent<Transform>();
        highlighterRenderer = tileHighlighter.GetComponent<Renderer>();

        wallHighlighters = new List<GameObject>();
        wallHighligherParent = new GameObject();

        for(int i = 0; i < wallLengthLimit; i++)
        {
            GameObject newHighlighter = Instantiate(wallHighlighter);
            newHighlighter.SetActive(false);
            newHighlighter.transform.parent = wallHighligherParent.transform;
            wallHighlighters.Add(newHighlighter);
        }

        CurrentSelectedObject = null;
        ObjectToPlace = null;

        tileHighlighter.SetActive(false);
        CurrentState = InteractionState.WallPlacingMode; 
    }

    void Start()
    {
        economyManager = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        levelManager = GameManager.Instance.ScriptHolderLink.GetComponent<LevelManager>();
        pathingGridSetup = GameManager.Instance.PathingGridManagerLink.GetComponent<PathingGridSetup>();
        Initialise();

        objectParent = GameObject.Find("Level/Placed Objects").transform;

        startWallTile = null;
        endWallTile = null;
    }

    void Update()
    {
        if (CurrentState == InteractionState.SelectionMode)
        {
            SelectionMode();
        }
        else if (CurrentState == InteractionState.PlacingMode)
        {
            PlacingMode();
        }
        else if (CurrentState == InteractionState.WallPlacingMode)
        {
            WallPlacing();
        }

        if(ObjectToPlace)
        {
            CurrentState = InteractionState.PlacingMode;
        }
        else
        {
            //CurrentState = InteractionState.SelectionMode;
        }
    }

    private void Initialise()
    {
        tileHighlighterList = new List<GameObject>();
        for (int i = 0; i < noOfTileHighlighters; i++)
        {
            GameObject newTileHighlighter = Instantiate(tileHighlighterPrefab);
            tileHighlighterList.Add(newTileHighlighter);
            newTileHighlighter.SetActive(false);
        }
    }

    private void SelectionMode()
    {
        if (tileHighlighter.activeSelf == true)
        {
            tileHighlighter.SetActive(false);
        }
        if (!OverUI())
        {
            interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(interactionRay, out hitInfo))
            {
                if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Object") == 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        NullSelectedObject();
                        CurrentSelectedObject = hitInfo.collider.gameObject.GetComponentInParent<PlaceableObject>();
                        CurrentSelectedObject.Selected = true;
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) && currentSelectedObject)
                    {
                        print("2");
                        currentSelectedObject.Selected = false;
                        NullSelectedObject();
                    }
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
                    placedOnTile = hitInfo.collider.gameObject.GetComponent<Tile>();
                    if (!tileHighlighter.activeSelf)
                        tileHighlighter.SetActive(true);

                    Vector3 highlighterPosition = hitInfo.collider.transform.position;
                    //highlighterPosition.y = highlighterPosition.y + 0.5f;
                    highlighterTransform.position = highlighterPosition;
                    if (placedOnTile.GetTileType() == Tile.TileType.Passable)
                    {
                        highlighterRenderer.material = canPlace;
                        if (Input.GetMouseButtonDown(0))
                        {
                            NullSelectedObject();

                            //CurrentState = InteractionState.WallPlacing;

                            if (economyManager.CheckCanAfford(ObjectToPlace.BuyCost))
                            {
                                InstantiateNewObject(ObjectToPlace, hitInfo.collider.gameObject.transform.position, highlighterTransform.rotation, placedOnTile);
                                ObjectToPlace = null;
                            }
                        }
                    }
                    else if (placedOnTile.GetTileType() == Tile.TileType.Impassable)
                    {
                        highlighterRenderer.material = cantPlace;
                    }
                }
                else if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Object") == 0)
                {
                    if(hitInfo.collider.gameObject.name != "Top")
                    placedOnTile = GetNearestTile(hitInfo.collider);
                    if (!tileHighlighter.activeSelf)
                        tileHighlighter.SetActive(true);
                    Vector3 highlighterPosition = placedOnTile.transform.position;
                    highlighterTransform.position = highlighterPosition;
                    if (placedOnTile.GetTileType() == Tile.TileType.Passable)
                    {
                        highlighterRenderer.material = canPlace;

                        if (Input.GetMouseButtonDown(0))
                        {
                            NullSelectedObject();

                            //CurrentState = InteractionState.WallPlacing;

                            if (economyManager.CheckCanAfford(ObjectToPlace.BuyCost))
                            {
                                InstantiateNewObject(ObjectToPlace, highlighterPosition, highlighterTransform.rotation, placedOnTile);
                                ObjectToPlace = null;
                            }
                        }
                    }
                    else if (placedOnTile.GetTileType() == Tile.TileType.Impassable)
                    {
                        highlighterRenderer.material = cantPlace;
                    }
                }
            }
        }
    }

    private void WallPlacing()
    {
        if (!OverUI())
        {
            interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(interactionRay, out hitInfo))
            {
                if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
                {
                    if (Input.GetMouseButtonDown(0) && !startWallTile && !endWallTile)
                    {
                        startWallTile = hitInfo.collider.GetComponent<Tile>();

                        foreach(GameObject highligher in wallHighlighters)
                        {
                            highligher.SetActive(false);
                        }
                    }
                    else if (Input.GetMouseButton(0) && startWallTile)
                    {   
                        endWallTile = hitInfo.collider.GetComponent<Tile>();
                        RaycastHit[] hits;

                        Vector3 heading = endWallTile.transform.position - startWallTile.transform.position;
                        float distance = heading.magnitude;
                        Vector3 direction = heading / distance;

                        Ray checkRay = new Ray(startWallTile.transform.position, direction);

                        Debug.DrawLine(startWallTile.transform.position, endWallTile.transform.position, Color.red, 1.0f, false);

                        hits = Physics.RaycastAll(checkRay, distance, wallLayerMask);

                        float angle = Vector3.Angle(heading, startWallTile.transform.forward);

                        if(angle == 90.0f || angle == 180.0f || angle == 0.0f)
                        {
                            for (int i = 0; i < hits.Length; i++)
                            {
                                wallHighlighters[i].transform.position = hits[i].transform.position;
                                wallHighlighters[i].SetActive(true);
                            }

                            for( int i = hits.Length; i < wallHighlighters.Count; i++)
                            {
                                wallHighlighters[i].SetActive(false);
                            }
                        }
                    }

                    if(Input.GetMouseButtonUp(0) && startWallTile && endWallTile)
                    {
                        endWallTile = null;
                        startWallTile = null;

                        foreach (GameObject highligher in wallHighlighters)
                        {
                            highligher.SetActive(false);
                        }
                    }
                }
            }
        }
    }
    private Tile GetTileMouseIsOn()
    {
        Tile tile = null;
        interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(interactionRay, out hitInfo))
        {
            if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
            {
                 tile = hitInfo.collider.gameObject.GetComponent<Tile>();
            }
        }
        return tile;
    }

    private Tile GetNearestTile(Collider col)
    {
        Tile tile = null;
        Collider[] colliders = Physics.OverlapSphere(col.transform.position, 1000.0f);
        Collider closestCol = null;
        Tile ownTile = col.gameObject.GetComponentInParent<PlaceableObject>().PlacedOnTile;
        foreach (Collider hit in colliders)
        {
            if (hit == col.GetComponent<Collider>() || hit.gameObject.tag != "Tile")
                continue;
            if (closestCol == null)
                closestCol = hit;
            if (hit.gameObject.GetComponent<Tile>().GetID() != ownTile.GetID())
            {
                if (Vector3.Distance(col.transform.position, hit.transform.position) <= Vector3.Distance(col.transform.position, closestCol.transform.position))
                    closestCol = hit;
            }
            tile = closestCol.gameObject.GetComponent<Tile>();
        }
        return tile;
    }
    

    private void InstantiateNewObject(PlaceableObject objectToPlace, Vector3 position, Quaternion rotation, Tile objectTile)
    {
        GameObject newObject = Instantiate(objectToPlace.gameObject, position, rotation, objectParent);
        levelManager.AddObjectToLists(newObject);
        pathingGridSetup.UpdateGrid();
        if (CheckIfMachineOrPlaceable(ObjectToPlace))
            economyManager.OnMachinePurchase(ObjectToPlace as Machine);
        else if (!CheckIfMachineOrPlaceable(ObjectToPlace))
            economyManager.OnBuildingPartPurchase(ObjectToPlace);
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
            return true;
        else
            return false;
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
