using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    private Animator zoomAnim;

    void Start()
    {
        zoomAnim = this.GetComponent<Animator>();
    }

    public void Zoom()
    {
        zoomAnim.Play("Camera Zoom");
    }

}
