using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMovement : MonoBehaviour {

    private Animator anim;


    // Use this for initialization
    void Start () {

        anim = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.UpArrow))
            StartCoroutine(PlayDownAnimation());

        if (Input.GetKeyDown(KeyCode.DownArrow))
            StartCoroutine(PlayUpAnimation());

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            StartCoroutine(PlayLeftAnimation());

        if (Input.GetKeyDown(KeyCode.RightArrow))
            StartCoroutine(PlayRightAnimation());

        if (Input.GetKeyDown(KeyCode.Return))
            StartCoroutine(PlayEnterAnimation());

    }

    IEnumerator PlayDownAnimation()
    {
        anim.SetBool("Up", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Up", false);
    }

    IEnumerator PlayUpAnimation()
    {
        anim.SetBool("Down", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Down", false);
    }

    IEnumerator PlayLeftAnimation()
    {
        anim.SetBool("Left", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Left", false);
    }

    IEnumerator PlayRightAnimation()
    {
        anim.SetBool("Right", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Right", false);
    }

    IEnumerator PlayEnterAnimation()
    {
        anim.SetBool("Enter", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Enter", false);
    }


}
