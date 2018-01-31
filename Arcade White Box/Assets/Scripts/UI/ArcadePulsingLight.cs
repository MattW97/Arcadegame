using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadePulsingLight : MonoBehaviour {

    public float duration = 1.0F;

    public Light lt;
    void Start()
    {
        lt = GetComponent<Light>();
    }
    void Update()
    {
        float phi = Time.time / duration * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.1f + 0.7f;
        lt.intensity = amplitude;
    }
}
