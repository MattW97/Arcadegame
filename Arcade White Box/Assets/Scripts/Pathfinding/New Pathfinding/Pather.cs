using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pather : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    private Seeker seeker;
    private Path path;
    private AIPath aiPath;

    private const float MAX_WAYPOINT_DISTANCE = 0.5f;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
    }

    private void Start()
    {
        aiPath.destination = target.position;
    }
}
