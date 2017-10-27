using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    [SerializeField] private Janitor janitor;

    private float levelSpeedFactor;
    private TimeAndCalendar gameTime;
    private List<Staff> spawnedStaff;

    void Awake()
    {
        spawnedStaff = new List<Staff>();
    }

    void Start()
    {
        gameTime = GameManager.Instance.SceneManagerLink.GetComponent<TimeAndCalendar>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(janitor, new Vector3(10, 10, 10), janitor.transform.rotation);
        }
    }

    public float GetSpeedFactor()
    {
        return gameTime.timeMultiplier;
    }
}
