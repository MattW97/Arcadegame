using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadePulsingLightBad : MonoBehaviour {

    public float minDuration = 1.0F;       
    public float maxDuration = 2.0F;

    public float minInt = 1.0F;
    public float maxInt = 1.5F;

    public Light lt;
    void Start()
    {
        lt = GetComponent<Light>();
    }
    void Update()
    {
        float duration = Random.Range(minDuration, maxDuration);
        float phi = Time.time / duration * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * minInt + maxInt;
        lt.intensity = amplitude;
    }
}
