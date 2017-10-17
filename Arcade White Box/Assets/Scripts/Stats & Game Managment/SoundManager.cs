using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    
    public AudioClip[] menuAudioClip;    
    public AudioClip[] inGameAudio;

    private AudioSource gameAudioSource;

    void Start()
    {
    }
    
    public void MenuAudio()
    {
        gameAudioSource.clip = menuAudioClip[Random.Range(0, menuAudioClip.Length)];
        gameAudioSource.PlayDelayed(0.0f);
    }

    public void InGameAudio()
    {
        gameAudioSource.clip = inGameAudio[Random.Range(0, inGameAudio.Length)];
        gameAudioSource.PlayDelayed(0.0f);
    }

    public void setAudioSource(AudioSource newAudioSource)
    {
        gameAudioSource = newAudioSource;
    }

    
}
