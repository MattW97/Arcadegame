﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Empty, Passable, Impassable, Object, Wall }

    [SerializeField] private TileType tileType;

    private int id;
    private Vector2 tileCoordinates;

    public TileType GetTileType()
    {
        return tileType;
    }

    public Vector2 GetCoordinates()
    {
        return tileCoordinates;
    }

    public void SetID(int id)
    {
        this.id = id;
    }

    public void SetTileType(TileType newType)
    {
        tileType = newType;
    }

    public void SetCoordinates(Vector2 tileCoordinates)
    {
        this.tileCoordinates = tileCoordinates;
    }

    public virtual TileSaveable GetTileSaveable()
    {
        TileSaveable newSave = new TileSaveable();
        newSave.tile_ID = this.id.ToString();
        newSave.posX = this.transform.position.x;
        newSave.posY = this.transform.position.y;
        newSave.posZ = this.transform.position.z;
        // TEXTURE ID NEEDS TO GO HERE!

        return newSave;
    }
}

[System.Serializable]
public class TileSaveable
{
    public string tile_ID;
    public float posX, posY, posZ;
    public int textureID;
}
