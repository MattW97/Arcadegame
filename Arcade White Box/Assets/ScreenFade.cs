using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    private Animator screenFadeOut;


    void Start()
    {
        screenFadeOut = this.GetComponent<Animator>();
    }

    public void FadeOut()
    {
        screenFadeOut.SetBool("Fade Out", true);
    }
}

