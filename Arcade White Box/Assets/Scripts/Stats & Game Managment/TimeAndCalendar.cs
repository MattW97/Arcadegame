using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAndCalendar : MonoBehaviour {

    public enum DayNames { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
    public int startHour, startMinute;
    public int startYear, startMonth, startDay;
    public float timeMultiplier;

    [SerializeField] private float speedOption1, speedOption2, speedOption3, speedOption4, speedOption5;


    public Day starterDay = new Day();
    private List<Day> listOfDays;
    private Text timeText, dateText;
    private int currentHour, currentMinute;
    private int currentYear, currentMonth, currentDay;
    private List<int> monthLengths;
    private List<string> monthNames;

    private float secondsMultiplier;
    private float seconds;

    private LevelManager _levelManagerLink;
    void Start ()
    {
        CurrentHour = startHour;
        CurrentMinute = startMinute;
        CurrentYear = startYear;
        CurrentMonth = startMonth;
        CurrentDay = startDay;
        seconds = 0;
        _levelManagerLink = this.gameObject.GetComponent<LevelManager>();
        timeText = GameObject.Find("UIInGame/Bottom Bar/Date And Time/Text").GetComponent<Text>();
        StartTimer();
        CreateCalendar();

    }


    // Update is called once per frame
    void Update () {
        


        UpdateTime();

        Seconds();

    }

    #region Time

    public int GetCurrentTime()
    {
        int time = 0;
        time = int.Parse(currentHour.ToString() + CurrentMinute.ToString());
        return time;
    }

    private void Seconds()
    {
        seconds += (Time.deltaTime * 60 * secondsMultiplier);
        if (seconds >= 60)
        {
            seconds = 0;
            MinuteIncrement();
        }
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
        _levelManagerLink.MachineDailyReset();
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

    private void SetTimeMultiplier(float option)
    {
        timeMultiplier = option;
        secondsMultiplier = option;
    }

    public void StartTimer()
    {
        StopTimer();
        SetTimeMultiplier(speedOption1);
    }

    public void StopTimer()
    {
        //CancelInvoke();
        secondsMultiplier = 0;
        timeMultiplier = 0;
    }

    public void StartTimerX2()
    {   
        StopTimer();
        SetTimeMultiplier(speedOption2);
    }

    public void StartTimerX3()
    {
        StopTimer();
        SetTimeMultiplier(speedOption3);
    }

    public void StartTimerX10()
    {
        StopTimer();
        SetTimeMultiplier(speedOption4);
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

    private Day prevDay;

    /// <summary>
    /// Starter method for creating the calendar. 
    /// </summary>
    private void CreateCalendar()
    {
        CalendarInit();
        prevDay = starterDay;
        Calendar calendar = new Calendar();
        calendar.Start();
        calendar.calendarYears.Add(CreateYear(prevDay.dateYear));
        calendar.CurrentDay = prevDay;
        calendar.CurrentYear = calendar.calendarYears[0];
        calendar.CurrentMonth = calendar.CurrentYear.monthsInYear[1];
    }

    /// <summary>
    /// Creates a year's worth of months (12). 
    /// @param the year number, basically increase by one on each call.
    /// </summary>
    /// <param name="yearNumber"></param>
    private Year CreateYear(int yearNumber)
    {
        Year newYear = new Year();
        newYear.yearNumber = yearNumber;
        for (int i = 0; i < 12; i++)
        {
           newYear.monthsInYear.Add(CreateMonth(monthLengths[i], i, yearNumber));  
        }
        return newYear;
    }

    /// <summary>
    /// Creates a month worth of days (Either, 28, 30, or 31).
    /// This is handled by CreateYear(). Do not call this method anywhere else.
    /// @param noOfDays - Number of days to be created and attached to this month.
    /// @param curMonthINT - Which number month this is, in the current year. Eg 2 = February.
    /// @param curYearINT - Which year this month belongs to.
    /// @return An instantiation of the Month class that this function creates.
    /// </summary>
    /// <param name="noOfDays"></param>
    /// <param name="curMonthINT"></param>
    /// <param name="curYearINT"></param>
    /// <returns></returns>
    private Month CreateMonth(int noOfDays, int curMonthINT, int curYearINT)
    {
        Month newMonth = new Month();
        newMonth.monthName = monthNames[curMonthINT];
        for (int i = 0; i < noOfDays; i++)
        {
            prevDay = CreateDay(prevDay, newMonth.monthName, i, curMonthINT, curYearINT);
            newMonth.daysInMonth.Add(prevDay);
        }
        return newMonth;
    }

/// <summary>
/// Creates a singular Day.
/// This is handled by CreateMonth(). Should have no reason to call this method on its own.
/// @param previousDay - The previous day that has been created. Used to update values of the day each call will create.
/// @param curMonthSTR - The month this day is attached to as a string. Done this way so we aren't unnecessarily trolling through a list.
/// @param curDayINT - The day integer this day should be. NOTE: This value has been increased by 1, because the value originally draws from a list which 
/// obviously starts at 0. As there isn't a "0ist" day, they have been increased by 1.
/// @param curMonthINT - The month integer this day should belong to. NOTE: Read note above, also applies.
/// @param curYearINT - The year integer this day should belong to.
/// @return An instantiation of the Day class that has been created.
/// </summary>
/// <param name="previousDay"></param>
/// <param name="curMonthSTR"></param>
/// <param name="curDayINT"></param>
/// <param name="curMonthINT"></param>
/// <param name="curYearINT"></param>
/// <returns></returns>
    private Day CreateDay(Day previousDay, string curMonthSTR, int curDayINT, int curMonthINT, int curYearINT)
    {
        Day newDay = new Day();
        newDay.dateDay = curDayINT += 1;
        newDay.dateMonth = curMonthINT += 1;
        newDay.dateYear = curYearINT;
        newDay.monthName = curMonthSTR;
        newDay.id = CreateDayID(newDay);
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
        listOfDays.Add(newDay);
        return newDay;
    }

    /// <summary>
    /// A method to create a unique ID for each day.
    /// This is done byy concatenating an int in the format YYYY/MM/DD
    /// @param the Day to create an ID for
    /// @return the ID
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    private int CreateDayID(Day day)
    {
        int id = 0;
        id = int.Parse(day.dateYear.ToString() + day.dateMonth.ToString() + day.dateDay.ToString());
        return id;
    }

    /// <summary>
    /// Initialisation for Calendar
    /// </summary>
    private void CalendarInit()
    {
        monthLengths = new List<int>();
        monthNames = new List<string>();
        listOfDays = new List<Day>();

        monthLengths.Add(31);
        monthLengths.Add(28);
        monthLengths.Add(31);
        monthLengths.Add(30);
        monthLengths.Add(31);
        monthLengths.Add(30);
        monthLengths.Add(31);
        monthLengths.Add(31);
        monthLengths.Add(30);
        monthLengths.Add(31);
        monthLengths.Add(30);
        monthLengths.Add(31);

        monthNames.Add("January");
        monthNames.Add("February");
        monthNames.Add("March");
        monthNames.Add("April");
        monthNames.Add("May");
        monthNames.Add("June");
        monthNames.Add("July");
        monthNames.Add("August");
        monthNames.Add("September");
        monthNames.Add("October");
        monthNames.Add("November");
        monthNames.Add("December");
    }


    #endregion Calendar

    #region Getters/Setters
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
    #endregion Getters/Setters

}

[Serializable]
public class Day
{
    public TimeAndCalendar.DayNames dayName;
    public string monthName;
    public int dateDay, dateMonth, dateYear, id;
}

[Serializable]
public class Month
{
    public string monthName;
    public List<Day> daysInMonth = new List<Day>();
}

[Serializable]
public class Year
{
    public int yearNumber;
    public List<Month> monthsInYear = new List<Month>();
}

[Serializable]
public class Calendar
{
    public List<Year> calendarYears;
    private Day currentDay;
    private Month currentMonth;
    private Year currentYear;

   public void Start()
    {
        calendarYears = new List<Year>();
    }

    public Day CurrentDay
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

    public Month CurrentMonth
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

    public Year CurrentYear
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
}


