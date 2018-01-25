using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pather : MonoBehaviour
{
    [SerializeField] [MinMaxRange(1.0f, 20.0f)] private MinMaxRange movementSpeed;

    private AIPath aiPath;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
    }

    private void Start()
    {
        aiPath.maxSpeed = movementSpeed.GetRandomValue();
    }
    
    public void SetTarget(Transform target)
    {
        aiPath.destination = target.position;
    }
    
    public void SetSpeedFactor(float speedFactor)
    {
        
    }

    public bool ReachedTarget()
    {
        return aiPath.reachedEndOfPath; 
    }
}
