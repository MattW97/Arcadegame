using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Janitor janitor;
    [SerializeField] private Mechanic mechanic;

    private LevelInteraction _tileInteractionLink;

    private float levelSpeedFactor;
    private TimeAndCalendar gameTime;
    private List<Staff> spawnedStaff;
    private List<Janitor> spawnedJanitors;
    private List<Mechanic> spawnedMechanics;


    private List<Tile> currentTrashTiles;
    private List<Machine> repairList;

    private StaffDetails staffDetails;

    void Awake()
    {
        spawnedStaff = new List<Staff>();
        spawnedJanitors = new List<Janitor>();
        spawnedMechanics = new List<Mechanic>();
        currentTrashTiles = new List<Tile>();
        repairList = new List<Machine>();
    }

    void Start()
    {
        

        gameTime = GameManager.Instance.ScriptHolderLink.GetComponent<TimeAndCalendar>();
        _tileInteractionLink = GameManager.Instance.SceneManagerLink.GetComponent<LevelInteraction>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnMechanic();
            print("spawned mechanic");
        }

        AssignJanitorJobs();
        AssignMechanicJobs();
    }

    private void AssignJanitorJobs()
    {
        foreach(Janitor janitor in spawnedJanitors)
        {
            if(!janitor.HasJob() && (currentTrashTiles.Count > 0))
            {
                janitor.SetNewTarget(currentTrashTiles[0]);
                currentTrashTiles.RemoveAt(0);
            }
        }
    }

    private void AssignMechanicJobs()
    {
        foreach (Mechanic mechanic in spawnedMechanics)
        {
            if (!mechanic.HasJob && (spawnedMechanics.Count > 0) && (repairList.Count > 0))
            {
                mechanic.AssignMachine(repairList[0]);
                repairList.RemoveAt(0);
                print("Assigned a Janitor to fix a machine!");
            }
        }
    }
    public void AddMachineToRepairList()
    {
        if (!repairList.Contains(_tileInteractionLink.PlacedSelectedObject as Machine))
        {
            repairList.Add(_tileInteractionLink.PlacedSelectedObject as Machine);
            print("Added " + _tileInteractionLink.PlacedSelectedObject.name + "to the list of machines to be repaired!");
        }
    }

    public Janitor SpawnJanitor()
    {
        Janitor newJanitor = Instantiate(janitor, spawnLocation.position, janitor.transform.rotation);
        spawnedJanitors.Add(newJanitor);
        spawnedStaff.Add(newJanitor);

        return newJanitor;
    }

    public Mechanic SpawnMechanic()
    {
        Mechanic newMechanic = Instantiate(mechanic, spawnLocation.position, janitor.transform.rotation);
        spawnedMechanics.Add(newMechanic);
        spawnedStaff.Add(newMechanic);
        return newMechanic;
    }

    public float GetSpeedFactor()
    {
        return gameTime.timeMultiplier;
    }

    public void AddToTrashTiles(Tile trashTile)
    {   
        if(!currentTrashTiles.Contains(trashTile))
        {
            currentTrashTiles.Add(trashTile);
        }
    }
}
