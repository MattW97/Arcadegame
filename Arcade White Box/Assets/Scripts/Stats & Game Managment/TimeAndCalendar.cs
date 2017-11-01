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

    public int CurrentYear
    {
        get
        {
            return currentYear;
        }

        set
        {
            currentYear = value;
        }
    }

    public int CurrentMonth
    {
        get
        {
            return currentMonth;
        }

        set
        {
            currentMonth = value;
        }
    }

    public int CurrentDay
    {
        get
        {
            return currentDay;
        }

        set
        {
            currentDay = value;
        }
    }

    // Use this for initialization
    void Start () {
        CurrentHour = startHour;
        CurrentMinute = startMinute;
        CurrentYear = startYear;
        CurrentMonth = startMonth;
        CurrentDay = startDay;

        timeText = GameObject.Find("UI Canvas/New/Bottom Bar/Date And Time/Text").GetComponent<Text>();

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
        if (CurrentDay == 29)
        {
            CurrentDay = 0;
            MonthIncrement();
        }
        else
        {
            CurrentDay++;
        }
    }

    private void MonthIncrement()
    {
        // do month stuff
        if (CurrentMonth == 11)
        {
            CurrentMonth = 0;
            YearIncrement();
        }

        else
        {
            CurrentMonth++;
        }
    }

    private void YearIncrement()
    {
        //do year stuff
        CurrentYear++;
        this.gameObject.GetComponent<PlayerManager>().BeenBankrupt = false;
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
        string day = LeadingZero(CurrentDay);
        string month = LeadingZero(CurrentMonth);
        dateText.text = ("" + day + "/" + month + "/" + CurrentYear + "");
    }
}
