using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingGraphUpdate : MonoBehaviour
{   
    private AstarPath aStarPathing;

    private void Awake()
    {
        aStarPathing = GetComponent<AstarPath>();
    }

    public void UpdateGrapthBounds(Bounds bounds)
    {
        aStarPathing.UpdateGraphs(bounds);
    }

    public void RescanEntireGraph()
    {
        aStarPathing.Scan();
    }
}
