using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelect : MonoBehaviour {

    [SerializeField]
    private GameObject startMenu, gameModeMenu, careerMenu, sandboxMenu, loadMenu, optionsMenu, soundOptions, visualOptions;

    private Animator anim;

    void Start()
    {

        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
            StartCoroutine(PlayEnterAnimation());

        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(PlayQuitAnimation());

    }

    IEnumerator PlayEnterAnimation()
    {        
        anim.SetBool("Enter", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Enter", false);
    }

    IEnumerator PlayQuitAnimation()
    {
        anim.SetBool("Esc", true);

        startMenu.SetActive(true);
        gameModeMenu.SetActive(false);
        careerMenu.SetActive(false);
        sandboxMenu.SetActive(false);
        loadMenu.SetActive(false);
        optionsMenu.SetActive(false);
        soundOptions.SetActive(false);
        visualOptions.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Esc", false);
    }
}
