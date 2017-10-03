using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour {
    [SerializeField] private float hireCost, wageCost;

    public float HireCost
    {
        get
        {
            return hireCost;
        }

        set
        {
            hireCost = value;
        }
    }

    public float WageCost
    {
        get
        {
            return wageCost;
        }

        set
        {
            wageCost = value;
        }
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
