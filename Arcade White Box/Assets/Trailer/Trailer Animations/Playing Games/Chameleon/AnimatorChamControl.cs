using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorChamControl : MonoBehaviour {

    Animator m_Animator;

    void Start()
    {
        //This gets the Animator, which should be attached to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();

        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {        
        yield return new WaitForSeconds(1);
        m_Animator.SetBool("Idle", false);
        m_Animator.SetBool("Walking", true);
    }

    void Update()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk Track"))
        {
            m_Animator.SetBool("Walking", false);
        }
    }
}
