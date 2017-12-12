﻿using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    [SerializeField]
    [MinMaxRange(1.0f, 20.0f)]
    private MinMaxRange movementSpeed;

    private bool reachedTarget, followingPath;
    private float speedFactor, realSpeed;
    private Transform target, unitTransform;
    private Vector3[] path;
    private int targetIndex;

    void Awake()
    {
        unitTransform = GetComponent<Transform>();

        ReachedTarget = false;
        realSpeed = movementSpeed.GetRandomValue();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void GetNewPath()
    {
        ReachedTarget = false;

        PathManager.RequestPath(unitTransform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            print("NEW PATH FOUND");

            path = newPath;
            targetIndex = 0;

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        else
        {
            print("NO PATH FOUND");
        }
    }

    public void StopCurrentPathing()
    {
        StopCoroutine("FollowPath");
        FollowingPath = false;
        ReachedTarget = false;

        print("CURRENT PATHING STOPPED");
    }

    private IEnumerator FollowPath()
    {
        print("FOLLOWING PATH");

        FollowingPath = true;
        ReachedTarget = false;
        Vector3 currentWaypoint = Vector3.zero;

        if (path.Length == 0)
        {
            ReachedTarget = true;
            FollowingPath = false;
            yield break;
        }
        else
        {
            currentWaypoint = path[0];
        }

        while (FollowingPath)
        {
            if (unitTransform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    ReachedTarget = true;
                    FollowingPath = false;
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }

            unitTransform.position = Vector3.MoveTowards(unitTransform.position, currentWaypoint, (realSpeed * SpeedFactor) * Time.deltaTime);
            unitTransform.LookAt(currentWaypoint);
            yield return null;
        }
    }

    public bool ReachedTarget { get { return reachedTarget; } set { reachedTarget = value; } }
    public bool FollowingPath { get { return followingPath; } set { followingPath = value; } }
    public float SpeedFactor { get { return speedFactor; } set { speedFactor = value; } }
}
