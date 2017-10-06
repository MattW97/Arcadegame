﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathingGrid : MonoBehaviour 
{	
	[SerializeField] private float nodeRadius;
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private bool drawGrid;

	private float nodeDiameter;
	private int gridSizeX, gridSizeY;
    private Vector2 gridWorldSize;
	private Node[,] grid;

    void Update()
    {
        
    }

    public void SetupGrid()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    }

	public void CreateGrid()
	{	
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottemLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for(int x = 0; x < gridSizeX; x++)
		{
			for(int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottemLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

				grid[x, y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();

		for(int x = -1; x <= 1; x++)
		{
			for(int y = -1; y <= 1; y++)
			{
				if(x == 0 && y == 0)
				{
					continue;
				}

				int checkX = node.GetGridX() + x;
				int checkY = node.GetGridY() + y;

				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);	
				}
			}
		}

		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

		return grid[x, y];
	}

	public int GetMaxSize()
	{
		return gridSizeX * gridSizeY;
	}

	public Vector2 GetGridSize()
	{
		return gridWorldSize;
	}

	public void SetGridSize(Vector2 gridWorldSize)
	{
		this.gridWorldSize = gridWorldSize;
	}

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null && drawGrid)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.GetWalkable() ? new Color(1, 1, 1, 0.2f) : new Color(1, 0, 0, 0.2f));
                Gizmos.DrawWireCube(n.GetWorldPosition(), Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}