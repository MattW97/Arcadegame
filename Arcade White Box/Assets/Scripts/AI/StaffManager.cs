using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Janitor janitor;
    [SerializeField] private Worker worker;

    private float levelSpeedFactor;
    private TimeAndCalendar gameTime;
    private List<Staff> spawnedStaff;
    private List<Janitor> spawnedJanitors;
    private List<Worker> spawnedWorkers;
    private List<Tile> availableJanitorJobs;
    private List<WorkerJob> availableWorkerJobs;

    private StaffDetails staffDetails;

    void Awake()
    {
        spawnedStaff = new List<Staff>();
        spawnedJanitors = new List<Janitor>();
        spawnedWorkers = new List<Worker>();
        availableJanitorJobs = new List<Tile>();
        availableWorkerJobs = new List<WorkerJob>();
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
            if(!janitor.HasJob() && (availableJanitorJobs.Count > 0))
            {
                janitor.SetNewTarget(availableJanitorJobs[0]);
                availableJanitorJobs.RemoveAt(0);
            }
        }
    }

    private void AssignWorkerJobs()
    {
        foreach(Worker worker in spawnedWorkers)
        {
            if(!worker.HasJob() && (availableWorkerJobs.Count > 0))
            {

            }
        }
    }

    public Janitor SpawnJanitor()
    {
        Janitor newJanitor = Instantiate(janitor, spawnLocation.position, janitor.transform.rotation);
        spawnedJanitors.Add(newJanitor);
        spawnedStaff.Add(newJanitor);

        return newJanitor;
    }

    public Worker SpawnWorker()
    {
        Worker newWorker = Instantiate(worker, spawnLocation.position, worker.transform.rotation);
        spawnedWorkers.Add(newWorker);
        spawnedStaff.Add(newWorker);

        return newWorker;
    }

    public float GetSpeedFactor()
    {
        return gameTime.timeMultiplier;
    }

    public void AddToTrashTiles(Tile trashTile)
    {   
        if(!availableJanitorJobs.Contains(trashTile))
        {
            availableJanitorJobs.Add(trashTile);
        }
    }
}
