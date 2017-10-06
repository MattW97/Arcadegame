using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTesting : MonoBehaviour
{
    [SerializeField] private Unit testUnit;
    [SerializeField] private Transform target;
    [SerializeField] private PathingGrid grid;

    private PathManager manager;
    private Pathfinding pathfinding;

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        manager = GetComponent<PathManager>();

        pathfinding.SetGrid(grid);
        pathfinding.SetPathManager(manager);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            grid.CreateGrid();
            print("Grid Created");
            testUnit.SetTarget(target);
            print("Target Set");
            testUnit.GetNewPath();
            print("New Path Given");
        }
    }
}
