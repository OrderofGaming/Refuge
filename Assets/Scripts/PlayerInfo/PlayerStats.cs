using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
	public void Initialize()
	{
		inventory = new List<Items.ItemContainer>();
		Items ic = GameObject.FindObjectOfType<Items>();

		inventory.Clear();

        governmentID = false;
        inJail = false;
	}

    //Primary Stats
	public int money = 0;
    public int wellbeing = 0;
    public int hygiene = 0;

    public enum sleepQuality
    {
        GOOD,
        NEUTRAL,
        BAD
    };

    public sleepQuality lastNightSleep;

    public CharacterBackground background;

    public List<Items.ItemContainer> inventory;

    //Buff/Debuff Flags
    public bool hasHome = false;
    public bool attendedSchool = false;
    public bool onTimeSchool = false;

    public bool isEmployed = false;
    public bool wentToWork = false;
    public bool onTimeWork = false;

    public bool governmentID;
    public bool inJail;

}

