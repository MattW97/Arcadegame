using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaultyLight : MonoBehaviour {

    private Material material;
    private float emission;

    private void Start()
    {
        material = GetComponent<Renderer>().material;

    }

    void Update()
    {
        float emission = Mathf.PingPong(Time.time, Random.Range(0.2f, 2.0f));
        Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        material.SetColor("_EmissionColor", finalColor);
    }

    //private IEnumerator LightFlicker()
    //{
        
    //    return new WaitForSeconds();
    //}
}
