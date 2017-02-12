using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[System.Serializable]
	public struct PlayerInfo
	{
		public string firstName;
		public int age;

		public ClothingController characterController;
		public Image backgroundImage;
		public Text infoLabel;
	}

	// Hardcode 4 players
	[Header("Main player")]
	public PlayerInfo m_player1;
	[Header("Opponent players")]
	public PlayerInfo m_player2;
	public PlayerInfo m_player3;
	public PlayerInfo m_player4;

	[Header("Set of haircolors")]
	public Color[] m_haircolors;

	[Header("Set of skintones")]
	public Color[] m_skintones;

	#region FEMALE NAMES

	string[] femaleNames = {
		"MARY",
		"PATRICIA",
		"LINDA",
		"BARBARA",
		"ELIZABETH",
		"JENNIFER",
		"MARIA",
		"SUSAN",
		"MARGARET",
		"DOROTHY",
		"LISA",
		"NANCY",
		"KAREN",
		"BETTY",
		"HELEN",
		"SANDRA",
		"DONNA",
		"CAROL",
		"RUTH",
		"SHARON",
		"MICHELLE",
		"LAURA",
		"SARAH",
		"KIMBERLY",
		"DEBORAH",
		"JESSICA",
		"SHIRLEY",
		"CYNTHIA",
		"ANGELA",
		"MELISSA"
	};

	#endregion

	#region MALE NAMES

	string[] maleNames = {
		"JAMES",
		"JOHN",
		"ROBERT",
		"MICHAEL",
		"WILLIAM",
		"DAVID",
		"RICHARD",
		"CHARLES",
		"JOSEPH",
		"THOMAS",
		"CHRISTOPHER",
		"DANIEL",
		"PAUL"	,
		"MARK"	,
		"DONALD",	
		"GEORGE",	
		"KENNETH",	
		"STEVEN",
		"EDWARD",	
		"BRIAN"	,
		"RONALD",	
		"ANTHONY",	
		"KEVIN",
		"JASON"	,
		"MATTHEW",	
		"GARY",
		"TIMOTHY",	
		"JOSE",
		"LARRY"	,
		"JEFFREY"
	};

	#endregion

	void Start ()
	{
		// Randomize all 4 players
		RandomizePlayer (m_player1);
		RandomizePlayer (m_player2);
		RandomizePlayer (m_player3);
		RandomizePlayer (m_player4);
	}

	void RandomizePlayer(PlayerInfo a_player)
	{
		// Male or female?
		a_player.characterController.isMale = Random.Range (0, 2) == 0 ? true : false;

		if (a_player.characterController.isMale) {
			a_player.firstName = maleNames [Random.Range (0, maleNames.Length)];
		} else {
			a_player.firstName = femaleNames [Random.Range (0, femaleNames.Length)];
		}
		// Not sure which numbers to use
		a_player.age = Random.Range (16, 23);

		int fontSize = a_player.infoLabel.fontSize;

		// Add the info
		a_player.infoLabel.text = a_player.firstName +
			"\n<size=" + (fontSize / 2).ToString() +">" + (a_player.characterController.isMale ? "Male, " : "Female, ") +
		a_player.age.ToString () + "</size>";
		
		// Pick a random hairstyle
		a_player.characterController.hairdo = Random.Range (1, 7);

		// A random shirt color
		a_player.characterController.shirtColor = Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.2f, 1.0f, 1.0f, 1.0f);
		// Pick a random haircolor
		a_player.characterController.hairColor = m_haircolors [Random.Range (0, m_haircolors.Length)];

		// Pick a random skintone
		a_player.characterController.skinTone = m_skintones [Random.Range (0, m_skintones.Length)];

		// Pick a random sleeve length
		a_player.characterController.sleeveLength = Random.Range(0, 4);

		// Start smiling!
		a_player.characterController.smile = 3;

		// Update everyone
		a_player.characterController.UpdateCharacter ();
	}
}
