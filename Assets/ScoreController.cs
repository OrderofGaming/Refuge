using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {
	public class ScoreInfo
	{
		public void Initialize()
		{
			Items ic = GameObject.FindObjectOfType<Items> ();

			inventory.Clear ();

			for (int i = 0; i < 3; i++) {
				inventory.Add (ic.items [Random.Range (0, ic.items.Length)]);
			}
		}

		public int money;
		public List<Items.ItemContainer> inventory;

		public bool governmentID = false;
		public int jailTime = 0;

		public int food = 0;
		public int hygiene = 0;
		public int clothes = 0;
	}

	public ScoreInfo[] playerScores;
}
