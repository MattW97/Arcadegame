using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{	
	private int gCost;
	private int hCost;
	private int gridX;
	private int gridY;
	private int heapIndex;
	private bool walkable;	
	private Vector3 worldPosition;
	private Node parent;

	public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
	{
		this.walkable = walkable;
		this.worldPosition = worldPosition;
		this.gridX = gridX;
		this.gridY = gridY;
	}

	public void SetParent(Node parent)
	{
		this.parent = parent;
	}

	public void SetGCost(int gCost)
	{
		this.gCost = gCost;
	}

	public void SetHCost(int hCost)
	{
		this.hCost = hCost;
	}

	public Node GetParent()
	{
		return parent;
	}

	public int GetGCost()
	{
		return gCost;
	}

	public int GetHCost()
	{
		return hCost;
	}

	public int GetFCost()
	{
		return gCost + hCost;
	}

	public int GetGridX()
	{
		return gridX;
	}

	public int GetGridY()
	{
		return gridY;
	}

	public bool GetWalkable()
	{
		return walkable;
	}

	public Vector3 GetWorldPosition()
	{
		return worldPosition;
	}

	public int CompareTo(Node nodeToCompare)
	{
		int compare = GetFCost().CompareTo(nodeToCompare.GetFCost());

		if(compare == 0)
		{
			compare = hCost.CompareTo(nodeToCompare.GetHCost());
		}

		return -compare;
	}

	public int HeapIndex
	{
		get
		{
			return heapIndex;
		}

		set
		{
			heapIndex = value;
		}
	}
}
