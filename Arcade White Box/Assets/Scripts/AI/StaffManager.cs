using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Janitor janitor;

    private float levelSpeedFactor;
    private TimeAndCalendar gameTime;
    private List<Staff> spawnedStaff;
    private List<Janitor> spawnedJanitors;
    private List<Tile> currentTrashTiles;

    void Awake()
    {
        spawnedStaff = new List<Staff>();
        spawnedJanitors = new List<Janitor>();
        currentTrashTiles = new List<Tile>();
    }

    void Start()
    {
        gameTime = GameManager.Instance.ScriptHolderLink.GetComponent<TimeAndCalendar>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            SpawnJanitor();
        }

        AssignJanitorJobs();
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

    public void SpawnJanitor()
    {
        Janitor newJanitor = Instantiate(janitor, spawnLocation.position, janitor.transform.rotation);
        spawnedJanitors.Add(newJanitor);
        spawnedStaff.Add(newJanitor);
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
