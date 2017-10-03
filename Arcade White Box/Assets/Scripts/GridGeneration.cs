using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private Vector2 gridSize;

    private List<Tile> gridTiles;
    private GameObject tileHolder;

    void Start()
    {
        tileHolder = new GameObject("Grid Tiles");

        gridTiles = new List<Tile>();

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        float widthOffset = 0;
        float lengthOffset = 0;

        if (gridSize.x % 2 == 0)
        {
            widthOffset = 0.5f;
        }

        if (gridSize.y % 2 == 0)
        {
            lengthOffset = 0.5f;
        }

        Vector3 startPoint = new Vector3((-(gridSize.x / 2)) + widthOffset, 0, (-(gridSize.y / 2)) + lengthOffset);

        for (int i = 0; i < gridSize.x; i++)
        {
            NewTile(startPoint, new Vector2(i, 0));

            for (int j = 0; j < gridSize.y - 1; j++)
            {
                startPoint.z++;

                NewTile(startPoint, new Vector2(i, j + 1));
            }

            startPoint.z = (-(gridSize.y / 2)) + lengthOffset;
            startPoint.x++;
        }
    }

    private void NewTile(Vector3 tilePosition, Vector2 coordinates)
    {
        GameObject tileGameObject = Instantiate(tile, tilePosition, tile.transform.rotation) as GameObject;
        tileGameObject.transform.parent = tileHolder.transform;

        Tile newTile = tileGameObject.GetComponent<Tile>();

        newTile.SetCoordinates(coordinates);
        newTile.SetTileType(Tile.TileType.Passable);
    }
}
