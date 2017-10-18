using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    
    public AudioClip[] menuAudio;    
    public AudioClip[] inGameAudio;

    //private AudioSource menuAudioSource;
    private AudioSource gameAudioSource;

    private int trackNumber = 0;

    public string currentSongName;

    public float fadeDuration;

    void Start()
    {
    }

    void Update()
    {
        //InGameAudio();
    }
    
    public void MenuAudio()
    {
        gameAudioSource.clip = menuAudio[Random.Range(0, menuAudio.Length)];
        gameAudioSource.Play();
    }

    public void InGameAudio()
    {
        if(!gameAudioSource.isPlaying && trackNumber < inGameAudio.Length)
        {
            gameAudioSource.clip = inGameAudio[trackNumber];
            gameAudioSource.Play();
            StartCoroutine(FadeIn());
            currentSongName = gameAudioSource.clip.ToString();
            print("The current song is " + currentSongName);
            trackNumber++;
        }
        else
        {
            //StartCoroutine(FadeOut());
        }
        if (trackNumber == inGameAudio.Length)
        {
            trackNumber = 0;
        }
        
        //gameAudioSource.PlayDelayed(0.0f);
    }   

    private IEnumerator FadeOut()
    {
        if (fadeDuration > 0f)
        {
            float startTime = gameAudioSource.clip.length - fadeDuration;
            float lerpValue = 0f;
            while (lerpValue < 1f)
            {
                lerpValue = Mathf.InverseLerp(startTime, gameAudioSource.clip.length, gameAudioSource.time);
                gameAudioSource.volume = Mathf.Lerp(gameAudioSource.volume, 0f, lerpValue);
                yield return null;
            }
        }
    }

    private IEnumerator FadeIn()
    {
        if (fadeDuration == 0f)
        {
            print("fade in 1");
            float lerpValue = 0f;
            while (lerpValue < 1f)
            {
                lerpValue = Mathf.InverseLerp(0f, fadeDuration, gameAudioSource.time);
                gameAudioSource.volume = Mathf.Lerp(0f, gameAudioSource.volume, lerpValue);
                print("fade in 2");
                yield return null;
            }
        }
        else
        {
            yield break;
        }
    }

    public void NextSong()
    {
        gameAudioSource.clip = inGameAudio[0];
    }

    public void setAudioSource(AudioSource newAudioSource)
    {
        gameAudioSource = newAudioSource;
    }

    
}
