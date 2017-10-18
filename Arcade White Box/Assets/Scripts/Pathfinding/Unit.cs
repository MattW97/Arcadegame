using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour 
{
    [SerializeField] [MinMaxRange(1.0f, 20.0f)] private MinMaxRange movementSpeed;

    private bool reachedTarget, followingPath;
    private float setSpeed;
	private Transform target, unitTransform;
	private Vector3[] path;
	private int targetIndex;

    void Awake()
	{
        unitTransform = GetComponent<Transform>();

        ReachedTarget = false;
        setSpeed = movementSpeed.GetRandomValue();
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

    public void StopCurrentPathing()
    {
        if(FollowingPath)
        {
            StopCoroutine("FollowPath");
            FollowingPath = false;
            ReachedTarget = false;
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

			unitTransform.position = Vector3.MoveTowards(unitTransform.position, currentWaypoint, setSpeed * Time.deltaTime);
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
