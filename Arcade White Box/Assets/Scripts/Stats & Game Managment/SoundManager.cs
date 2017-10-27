using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    
    public AudioClip[] menuAudio;    
    public AudioClip[] inGameAudio;
    
    private AudioSource gameAudioSource;
    private AudioSource menuAudioSource;

    private int trackNumber = 0;
    private bool doneOnce = false;

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
        menuAudioSource.clip = menuAudio[Random.Range(0, menuAudio.Length)];
        menuAudioSource.Play();
    }

    public void InGameAudio()
    {     

        if (!gameAudioSource.isPlaying && !isPaused && trackNumber < inGameAudio.Length)
        {
            gameAudioSource.clip = inGameAudio[trackNumber];
            gameAudioSource.Play();
            currentSongName = gameAudioSource.clip.name;
            if (doneOnce)
            {
                GameManager.Instance.GetComponent<EventManager>().SongSwitch();
            }
            doneOnce = true;
            trackNumber++;
        }

        if (trackNumber == inGameAudio.Length)
        {
            trackNumber = 0;
        }
    }  

    public void NextSong()
    {
        gameAudioSource.clip = inGameAudio[0];
    }

    public void setMenuAudioSource(AudioSource newMenuAudioSource)
    {
        menuAudioSource = newMenuAudioSource;
    }

    public void setGameAudioSource(AudioSource newGameAudioSource)
    {
        gameAudioSource = newGameAudioSource;
    }


}
