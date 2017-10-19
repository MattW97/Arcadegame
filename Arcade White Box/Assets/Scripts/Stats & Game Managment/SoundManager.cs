using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    
    public AudioClip[] menuAudio;    
    public AudioClip[] inGameAudio;
    
    private AudioSource gameAudioSource;

    private int trackNumber = 0;

   // public bool ignoreListenerPause;
    public string currentSongName;
    private float length;
    public bool isPaused = false;

    //public float fadeInDuration = 1;
    //public float fadeOutDuration = 1;   

    void OnApplicationFocus(bool hasFocus)
    {        
        if (!hasFocus)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            isPaused = false;
        }
    }


    public void Update()
    {
        //gameAudioSource.ignoreListenerPause = true;
    }

    public void MenuAudio()
    {
        gameAudioSource.clip = menuAudio[Random.Range(0, menuAudio.Length)];
        gameAudioSource.Play();
    }

    public void InGameAudio()
    {     

        if (!gameAudioSource.isPlaying && !isPaused && trackNumber < inGameAudio.Length)
        {
            gameAudioSource.clip = inGameAudio[trackNumber];
            gameAudioSource.Play();
            currentSongName = gameAudioSource.clip.ToString();
            //print(gameAudioSource.clip.length.ToString());
            print("The current song is " + currentSongName);
            trackNumber++;
        }

        if (trackNumber == inGameAudio.Length)
        {
            trackNumber = 0;
        }
    }   

    //private IEnumerator FadeOut()
    //{
    //    if (fadeOutDuration > 0f)
    //    {
    //        float startTime = gameAudioSource.clip.length - fadeOutDuration;
    //        float lerpValue = 0f;
    //        while (lerpValue < 1f)
    //        {
    //            lerpValue = Mathf.InverseLerp(startTime, gameAudioSource.clip.length, gameAudioSource.time);
    //            gameAudioSource.volume = Mathf.Lerp(gameAudioSource.volume, 0f, lerpValue);
    //            yield return null;
    //        }
    //    }
    //}

    //private IEnumerator FadeIn()
    //{
    //    if (fadeInDuration < 1f)
    //    {
    //        print("fade in 1");
    //        float lerpValue = 0f;
    //        while (lerpValue < 1f)
    //        {
    //            lerpValue = Mathf.InverseLerp(0f, fadeInDuration, gameAudioSource.time);
    //            gameAudioSource.volume = Mathf.Lerp(0f, gameAudioSource.volume, lerpValue);
    //            print("fade in 2");
    //            yield return null;
    //        }
    //    }
    //    else
    //    {
    //        yield break;
    //    }
    //}

    public void NextSong()
    {
        gameAudioSource.clip = inGameAudio[0];
    }

    public void setAudioSource(AudioSource newAudioSource)
    {
        gameAudioSource = newAudioSource;
    }

    
}
