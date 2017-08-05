using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUtility : MonoBehaviour
{
    public int wakeUpTime = 7;
    public int tickSize = 5;
    public int tickCounter = 0;

    void Start ()
    {
		
	}

    public void AdvanceTime(int hours, int minutes = 0)
    {
        tickCounter += (int)Mathf.Round((float)(hours * 60 + minutes) / tickSize);
    }

    public void SetTime(int hours, int minutes = 0)
    {
        tickCounter = 0;
        AdvanceTime(hours, minutes);
    }

    public void SetTime(float hours)
    {
        tickCounter = (int)(hours * (60 / tickSize));
    }

    public int GetHour()
    {
        return tickCounter / (60 / tickSize);
    }

    public int GetMins()
    {
        return (tickCounter % (60 / tickSize)) * tickSize;
    }

    public bool IsBefore(int hour, int minutes = 0)
    {
        return (float)tickCounter < (float)(hour * (60 / tickSize) + (float)minutes / tickSize);
    }
}
