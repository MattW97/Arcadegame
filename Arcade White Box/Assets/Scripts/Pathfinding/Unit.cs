using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour 
{	
	public float speed;

	private Vector3 originalPos;
	private Transform target;
	private Transform unitTransform;
	private Vector3[] path;
	int targetIndex;

	void Awake()
	{	
		unitTransform = transform;
		originalPos = unitTransform.position;		
	}

	void OnEnable()
	{
		originalPos = unitTransform.position;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void GetNewPath()
	{
		PathManager.RequestPath(unitTransform.position, target.position, OnPathFound);
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if(pathSuccessful)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0];

		while(true)
		{
			if(unitTransform.position == currentWaypoint)
			{
				targetIndex++;
				if(targetIndex >= path.Length)
				{	
					unitTransform.position = originalPos;
					yield break;
				}

				currentWaypoint = path[targetIndex];
			}

			unitTransform.position = Vector3.MoveTowards(unitTransform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;
		}
	}
}
