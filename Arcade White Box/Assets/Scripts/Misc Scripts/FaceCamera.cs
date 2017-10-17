using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform transformToFace;
    private Transform cameraTransform;

    void Start()
    {
        transformToFace = GetComponent<Transform>();
        cameraTransform = Camera.main.GetComponent<Transform>();

    }

    void Update()
    {
        transformToFace.LookAt(cameraTransform.position, Vector3.forward);
    }
}
