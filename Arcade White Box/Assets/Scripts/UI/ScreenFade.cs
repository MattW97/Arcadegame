using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    private Animator screenFadeOut;
    public GameObject image;


    void Start()
    {
        image.SetActive(false);
        screenFadeOut = this.GetComponent<Animator>();
    }

    public void FadeOut()
    {
        image.SetActive(true);
        screenFadeOut.SetBool("Fade Out", true);
    }
}

