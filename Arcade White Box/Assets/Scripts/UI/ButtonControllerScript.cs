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

    public void startTimer()
    {
        timeAndCalendarLink.StartTimer();
    }

    public void doubleSpeed()
    {
        timeAndCalendarLink.StartTimerX2();
    }

    public void tripleSpeed()
    {
        timeAndCalendarLink.StartTimerX3();
    }

    public void maxSpeed()
    {
        timeAndCalendarLink.StartTimerX10();
    }
}
