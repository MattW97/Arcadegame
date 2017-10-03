using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObject : MonoBehaviour {

    private Tile placedOnTile;

    [SerializeField] private string gameName, description;
    [SerializeField] private float gameCost, machineCost, runningCost, machineMaintenenceCost;
    [SerializeField] private int minNumberOfPlaysBeforeBreak, maxNumberOfPlaysBeforeBreak;


    private int numOfPlays;

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

    public float MachineMaintenenceCost
    {
        get
        {
            return machineMaintenenceCost;
        }

        set
        {
            machineMaintenenceCost = value;
        }
    }

    public int MinNumberOfPlaysBeforeBreak
    {
        get
        {
            return minNumberOfPlaysBeforeBreak;
        }

        set
        {
            minNumberOfPlaysBeforeBreak = value;
        }
    }

    public int MaxNumberOfPlaysBeforeBreak
    {
        get
        {
            return maxNumberOfPlaysBeforeBreak;
        }

        set
        {
            maxNumberOfPlaysBeforeBreak = value;
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    //call this BEFORE destroying the object this script is attached to.
    public void Cleanup()
    {
        placedOnTile.SetTileType(Tile.TileType.Passable);
    }

    public void setTile(Tile newTile)
    {
        placedOnTile = newTile;
    }

    
}
