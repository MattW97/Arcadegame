using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public delegate void SongNameNotification();
    public static event SongNameNotification SongSwitched;

    public delegate void SaveNotification();
    public static event SaveNotification Save;

    public void SongSwitch()
    {
        SongSwitched();
    }

    public void SaveObject()
    {
        Save();
    }

    void Update()
    {
    }

}
