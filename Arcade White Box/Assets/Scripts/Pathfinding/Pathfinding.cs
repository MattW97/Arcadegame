using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding2 : MonoBehaviour 
{	
	private PathingGrid grid;
	private PathManager pathManager;

	public void SetGrid(PathingGrid grid)
	{
		this.grid = grid;
	}

	public void SetPathManager(PathManager pathManager)
	{
		this.pathManager = pathManager;
	}

	public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
	{
		StartCoroutine(FindPath(startPosition, targetPosition));
	}

	IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
	{
        Vector3[] wayPoints = new Vector3[0];
		bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPosition);
		Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        if (startNode.GetWalkable() && targetNode.GetWalkable())
		{
			Heap<Node> openSet = new Heap<Node>(grid.GetMaxSize());
			HashSet<Node> closedSet = new HashSet<Node>();

			openSet.Add(startNode);

			while(openSet.GetCount() > 0)
			{	
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if(currentNode == targetNode)
				{	
					pathSuccess = true;
					break;
				}

				foreach(Node neighbour in grid.GetNeighbours(currentNode))
				{
                    if (!neighbour.GetWalkable() || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newMovementCostToNeighbour = currentNode.GetGCost() + GetDistance(currentNode, neighbour);

					if(newMovementCostToNeighbour < neighbour.GetGCost() || !openSet.Contains(neighbour))
					{
						neighbour.SetGCost(newMovementCostToNeighbour);
						neighbour.SetHCost(GetDistance(neighbour, targetNode));
						neighbour.SetParent(currentNode);

						if(!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
						}
						else
						{
							openSet.UpdateItem(neighbour);
						}
					}
				}
			}
		}
        else
        {
            print(gameObject.name + " NO WALKABLE: STARTNODE- " + startNode.GetWalkable() + " TARGET NODE- " + targetNode.GetWalkable());

        }

		yield return null;

		if(pathSuccess)
		{
			wayPoints = RetracePath(startNode, targetNode);
		}
        else
        {
            print("PATHING WASN'T SUCCESSFUL");
        }

		pathManager.FinishedProcessingPath(wayPoints, pathSuccess);
	}

	private Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while(currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.GetParent();
		}

		Vector3[] wayPoints = SimplifyPath(path);

		Array.Reverse(wayPoints);

		return wayPoints;
	}

	private Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> wayPoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for(int i = 1; i < path.Count; i++)
		{
			Vector2 directionNew = new Vector2(path[i - 1].GetGridX() - path[i].GetGridX(), path[i - 1].GetGridY() - path[i].GetGridY());
			if(directionNew != directionOld)
			{
				wayPoints.Add(path[i].GetWorldPosition());
			}

			directionOld = directionNew;
		}

		return wayPoints.ToArray();
	}

	private int GetDistance(Node nodeA, Node nodeB)
	{
		int distX = Mathf.Abs(nodeA.GetGridX() - nodeB.GetGridX());
		int distY = Mathf.Abs(nodeA.GetGridY() - nodeB.GetGridY());

		if(distX > distY)
		{
			return 14 * distY + 10 * (distX - distY);
		}
		else
		{
			return 14 * distX + 10 * (distY - distX);
		}
	}
}
