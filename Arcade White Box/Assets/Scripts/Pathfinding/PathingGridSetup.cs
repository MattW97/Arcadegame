using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingGridSetup : MonoBehaviour
{
    [SerializeField] private Vector2 gridSize;
 
    private PathingGrid pathingGrid;
    private PathManager pathManager;
    private Pathfinding pathFinding;

    void Start()
    {
        pathingGrid = GetComponent<PathingGrid>();
        pathManager = GetComponent<PathManager>();
        pathFinding = GetComponent<Pathfinding>();

        InitialiseGrid();
    }

    public void UpdateGrid()
    {
        pathingGrid.CreateGrid();
    }

    public void InitialiseGrid()
    {
        pathFinding.SetGrid(pathingGrid);
        pathFinding.SetPathManager(pathManager);

        pathingGrid.SetGridSize(gridSize);
        pathingGrid.SetupGrid();
        pathingGrid.CreateGrid();
    }
}
