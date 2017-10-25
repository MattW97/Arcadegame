using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {
    public float timer = 8f;
    public string levelToLoad = "Main Menu";


	// Use this for initialization
	void Start () {
        StartCoroutine(DisplayScene());        		
	}
	
	IEnumerator DisplayScene()
    {
        yield return new WaitForSeconds(timer);
        print("switch");
        SceneManager.LoadScene(2);
    }
}
