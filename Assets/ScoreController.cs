using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {
	[System.Serializable]
	public class ScoreInfo
	{
		public void Initialize()
		{
			inventory = new List<Items.ItemContainer> ();
			Items ic = GameObject.FindObjectOfType<Items> ();

			inventory.Clear ();

			// Start with 3, cheap, unique items
			for (int i = 0; i < 3; i++) {
				var temp = ic.items [Random.Range (0, ic.items.Length)];
				while (temp.value > 20 || inventory.Contains(temp)) {
					temp = ic.items [Random.Range (0, ic.items.Length)];
				}
				inventory.Add (temp);
			}

			money = 10;
			governmentID = false;
			jailTime = 0;
			food = 5;
			hygiene = 5;
			clothes = 5;
		}

		public int money;
		public List<Items.ItemContainer> inventory;

		public bool governmentID;
		public int jailTime;

		public int food;
		public int hygiene;
		public int clothes;
	}

	public List<ScoreInfo> playerScores;
}