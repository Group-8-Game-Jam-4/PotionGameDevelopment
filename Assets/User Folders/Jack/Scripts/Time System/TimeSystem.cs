using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public float daysPassed;
    public float hoursPassed;
    public float minutesPassed;
    public int timeIncrement;

    private void Start()
    {
        InvokeRepeating("TimeIncrement", 0f, 1f);
    }

    void TimeIncrement()
    {
        minutesPassed += timeIncrement;

        if (minutesPassed >= 60)
        {
            hoursPassed += 1;
            minutesPassed = 0;
            HourHasPassed();
        }

        if (hoursPassed >= 24)
        {
            daysPassed += 1;
            hoursPassed = 0;
            DayHasPassed();
        }
    }

    void HourHasPassed()
    {
        Debug.Log("hour has passed");
    }

    void DayHasPassed()
    {
        Debug.Log("day has passed");
    }
}
