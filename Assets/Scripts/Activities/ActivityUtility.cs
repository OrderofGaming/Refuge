using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityUtility : MonoBehaviour
{
    public enum ActivityType
    {
        HOMEWORK,
        HANGOUT,
        HANGOUT_ALONE,
        SLEEP,
        SHOPPING,
        MUSIC,
        WORKOUT,
        ART
    };

    [System.Serializable]
    public struct Activity
    {
        public string name;
        public ActivityType typeOfActivity;
        public int timeInHours;
        public int timeInMinutes;
        public int wellbeingChange;
        public int hygieneChange;
    };

    public List<Activity> listOfActivities;

	// Use this for initialization
	void Start ()
    {
		
	}
	
}
