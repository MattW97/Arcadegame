using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Empty, Passable, Impassable, Object, Wall }

    [SerializeField] private TileType tileType;

    private Vector2 tileCoordinates;

    public TileType GetTileType()
    {
        return tileType;
    }

    public Vector2 GetCoordinates()
    {
        return tileCoordinates;
    }

    public void SetTileType(TileType newType)
    {
        tileType = newType;
    }

    public void SetCoordinates(Vector2 tileCoordinates)
    {
        this.tileCoordinates = tileCoordinates;
    }
}
