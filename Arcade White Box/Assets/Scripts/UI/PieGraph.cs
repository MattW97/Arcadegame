using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieGraph : MonoBehaviour {

    public Image wedgePrefab;
    public List<float> values;
    public List<Color> wedgeColours;


	// Use this for initialization
	void Start () {
        MakeGraph();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MakeGraph()
    {
        float total = 0.0f;
        float zRotation = 0.0f;
        for (int i = 0; i < values.Count; i++)
        {
            total += values[i];
        }

        for (int i = 0; i < values.Count; i++)
        {
            Image newWedge = Instantiate(wedgePrefab) as Image;
            newWedge.transform.SetParent(transform, false);
            newWedge.color = wedgeColours[i];
            newWedge.fillAmount = values[i] / total;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360f;
        }
    }

    public void SetNewValues(List<float> newValues)
    {
        values = newValues;
    }
}
