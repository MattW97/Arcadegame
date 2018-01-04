using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHider : MonoBehaviour {
    public new Camera camera;

    //List of all objects that we have hidden.
    public List<Transform> hiddenObjects;

    //Layers to hide
    public LayerMask layerMask;
    
    public float maxDistance;

    private Transform currentHit;

    private Transform wasHidden;

    private void Start()
    {
        //Initialize the list
        hiddenObjects = new List<Transform>();        
    }

    void Update()
    {

        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        //Find the direction from the camera to the player
        //Vector3 direction = player.position - camera.position;

        //The magnitude of the direction is the distance of the ray
        //float distance = direction.magnitude;

        //Raycast and store all hit objects in an array. Also include the layermaks so we only hit the layers we have specified
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask);
             
        //Go through the objects
        for (int i = 0; i < hits.Length; i++)
        {
            currentHit = hits[i].transform;

            //Only do something if the object is not already in the list
            if (!hiddenObjects.Contains(currentHit))
            {
                //string val = currentHit.gameObject.layer.ToString();
                //print(val);
                //Add to list and disable renderer
                hiddenObjects.Add(currentHit);
                StartCoroutine(FadeOut(0.3f, 0.1f));
                //currentHit.GetComponent<Renderer>().enabled = false;
            }
        }

        //clean the list of objects that are in the list but not currently hit.
        for (int i = 0; i < hiddenObjects.Count; i++)
        {
            bool isHit = false;
            //Check every object in the list against every hit
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j].transform == hiddenObjects[i])
                {
                    isHit = true;
                    break;
                }
            }

            //If it is not among the hits
            if (!isHit)
            {
                //Enable renderer, remove from list, and decrement the counter because the list is one smaller now
                wasHidden = hiddenObjects[i];
                StartCoroutine(FadeIn(1.0f, 0.1f));
                //wasHidden.GetComponent<Renderer>().enabled = true;
                hiddenObjects.RemoveAt(i);
                i--;
            }
        }
                
    }

    IEnumerator FadeOut(float aValue, float aTime)
    {
        float alpha = currentHit.GetComponent<Renderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            currentHit.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }
    }
    IEnumerator FadeIn(float aValue, float aTime)
    {
        float alpha = wasHidden.GetComponent<Renderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            wasHidden.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }
    }
}
