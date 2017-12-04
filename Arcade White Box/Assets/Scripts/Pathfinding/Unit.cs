using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour 
{
    [SerializeField] [MinMaxRange(1.0f, 20.0f)] private MinMaxRange movementSpeed;
    [SerializeField] private float turnDistance;
    [SerializeField] private float turnSpeed;

    const float PATH_UPDATE_THRESHOLD = 0.5f;
    const float MIN_PATH_UPDATE_TIME = 0.2f;

    private bool reachedTarget, followingPath;
    private float speedFactor, realSpeed;
	private Transform target, unitTransform;
    private PathingPath path;
	private int targetIndex;

    void Awake()
	{
        unitTransform = GetComponent<Transform>();

        ReachedTarget = false;
        realSpeed = movementSpeed.GetRandomValue();
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

    public void GetRandomNewPath()
    {
        ReachedTarget = false;
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new PathingPath(waypoints, unitTransform.position, turnDistance);
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

    private IEnumerator UpdatePath()
    {   
        if(Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }

        PathManager.RequestPath(unitTransform.position, target.position, OnPathFound);
        float sqrMoveThreshold = PATH_UPDATE_THRESHOLD * PATH_UPDATE_THRESHOLD;
        Vector3 targetPosOld = target.position;

        while(true)
        {
            yield return new WaitForSeconds(MIN_PATH_UPDATE_TIME);
            if((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathManager.RequestPath(unitTransform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    private IEnumerator FollowPath()
    {
        FollowingPath = true;
        ReachedTarget = false;
        int pathIndex = 0;

        if (path.finishLineIndex == 0 || path.lookPoints.Length == 0)
        {
            ReachedTarget = true;
            FollowingPath = false;
            print("UNIT WAS ALREADY AT TARGET LOCATION");
            yield break;
        }

        unitTransform.LookAt(path.lookPoints[0]);

        while (FollowingPath)
        {
            Vector2 pos2D = new Vector2(unitTransform.position.x, unitTransform.position.z);
            while(path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if(pathIndex == path.finishLineIndex)
                {
                    ReachedTarget = true;
                    FollowingPath = false;
                    yield break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (FollowingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - unitTransform.position);
                unitTransform.rotation = Quaternion.Lerp(unitTransform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                unitTransform.Translate(Vector3.forward * Time.deltaTime * (realSpeed * speedFactor), Space.Self);
            }

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

    public float SpeedFactor
    {
        get
        {
            return speedFactor;
        }

        set
        {
            speedFactor = value;
        }
    }
    
    public void OnDrawGizmos()
    {
        if(path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
