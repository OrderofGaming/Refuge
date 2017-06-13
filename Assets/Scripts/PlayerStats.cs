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
		missedTurns = 0;
        inJail = false;
	}

    // Primary Stats
	public int money = 0;
    public int wellbeing = 0;
    public int hygiene = 0;

    public CharacterBackground background;

    public List<Items.ItemContainer> inventory;

	public bool governmentID;
    public bool inJail;
    public int missedTurns;

}

