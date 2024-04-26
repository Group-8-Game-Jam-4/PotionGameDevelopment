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
        //runs time increment every second
        InvokeRepeating("TimeIncrement", 0f, 1f);
    }

    void TimeIncrement()
    {
        //increases minutes passed by whatever time increment is set to (currently 1)
        minutesPassed += timeIncrement;

        //if 60 minutes have passed (60 seconds irl if increment is set to 1)
        if (minutesPassed >= 60)
        {
            //increment hours by 1
            hoursPassed += 1;

            //reset minutes
            minutesPassed = 0;

            //run hours has passed
            HourHasPassed();
        }

        //if 24 hours have passed (24 minutes irl if increment is set to 1 i think)
        if (hoursPassed >= 24)
        {
            //increment days by 1
            daysPassed += 1;

            //reset hours
            hoursPassed = 0;

            //run days has passed
            DayHasPassed();
        }
    }

    void HourHasPassed()
    {
        //put shit here
    }

    void DayHasPassed()
    {
        //put shit here
    }
}
