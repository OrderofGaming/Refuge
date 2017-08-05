using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour {

	[System.Serializable]
	public class ItemContainer
	{
		public string title;
		[Multiline]
		public string description;
		public int value;
		public Sprite image;
	}

	[Header("Every item in game")]
	public ItemContainer[] items;
}