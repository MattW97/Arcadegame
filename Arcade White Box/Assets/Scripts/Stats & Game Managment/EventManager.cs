using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public delegate void SongNameNotification();
    public static event SongNameNotification SongSwitched;

    public void SongSwitch()
    {
        SongSwitched();
    }

}
