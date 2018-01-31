using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PathingGraphUpdate : MonoBehaviour
{   
    [SerializeField] private AstarPath aStarPathing;
    [SerializeField] private Collider box;

    private void Update()
    {   
        if(Input.GetKeyDown(KeyCode.L))
        {
            UpdateGrapthBounds(box.bounds);
        }
    }

    public void UpdateGrapthBounds(Bounds bounds)
    {
        AstarPath.active.UpdateGraphs(bounds);
        //aStarPathing.Scan();
        print("UPDATED AT BOUNDS: " + bounds);
    }

    public void RescanEntireGraph()
    {
        aStarPathing.Scan();
    }
}
