using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform transformToFace;

    void Start()
    {
        transformToFace = GetComponent<Transform>();
    }

    void Update()
    {
        transformToFace.LookAt(Camera.main.transform.position, Vector3.forward);
    }
}
