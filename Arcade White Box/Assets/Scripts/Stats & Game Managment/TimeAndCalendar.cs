using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAndCalendar : MonoBehaviour {

    public int startHour, startMinute;
    public int startYear, startMonth, startDay;

    [SerializeField] private List<string> dayNames, monthNames;

    private Text timeText, dateText;

    private int currentHour, currentMinute;
    private int currentYear, currentMonth, currentDay;


    public int CurrentHour
    {
        get
        {
            return currentHour;
        }

        set
        {
            currentHour = value;
        }
    }

    public int CurrentMinute
    {
        get
        {
            return currentMinute;
        }

        set
        {
            currentMinute = value;
        }
    }

    // Use this for initialization
    void Start () {
        CurrentHour = startHour;
        CurrentMinute = startMinute;
        currentYear = startYear;
        currentMonth = startMonth;
        currentDay = startDay;

        timeText = GameObject.Find("UI Canvas/Top Banner/Time").GetComponent<Text>();

        StartTimer();
	}
	
	// Update is called once per frame
	void Update () {

        UpdateTime();
        // UpdateDate();

    }

    private void MinuteIncrement()
    {
        if (CurrentMinute == 59)
        {
            CurrentMinute = 0;
            HourIncrement();
        }
        else
        {
            CurrentMinute++;
        }
    }

    private void HourIncrement()
    {
        if (CurrentHour == 23)
        {
            CurrentHour = 0;
            DayIncrement();
        }
        else
        {
            CurrentHour++;
        }
    }

    private void DayIncrement()
    {
        // do daily stuff
        if (currentDay == 29)
        {
            currentDay = 0;
            MonthIncrement();
        }
        else
        {
            currentDay++;
        }
    }

    private void MonthIncrement()
    {
        // do month stuff
        if (currentMonth == 11)
        {
            currentMonth = 0;
            YearIncrement();
        }

        else
        {
            currentMonth++;
        }
    }

    private void YearIncrement()
    {
        //do year stuff
        currentYear++;
    }

    public void PrintTime()
    {
        print("Current time is " + CurrentHour + ":" + CurrentMinute + ".");
    }

    public void StartTimer()
    {
        StopTimer();
        InvokeRepeating("MinuteIncrement", 0, 1.0f);
    }

    public void StopTimer()
    {
        CancelInvoke();
    }

    public void StartTimerX2()
    {   
        StopTimer();
        InvokeRepeating("MinuteIncrement", 0, 0.5f);
    }

    public void StartTimerX3()
    {
        StopTimer();
        InvokeRepeating("MinuteIncrement", 0, 0.33f);
    }

    public void StartTimerX10()
    {
        StopTimer();
        InvokeRepeating("MinuteIncrement", 0, 0.01f);
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    private void UpdateTime()
    {
        string min = LeadingZero(CurrentMinute);
        string hour = LeadingZero(CurrentHour);
        timeText.text = ("" + hour + ":" + min + "");
    }

    private void UpdateDate()
    {
        string day = LeadingZero(currentDay);
        string month = LeadingZero(currentMonth);
        dateText.text = ("" + day + "/" + month + "/" + currentYear + "");
    }
}
