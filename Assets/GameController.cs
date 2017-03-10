using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Options
{
	public GameObject container;
	public Button trade, shop, chance;
}

[System.Serializable]
public class TradeInterface
{
	public GameObject container;
	public Button back;

}

[System.Serializable]
public class ShopInterface
{
	public GameObject container;
	public Button back;

}

[System.Serializable]
public class ChanceInterface
{
	public GameObject container;
	public Button back;

}

[System.Serializable]
public class UIInterface
{
	[Header("Resource Bars")]
	public UpdateLabel hygiene, food, clothing;

	[Header("Inventory UI")]
	public Image[] invSprite;
	public Text[] invText;

	[Header("Top Bar")]
	public Toggle moneyIcon;
	public Text moneyText;
}

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

	[Header("Interfaces")]
	public GameObject raycastBlock;

	public UIInterface mainUI;

	public Options options;
	public TradeInterface trading;
	public ShopInterface shopping;
	public ChanceInterface chance;

	public ScoreController score;

	// Hardcode 4 players
	[Header("Main player")]
	public PlayerInfo m_player1;
	[Header("Opponent players")]
	public PlayerInfo m_player2;
	public PlayerInfo m_player3;
	public PlayerInfo m_player4;

	private PlayerInfo[] players;

	// This boolean dictates when the turn is over
	private bool turnInProgress;

	[Header("Set of haircolors")]
	public Color[] m_haircolors;

	[Header("Set of skintones")]
	public Color[] m_skintones;

	// Control who's turn it is
	private int currentPlayersTurn = 0;

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

		players = new PlayerInfo[4];

		players [0] = m_player1;
		players [1] = m_player2;
		players [2] = m_player3;
		players [3] = m_player4;

		InitializePlayerInventories ();

		LinkFunctions ();

		StartCoroutine (MainTurn ());
	}

	void RandomizePlayer(PlayerInfo a_player)
	{
		// Male or female?
		a_player.characterController.isMale = Random.Range (0, 2) == 0 ? true : false;

		if (a_player.characterController.isMale) {
			a_player.firstName = maleNames [Random.Range (0, maleNames.Length)] + " " + ((char)(65 + Random.Range(0, 26))).ToString() + ".";
		} else {
			a_player.firstName = femaleNames [Random.Range (0, femaleNames.Length)] + " " + ((char)(65 + Random.Range(0, 26))).ToString() + ".";
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

	void InitializePlayerInventories()
	{
		score.playerScores = new List<ScoreController.ScoreInfo> ();

		for (int i = 0; i < 4; i++) {
			score.playerScores.Add (new ScoreController.ScoreInfo ());
			score.playerScores [i].Initialize ();
		}

		UpdatePlayerInfo ();
	}

	public void UpdatePlayerInfo()
	{
		mainUI.invSprite [0].sprite = score.playerScores [0].inventory [0].image;
		mainUI.invSprite [1].sprite = score.playerScores [0].inventory [1].image;
		mainUI.invSprite [2].sprite = score.playerScores [0].inventory [2].image;


		mainUI.invText[0].text = "$" + score.playerScores [0].inventory [0].value.ToString();
		mainUI.invText[1].text = "$" + score.playerScores [0].inventory [1].value.ToString();
		mainUI.invText[2].text = "$" + score.playerScores [0].inventory [2].value.ToString();

		mainUI.moneyText.text = "$" + score.playerScores [0].money.ToString("N0");

		mainUI.moneyIcon.isOn = score.playerScores[0].money == 0 ? false : true;
	}

	void SetContainersActiveFalse(bool raycastActive)
	{
		raycastBlock.SetActive (raycastActive);
		trading.container.SetActive (false);
		shopping.container.SetActive (false);
		chance.container.SetActive (false);
		options.container.SetActive (false);
	}

	void SetBackgroundSelected(int a_selected = 0)
	{
		Color colorToSet; ColorUtility.TryParseHtmlString ("#BFDFFFFF", out colorToSet);
		for (int i = 0; i < 4; i++) {
			if (i != a_selected)
				players [i].backgroundImage.color = Color.black;
			else
				players [i].backgroundImage.color = colorToSet;
		}
	}

	void LinkFunctions()
	{
		options.chance.onClick.AddListener (() => chance_onClick ());
		options.trade.onClick.AddListener (() => trade_onClick ());
		options.shop.onClick.AddListener (() => shop_onClick ());

		//trading.back.onClick.AddListener (() => back_onClick ());
		//shopping.back.onClick.AddListener (() => back_onClick ());
	}

	void back_onClick()
	{
		SetContainersActiveFalse (false);
	}

	void chance_onClick()
	{
		SetContainersActiveFalse (true);
		chance.container.SetActive (true);
	}

	void trade_onClick()
	{
		SetContainersActiveFalse (true);
		trading.container.SetActive (true);
	}

	void shop_onClick()
	{
		SetContainersActiveFalse (true);
		shopping.container.SetActive (true);
	}

	IEnumerator MainTurn()
	{
		turnInProgress = true;
		SetBackgroundSelected (currentPlayersTurn);
	
		SetContainersActiveFalse (true);

		if (currentPlayersTurn == 0) {
			options.container.SetActive (true);
		}

		//players[currentPlayersTurn].characterController.LookHappy (1.0f);

		while (turnInProgress) {
			// Game loop here

			if (currentPlayersTurn == 0) {
				// PLAYER TURN
				if (shopping.container.activeInHierarchy) {
					yield return StartCoroutine (ShopTurn ());
				} else if (trading.container.activeInHierarchy) {
					yield return StartCoroutine (TradeTurn ());
				} else if (chance.container.activeInHierarchy) {
					yield return StartCoroutine (ChanceTurn ());
				}
			} else {
				// AI TURN
				yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));

				turnInProgress = false;
			}

			yield return new WaitForEndOfFrame ();
		}

		currentPlayersTurn = (currentPlayersTurn + 1) % 4; // 4, or more players I guess
		StartCoroutine(MainTurn());

		Debug.Log ("Ended Main Turn Coroutine");

		yield return null;
	}

	IEnumerator ShopTurn()
	{
		while (shopping.container.activeInHierarchy) {
			// Shop Turn

			yield return new WaitForFixedUpdate ();
		}

		yield return null;
	}

	IEnumerator TradeTurn()
	{
		while (trading.container.activeInHierarchy) {
			// Trade Turn

			yield return new WaitForFixedUpdate ();
		}

		yield return null;
	}

	IEnumerator ChanceTurn()
	{
		while (chance.container.activeInHierarchy) {
			// Chance Turn

			yield return new WaitForFixedUpdate ();
		}

		yield return null;
	}
}
