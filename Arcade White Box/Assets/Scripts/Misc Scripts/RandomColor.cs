using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour {

    public Transform parent;
    public List<Color> colours;

    List<Transform> objectstoColor;

    void OnEnable()
    {
        objectstoColor = new List<Transform>();

        foreach (Transform item in parent)
        {
            objectstoColor.Add(item);
        }

        SwitchColors();
    } 
    
    public void SwitchColors()
    {
        foreach (Transform item in objectstoColor)
        {
            Renderer rend = item.gameObject.GetComponent<Renderer>();
           // rend.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            rend.material.color = colours[Random.Range(0, colours.Count)];
        }
    }
}
