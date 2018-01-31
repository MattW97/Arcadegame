using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelInteraction : MonoBehaviour
{
    [SerializeField] private GameObject wallHighlighter, roomScaler;
    [SerializeField] private Material canPlace, cantPlace;
    [SerializeField] private int noOfTileHighlighters, wallLengthLimit;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private GameObject objectGhostPrefab;

    public enum InteractionState {SelectionMode, PlacingMode, WallPlacingMode, RoomPlacing, IdleMode}

    private Transform objectParent;
    private List<GameObject> tileHighlighterList;
    private Renderer highlighterRenderer;
    private PlaceableObject currentSelectedObject;
    private BaseAI currentSelectedAI;
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
    private PlacingObjectInteractionMenuUI placingObject;
    private GameObject objectGhost;
    private GameObject startTile, endTile;

    void Awake()
    {
        objectGhost = Instantiate(objectGhostPrefab, Vector3.zero, Quaternion.identity);
        roomScaler = Instantiate(roomScaler, Vector3.zero, roomScaler.transform.rotation);

        wallHighlighters = new List<GameObject>();
        wallHighligherParent = new GameObject();

        for(int i = 0; i < wallLengthLimit; i++)
        {
            GameObject newHighlighter = Instantiate(wallHighlighter);
            newHighlighter.SetActive(false);
            newHighlighter.transform.parent = wallHighligherParent.transform;
            wallHighlighters.Add(newHighlighter);
        }

        PlacedSelectedObject = null;
        CurrentSelectedAI = null;
        ObjectToPlace = null;

        CurrentState = InteractionState.SelectionMode; 
    }

    void Start()
    {
        economyManager = GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>();
        levelManager = GameManager.Instance.ScriptHolderLink.GetComponent<LevelManager>();
        //pathingGridSetup = GameManager.Instance.PathingGridManagerLink.GetComponent<PathingGridSetup>();
        placingObject = GameManager.Instance.ObjectInfoBox.GetComponent<PlacingObjectInteractionMenuUI>();


        //Initialise();

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
        else if(CurrentState == InteractionState.RoomPlacing)
        {
            RoomPlacing();
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

    //private void Initialise()
    //{
    //    tileHighlighterList = new List<GameObject>();
    //    for (int i = 0; i < noOfTileHighlighters; i++)
    //    {
    //        GameObject newTileHighlighter = Instantiate(tileHighlighterPrefab);
    //        tileHighlighterList.Add(newTileHighlighter);
    //        newTileHighlighter.SetActive(false);
    //    }
    //}

    private void SelectionMode()
    {

        if (objectGhost != null && objectGhost.gameObject.activeSelf)
        {
            objectGhost.gameObject.SetActive(false);
        }

        if (OverUI())
        {
            return;
        }

        interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(interactionRay, out hitInfo))
        {
            if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Object") == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    NullSelectedObject();
                    PlacedSelectedObject = hitInfo.collider.gameObject.GetComponentInParent<PlaceableObject>();
                    PlacedSelectedObject.Selected = true;
                }
            }

            else if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "AI") == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    NullSelectedObject();
                    currentSelectedAI = hitInfo.collider.gameObject.GetComponent<BaseAI>();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && currentSelectedObject)
                {
                    currentSelectedObject.Selected = false;
                    NullSelectedObject();
                }
            }
            ObjectInteraction();
        }
    }

    private void RoomPlacing()
    {
        interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(interactionRay, out hitInfo))
        {
            if (!startTile)
            {
                if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        roomScaler.transform.localScale = new Vector3(1, 1, 1);

                        startTile = hitInfo.collider.gameObject;

                        roomScaler.SetActive(true);
                        roomScaler.transform.position = startTile.transform.position;
                    }
                }
            }
            else if (startTile)
            {
                if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
                {
                    endTile = hitInfo.collider.gameObject;

                    if (endTile != startTile)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            startTile = null;
                            endTile = null;
                            roomScaler.SetActive(false);
                        }
                        else
                        {
                            Vector3 startPosition = startTile.transform.position;
                            Vector3 endPosition = endTile.transform.position;
                            Vector3 centrePoint = ((startPosition + endPosition) / 2) - new Vector3(0.5f, 0, 0.5f);
                            Vector3 newScale = roomScaler.transform.localScale;
                            //Vector3 newPosition = roomScaler.transform.position;

                            //newPosition.x = Mathf.Lerp(newPosition.x, centrePoint.x, Time.deltaTime * 5.0f);
                            //newPosition.y = Mathf.Lerp(newPosition.y, centrePoint.y, Time.deltaTime * 5.0f);

                            newScale.x = Mathf.Lerp(newScale.x, Mathf.Abs(startPosition.x - endPosition.x), Time.deltaTime * 25.0f);
                            newScale.y = Mathf.Lerp(newScale.y, Mathf.Abs(startPosition.z - endPosition.z), Time.deltaTime * 25.0f);

                            //newScale.x = Mathf.Abs(startPosition.x - endPosition.x);
                            //newScale.y = Mathf.Abs(startPosition.z - endPosition.z);

                            roomScaler.transform.position = centrePoint;
                            roomScaler.transform.localScale = newScale;
                        }
                    }
                }
            }
        }
    }

    private void PlacingMode()
    {
        if (OverUI())
        {
            return;
        }
        interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(interactionRay, out hitInfo))
        {
            if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Tile") == 0)
            {
                placedOnTile = hitInfo.collider.gameObject.GetComponent<Tile>();

                if (!objectGhost.activeSelf)
                    objectGhost.SetActive(true);
                objectGhost.transform.position = hitInfo.collider.transform.position;

                if (Input.GetMouseButtonDown(0))
                {
                    if (objectGhost.GetComponentInChildren<ObjectGhost>().IsPlaceable())
                    {
                        NullSelectedObject();
                        placingObject.GetComponent<Animator>().SetBool("Placing", false);
                        if (economyManager.CheckCanAfford(ObjectToPlace.BuyCost))
                        {
                            GameObject temp = InstantiateNewObject(ObjectToPlace, hitInfo.collider.gameObject.transform.position, objectGhost.transform.rotation, placedOnTile);
                            objectGhost.GetComponentInChildren<ObjectGhost>().OnPlaced(temp);
                            ObjectToPlace = null;
                        }
                    }
                    else
                    {
                        print("Something is blocking this tile!");
                    }
                }

            }
            else if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Object") == 0)
            {

                if (hitInfo.collider.gameObject.name != "Top")
                    placedOnTile = GetNearestTile(hitInfo.collider);
                if (!objectGhost.activeSelf)
                    objectGhost.SetActive(true);
                objectGhost.transform.position = placedOnTile.transform.position;

                if (Input.GetMouseButtonDown(0))
                {
                    if (objectGhost.GetComponentInChildren<ObjectGhost>().IsPlaceable())
                    {
                        NullSelectedObject();
                        if (economyManager.CheckCanAfford(ObjectToPlace.BuyCost))
                        {
                            GameObject temp = InstantiateNewObject(ObjectToPlace, objectGhost.transform.position, objectGhost.transform.rotation, placedOnTile);
                            objectGhost.GetComponentInChildren<ObjectGhost>().OnPlaced(temp);
                            ObjectToPlace = null;
                        }
                    }
                }
            }
            else if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Wall") == 0)
            {
                if (hitInfo.collider.gameObject.name != "Top")
                    placedOnTile = GetNearestTileWallVersion(hitInfo.collider);
                if (!objectGhost.activeSelf)
                    objectGhost.SetActive(true);
                objectGhost.transform.position = placedOnTile.transform.position;
                if (Input.GetMouseButtonDown(0))
                {
                    if (objectGhost.GetComponentInChildren<ObjectGhost>().IsPlaceable())
                    {
                        NullSelectedObject();
                        if (economyManager.CheckCanAfford(ObjectToPlace.BuyCost))
                        {
                            GameObject temp = InstantiateNewObject(ObjectToPlace, objectGhost.transform.position, objectGhost.transform.rotation, placedOnTile);
                            objectGhost.GetComponentInChildren<ObjectGhost>().OnPlaced(temp);
                            ObjectToPlace = null;
                        }
                    }
                }
            }
            else if (string.CompareOrdinal(hitInfo.collider.gameObject.tag, "Ghost") == 0)
            {
                print("Touching ghost! This shouldn't happen!");
            }
            else
            {
                objectGhost.SetActive(false);
                foreach (Tile tile in GameManager.Instance.SceneManagerLink.GetComponent<ObjectManager>().AllTiles)
                {
                    tile.ToggleHighlighter(false);
                }
                objectGhost.gameObject.GetComponentInChildren<ObjectGhost>().collidedTiles.Clear();
            }
        }
        ObjectInteraction();
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
        Collider[] colliders = Physics.OverlapSphere(col.transform.position, 10.0f);
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

    private Tile GetNearestTileWallVersion(Collider col)
    {
        Tile tile = null;
        Collider[] colliders = Physics.OverlapSphere(col.transform.position, 10.0f);
        Collider closestCol = null;
        foreach (Collider hit in colliders)
        {
            if (hit == col.GetComponent<Collider>() || hit.gameObject.tag != "Tile")
                continue;
            if (closestCol == null)
                closestCol = hit;
            
            if (Vector3.Distance(col.transform.position, hit.transform.position) <= Vector3.Distance(col.transform.position, closestCol.transform.position))
                closestCol = hit;
            tile = closestCol.gameObject.GetComponent<Tile>();
        }
        return tile;
    }
    

    private GameObject InstantiateNewObject(PlaceableObject objectToPlace, Vector3 position, Quaternion rotation, Tile objectTile)
    {
        GameObject newObject = Instantiate(objectToPlace.gameObject, position, rotation, objectParent);
        levelManager.AddObjectToLists(newObject);

        //pathingGridSetup.UpdateGrid();

        if (CheckIfMachineOrPlaceable(ObjectToPlace))
            economyManager.OnMachinePurchase(ObjectToPlace as Machine);
        else
            economyManager.OnBuildingPartPurchase(ObjectToPlace);
        PlaceableObject newPlaceableObject = newObject.GetComponent<PlaceableObject>();
        newPlaceableObject.PlacedOnTile = objectTile;
        newPlaceableObject.PrefabName = objectToPlace.name;
        objectTile.SetIfPlacedOn(true);
        return newObject;
    }

    private void ObjectInteraction()
    {
        if (PlacedSelectedObject)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DestroyCurrentlySelectedObject();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                PlacedSelectedObject.transform.eulerAngles += new Vector3(0.0f, 90.0f, 0.0f);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                PlacedSelectedObject.transform.eulerAngles += new Vector3(0.0f, -90.0f, 0.0f);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                objectGhost.transform.eulerAngles += new Vector3(0.0f, 90.0f, 0.0f);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                objectGhost.transform.eulerAngles += new Vector3(0.0f, -90.0f, 0.0f);
            }
        }
    }



    public void NullSelectedObject()
    {
        if (PlacedSelectedObject)
        {
            PlacedSelectedObject.Selected = false;
            PlacedSelectedObject = null;
        }
        if(CurrentSelectedAI)
        {
            CurrentSelectedAI = null;
        }
    }

    public void ClearObjectToPlace()
    {
        ObjectToPlace = null;
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
            objectGhost.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyCurrentlySelectedObject()
    {
        foreach (Tile tile in PlacedSelectedObject.GetComponent<PlaceableObject>().OccupiedTiles)
        {
            tile.SetIfPlacedOn(false);
        }

        GameManager.Instance.ScriptHolderLink.GetComponent<LevelManager>().RemoveObjectFromLists(PlacedSelectedObject.gameObject);

        Destroy(PlacedSelectedObject.gameObject);

        if (CheckIfMachineOrPlaceable(PlacedSelectedObject))
        {
            GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().CurrentCash += PlacedSelectedObject.returnAmount();
        }
        else if (!CheckIfMachineOrPlaceable(PlacedSelectedObject))
        {
            GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().CurrentCash += PlacedSelectedObject.returnAmount();
        }

        PlacedSelectedObject = null;
    }

    public void ClearObjectParent()
    {
        foreach (Transform child in objectParent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    public void ReassignObjectGhost(PlaceableObject obj)
    {
        foreach (Transform child in objectGhost.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        ObjectGhost newGhost = Instantiate(obj.GetGhost().GetComponent<ObjectGhost>());
        newGhost.transform.SetParent(objectGhost.transform);
        newGhost.transform.position = objectGhost.transform.position;
        newGhost.transform.rotation = objectGhost.transform.rotation;
        newGhost.gameObject.SetActive(true);
    }

    public PlaceableObject PlacedSelectedObject
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

    public BaseAI CurrentSelectedAI
    {
        get
        {
            return currentSelectedAI;
        }

        set
        {
            currentSelectedAI = value;
        }
    }
}
