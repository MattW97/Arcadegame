using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour 
{	
	[SerializeField] private float speed;

    private bool reachedTarget, followingPath;
	private Transform target, unitTransform;
	private Vector3[] path;
	private int targetIndex;

    void Awake()
	{
        unitTransform = GetComponent<Transform>();

        ReachedTarget = false;
    }

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void GetNewPath()
	{
        ReachedTarget = false;

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

	private IEnumerator FollowPath()
	{
        FollowingPath = true;

		Vector3 currentWaypoint = path[0];

		while(true)
		{
			if(unitTransform.position == currentWaypoint)
			{
				targetIndex++;
				if(targetIndex >= path.Length)
                {
                    ReachedTarget = true;
                    FollowingPath = false;
                    yield break;
				}

				currentWaypoint = path[targetIndex];
			}

			unitTransform.position = Vector3.MoveTowards(unitTransform.position, currentWaypoint, speed * Time.deltaTime);
            unitTransform.LookAt(currentWaypoint);
			yield return null;
		}
    }

    public bool ReachedTarget
    {
        get
        {
            return reachedTarget;
        }

        set
        {
            reachedTarget = value;
        }
    }

    public bool FollowingPath
    {
        get
        {
            return followingPath;
        }

        set
        {
            followingPath = value;
        }
    }
}
