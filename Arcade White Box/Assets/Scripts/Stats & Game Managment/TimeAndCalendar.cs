using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAndCalendar : MonoBehaviour {

    public enum DayNames { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
    public enum MonthNames { January, February, March, April, May, June, July, August, September, October, November, December }
    public int startHour, startMinute;
    public int startYear, startMonth, startDay;
    public Day starterDay = new Day();
    public int numberOfDaysToCreate;
    public float timeMultiplier;

    public List<Day> listOfDays;

    [SerializeField]
    private float speedOption1, speedOption2, speedOption3, speedOption4, speedOption5;
    private Text timeText, dateText;
    private int currentHour, currentMinute;
    private int currentYear, currentMonth, currentDay;
    private Month january, february, march, april, may, june, july, august, september, october, november, december;


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
        //CreateMonths();
        StartTimer();
       // CreateDay(starterDay);
	}
	
	// Update is called once per frame
	void Update () {

        UpdateTime();
        // UpdateDate();

    }

    #region Time

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

    private void SetInvokeRepeating(float option)
    {
        timeMultiplier = Mathf.Round(1 / option);
        InvokeRepeating("MinuteIncrement", 0, option);
    }

    public void StartTimer()
    {
        StopTimer();
        SetInvokeRepeating(speedOption1);
    }

    public void StopTimer()
    {
        CancelInvoke();
    }

    public void StartTimerX2()
    {   
        StopTimer();
        SetInvokeRepeating(speedOption2);
    }

    public void StartTimerX3()
    {
        StopTimer();
        SetInvokeRepeating(speedOption3);
    }

    public void StartTimerX10()
    {
        StopTimer();
        SetInvokeRepeating(speedOption4);
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

    #endregion Time

    #region Calendar

    private void CreateCalendar()
    {
        
    }

    private void CreateMonths()
    {
        Month january = new Month();
        january.monthName = MonthNames.January;
        january.numberOfDays = 31;

        Month february = new Month();
        february.monthName = MonthNames.February;
        february.numberOfDays = 28;

        Month march = new Month();
        march.monthName = MonthNames.March;
        march.numberOfDays = 31;

        Month april = new Month();
        april.monthName = MonthNames.April;
        april.numberOfDays = 30;

        Month may = new Month();
        may.monthName = MonthNames.May;
        may.numberOfDays = 31;

        Month june = new Month();
        june.monthName = MonthNames.June;
        june.numberOfDays = 30;

        Month july = new Month();
        july.monthName = MonthNames.July;
        july.numberOfDays = 31;

        Month august = new Month();
        august.monthName = MonthNames.August;
        august.numberOfDays = 31;

        Month september = new Month();
        september.monthName = MonthNames.September;
        september.numberOfDays = 30;

        Month october = new Month();
        october.monthName = MonthNames.October;
        october.numberOfDays = 31;

        Month november = new Month();
        november.monthName = MonthNames.November;
        november.numberOfDays = 30;

        Month december = new Month();
        december.monthName = MonthNames.December;
        december.numberOfDays = 31;
    }

    private Day CreateDay(Day previousDay)
    {
        Day newDay = new Day();
        newDay.month = january;
        newDay.id = previousDay.id++;
        switch (previousDay.dayName)
        {
            case DayNames.Monday:
                newDay.dayName = DayNames.Tuesday;
                break;
            case DayNames.Tuesday:
                newDay.dayName = DayNames.Wednesday;
                break;
            case DayNames.Wednesday:
                newDay.dayName = DayNames.Thursday;
                break;
            case DayNames.Thursday:
                newDay.dayName = DayNames.Friday;
                break;
            case DayNames.Friday:
                newDay.dayName = DayNames.Saturday;
                break;
            case DayNames.Saturday:
                newDay.dayName = DayNames.Sunday;
                break;
            case DayNames.Sunday:
                newDay.dayName = DayNames.Monday;
                break;
        }

        if (previousDay.dateDay == previousDay.month.numberOfDays)
        {
            newDay.dateDay = 1;
            switch (previousDay.month.monthName)
            {
                case MonthNames.January:
                    newDay.month.monthName = MonthNames.February;
                    newDay.month = february;
                    newDay.dateMonth = 2;
                    break;
                case MonthNames.February:
                    newDay.month.monthName = MonthNames.March;
                    newDay.month = march;
                    newDay.dateMonth = 3;
                    break;
                case MonthNames.March:
                    newDay.month.monthName = MonthNames.April;
                    newDay.month = april;
                    newDay.dateMonth = 4;
                    break;
                case MonthNames.April:
                    newDay.month.monthName = MonthNames.May;
                    newDay.month = may;
                    newDay.dateMonth = 5;
                    break;
                case MonthNames.May:
                    newDay.month.monthName = MonthNames.June;
                    newDay.month = june;
                    newDay.dateMonth = 6;
                    break;
                case MonthNames.June:
                    newDay.month.monthName = MonthNames.July;
                    newDay.month = july;
                    newDay.dateMonth = 7;
                    break;
                case MonthNames.July:
                    newDay.month.monthName = MonthNames.August;
                    newDay.month = august;
                    newDay.dateMonth = 8;
                    break;
                case MonthNames.August:
                    newDay.month.monthName = MonthNames.September;
                    newDay.month = september;
                    newDay.dateMonth = 9;
                    break;
                case MonthNames.September:
                    newDay.month.monthName = MonthNames.October;
                    newDay.month = october;
                    newDay.dateMonth = 10;
                    break;
                case MonthNames.October:
                    newDay.month.monthName = MonthNames.November;
                    newDay.month = november;
                    newDay.dateMonth = 11;
                    break;
                case MonthNames.November:
                    newDay.month.monthName = MonthNames.December;
                    newDay.month = december;
                    newDay.dateMonth = 12;
                    break;
                case MonthNames.December:
                    newDay.month.monthName = MonthNames.January;
                    newDay.month = february;
                    newDay.dateMonth = 1;
                    newDay.dateYear = previousDay.dateYear++;
                    break;

            }
            

        }
        else
        {
            newDay.dateDay = previousDay.dateDay++;
            newDay.month = previousDay.month;
        }
        print("Created a new day with the ID " + newDay.id);
        listOfDays.Add(newDay);
        if (newDay.id != numberOfDaysToCreate)
        {
            CreateDay(newDay);
        }

        return newDay;
        
    }

    #endregion Calendar
}

[Serializable]
public class Day
{
    public TimeAndCalendar.DayNames dayName;
    public int dateDay, dateMonth, dateYear, id;
    public Month month;
}

[Serializable]
public class Month
{
    public TimeAndCalendar.MonthNames monthName;
    public int numberOfDays;
    
}
