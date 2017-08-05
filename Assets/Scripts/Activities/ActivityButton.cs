using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityButton : MonoBehaviour
{

    public ActivityUtility.Activity activityData { get; protected set; }
    public Text label;
    public GameController game;

    public void SetActivityData(ActivityUtility.Activity newData)
    {
        activityData = newData;
        label.text = newData.name;
    }

    public void DoActivity()
    {
        game.time.AdvanceTime(0, activityData.timeInMinutes);
        game.player.c.stats.wellbeing += activityData.wellbeingChange;
        game.player.c.stats.hygiene += activityData.hygieneChange;
    }
}
