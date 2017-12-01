using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    
    public AudioClip[] menuAudio;    
    public AudioClip[] inGameAudio;
    [SerializeField] private AudioClip oneSecAudio;
    
    private AudioSource gameAudioSource;
    private AudioSource menuAudioSource;

    private int trackNumber = 0;
    private bool doneOnce = false;

    public string currentSongName;
    private float length;
    public bool isPaused = false;


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
       
    }

    public void MenuMusic()
    {
        menuAudioSource.clip = menuAudio[Random.Range(0, menuAudio.Length)];
        menuAudioSource.Play();
    }

    public IEnumerator InGameMusic()
    {
        if (!gameAudioSource.isPlaying && !isPaused)
        {
            if (!doneOnce)
            {
                gameAudioSource.clip = oneSecAudio;
                gameAudioSource.Play();
                doneOnce = true;
                isPaused = true;
                yield return new WaitForSeconds(oneSecAudio.length);
                isPaused = false;
            }
            trackNumber = StartMusic(trackNumber);
            if (GameManager.Instance.SettingManager.GetMusicVolume() != 0)
            {
                GameManager.Instance.GetComponent<EventManager>().SongSwitch();
            }

        }
    }

    private int StartMusic(int oldTrackNumber)
    {
        int trackNum = RandomTrack(oldTrackNumber);
        gameAudioSource.clip = inGameAudio[trackNum];
        currentSongName = gameAudioSource.clip.name;
        gameAudioSource.Play();
        return trackNum;
    }

    private int RandomTrack(int oldTrack)
    {
        int track = Random.Range(0, inGameAudio.Length);
        if (track == oldTrack)
        {
           track = RandomTrack(oldTrack);
        }
        return track; 
    }

    public void setMenuAudioSource(AudioSource newMenuAudioSource)
    {
        menuAudioSource = newMenuAudioSource;
    }

    public void setGameAudioSource(AudioSource newGameAudioSource)
    {
        gameAudioSource = newGameAudioSource;
    }

    public bool DoneOnce
    {
        get
        {
            return doneOnce;
        }

        set
        {
            doneOnce = value;
        }
    }

}
