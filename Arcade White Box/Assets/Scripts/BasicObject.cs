using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObject : MonoBehaviour
{
    [SerializeField] private string gameName, description;
    [SerializeField] private float gameCost, machineCost, runningCost, maintenenceCost;
    [SerializeField] private int minDurability, maxDurability;
    [SerializeField] private GameObject selectionMesh;

    private bool selected;
    private int numOfPlays;
    private Tile placedOnTile;
    private Transform selectionTransform;

    void Awake()
    {
        selectionTransform = selectionMesh.GetComponent<Transform>();
        selectionMesh.SetActive(false);
        Selected = false;
    }

    void Update()
    {
        if(Selected)
        {
            PulseScale();
        }
    }

    void OnMouseOver()
    {
        selectionMesh.SetActive(true);
    }

    void OnMouseExit()
    {   
        if(!Selected)
        {
            selectionMesh.SetActive(false);
        }
    }

    private void PulseScale()
    {
        selectionTransform.localScale = new Vector3(Mathf.PingPong(Time.time, 0.2f) + 1.0f, Mathf.PingPong(Time.time, 0.2f) + 1.0f, Mathf.PingPong(Time.time, 0.2f) + 1.0f);
    }

    public void IncrementPlays()
    {
        numOfPlays++;
    }

    public string GameName
    {
        get
        {
            return gameName;
        }

        set
        {
            gameName = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public float GameCost
    {
        get
        {
            return gameCost;
        }

        set
        {
            gameCost = value;
        }
    }

    public float MachineCost
    {
        get
        {
            return machineCost;
        }

        set
        {
            machineCost = value;
        }
    }

    public float RunningCost
    {
        get
        {
            return runningCost;
        }

        set
        {
            runningCost = value;
        }
    }

    public float MaintenenceCost
    {
        get
        {
            return maintenenceCost;
        }

        set
        {
            maintenenceCost = value;
        }
    }

    public int MinDurability
    {
        get
        {
            return minDurability;
        }

        set
        {
            minDurability = value;
        }
    }

    public int MaxDurability
    {
        get
        {
            return MaxDurability;
        }

        set
        {
            MaxDurability = value;
        }
    }

    public bool Selected
    {
        get
        {
            return selected;
        }

        set
        {
            selected = value;

            if(!selected)
            {
                selectionTransform.localScale = new Vector3(1, 1, 1);
                selectionMesh.SetActive(false);
            }
        }
    }

    public Tile PlacedOnTile
    {
        get
        {
            return placedOnTile;
        }

        set
        {
            placedOnTile = value;
        }
    }
}
