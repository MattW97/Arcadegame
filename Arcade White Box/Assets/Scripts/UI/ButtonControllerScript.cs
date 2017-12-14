using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerScript : MonoBehaviour {

    private TimeAndCalendar timeAndCalendarLink;

    // Use this for initialization
    void Start()
    {
        timeAndCalendarLink = GameManager.Instance.ScriptHolderLink.GetComponent<TimeAndCalendar>();
    }

    public void pauseTimer()
    {
        timeAndCalendarLink.StopTimer();
    }

    public void BaseSpeed()
    {
        timeAndCalendarLink.StartTimer();
    }

    public void SecondSpeedOption()
    {
        timeAndCalendarLink.StartTimerX2();
    }

    public void ThirdSpeedOption()
    {
        timeAndCalendarLink.StartTimerX3();
    }

    public void MaxSpeexOption()
    {
        timeAndCalendarLink.StartTimerX10();
    }
}
