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

}

[System.Serializable]
public class ShopInterface
{
	public GameObject container;

    public Button govID;

    public Button food;
    public Button hygiene;
    public Button clothing;

    public Button[] items;
}

[System.Serializable]
public class ChanceInterface
{
	public GameObject container;

    public Text title;
    public Text description;
    public Image icon;
    public Button proceed;
}

[System.Serializable]
public class InventoryViewer
{
    [Header("Inventory UI")]
    public Image[] invSprite;
    public Button[] invButtons;
    private int currIndex;
    private ScoreController score;
    public Text[] invText;
    public Button left;
    public Button right;

    public void SetupButtons(ScoreController s)
    {
        left.onClick.AddListener(() => left_onClick());
        right.onClick.AddListener(() => right_onClick());

        score = s;

        UpdateUIElements();
        InvButtons();
    }

    void InvButtons()
    {
        if (invButtons != null) // We need to set up buttons
        {
            for (int i = 0; i < invButtons.Length; i++)
            {
                invButtons[i].onClick.RemoveAllListeners();
                invButtons[i].interactable = false;
                if (score.playerScores[0].inventory.Count > i)
                {
                    invButtons[i].onClick.AddListener(() =>
                    {
                        //score.gameObject.GetComponent<GameController>().ChangeMoney(
                        //    score.playerScores[0].inventory[currIndex + i].value);

                        //score.playerScores.RemoveAt(currIndex + i);
                        //score.gameObject.GetComponent<GameController>()
                        //    .UpdatePlayerInfo();
                    });
                    invButtons[i].interactable = true;
                }
            }
        }
    }

    void left_onClick()
    {
        currIndex -= 1;
        UpdateUIElements();
    }

    void right_onClick()
    {
        currIndex += 1;
        UpdateUIElements();
    }

    public void UpdateUIElements()
    {
        if (currIndex < 0) currIndex = 0;
        if (currIndex > score.playerScores[0].inventory.Count - invSprite.Length)
        {
            currIndex = score.playerScores[0].inventory.Count - invSprite.Length;
        }

        for (int i = 0; i < invSprite.Length; i++)
        {
            if (score.playerScores[0].inventory.Count > i)
            {
                invSprite[i].sprite = score.playerScores[0].inventory[currIndex + i].image;
                invText[i].text = "$" + score.playerScores[0].inventory[currIndex + i].value.ToString();
            }
            else
            {
                invSprite[i].sprite = null;
                invText[i].text = string.Empty;
            }
        }

        InvButtons();
    }
}

[System.Serializable]
public class UIInterface
{
	[Header("Resource Bars")]
	public UpdateLabel hygiene, food, clothing;

    public InventoryViewer[] viewers;

	[Header("Top Bar")]
	public Toggle moneyIcon;
	public Text moneyText;

    public Text roundCounter;
}

public class GameController : MonoBehaviour {

	[System.Serializable]
	public struct PlayerInfo
	{
		public string firstName;
		public int age;

		public CharacterController characterController;
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
    public Button endTurn;
    public Button backButton;

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
	[HideInInspector] public int currentPlayersTurn = 0;

    private int turnCounter = 0;

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

        SetupShop(true);

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

        foreach(InventoryViewer i in mainUI.viewers)
        {
            i.SetupButtons(score);
        }

		UpdatePlayerInfo ();
	}

	public void UpdatePlayerInfo()
	{
        mainUI.hygiene.UpdateValues(score.playerScores[0].hygiene);
        mainUI.food.UpdateValues(score.playerScores[0].food);
        mainUI.clothing.UpdateValues(score.playerScores[0].clothes);

        mainUI.moneyText.text = "$" + score.playerScores [0].money.ToString("N0");

		mainUI.moneyIcon.isOn = score.playerScores[0].money == 0 ? false : true;

        foreach (InventoryViewer i in mainUI.viewers)
        {
            i.UpdateUIElements();
        }
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
        endTurn.onClick.AddListener(() => endTurn_onClick());
        options.trade.onClick.AddListener(() => { backButton.gameObject.SetActive(true); });
        options.shop.onClick.AddListener(() => { backButton.gameObject.SetActive(true); });
        options.chance.onClick.AddListener(() => { backButton.gameObject.SetActive(true); });

        backButton.onClick.AddListener (() => back_onClick ());
	}

	void back_onClick()
	{
		SetContainersActiveFalse (false);
        raycastBlock.SetActive(true);
        options.container.SetActive(true);
        backButton.gameObject.SetActive(false);
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

    void endTurn_onClick()
    {
        turnInProgress = false;
    }

	IEnumerator MainTurn()
	{
		turnInProgress = true;
		SetBackgroundSelected (currentPlayersTurn);
	
		SetContainersActiveFalse (true);

		if (currentPlayersTurn == 0) { // main player
            SetupShop(false);
            turnCounter += 1;
            mainUI.roundCounter.text = "Turn " + turnCounter.ToString();
            raycastBlock.SetActive(true);
			options.container.SetActive (true);
		}

		players[currentPlayersTurn].characterController.LookHappy (1.0f);

        // Update missed turns, leave jail if missed turns are at 0
        if (score.playerScores[currentPlayersTurn].missedTurns <= 0)
        {
            score.playerScores[currentPlayersTurn].inJail = false;
            score.playerScores[currentPlayersTurn].missedTurns = 0;
        }
        else
        {
            // Only happen for the player
            turnInProgress = false;
        }
        score.playerScores[currentPlayersTurn].missedTurns -= 1;

        // Update the UI for in jail
        for (int i = 0; i < players.Length; i++)
        {
            if (score.playerScores[i].inJail)
                players[i].characterController.inJail = true;
            else
                players[i].characterController.inJail = false;

            players[i].characterController.UpdateCharacter();
        }

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
                SetMoney(score.playerScores[0].money); // Update the UI for the main players bank
            } else {
                // AI TURN
                yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));

                switch (Random.Range(0, 2)) // Set to 3 for trade enabled
                {
                    case 0: // Chance
                        AIChance();
                        break;
                    case 1: // Shop
                        AIShop();
                        break;
                    case 3: // Trade
                        break;
                }

				turnInProgress = false;
			}

			yield return new WaitForEndOfFrame ();
		}

        currentPlayersTurn = (currentPlayersTurn + 1) % 4; // 4, or more players I guess
		StartCoroutine(MainTurn());

		yield return null;
	}

    void AIChance()
    {
        var cp = GetComponent<CardProvider>();
        var card = cp.GetCard(); // also calls the function here

        // Show AI turn info here
        Debug.LogFormat("Player {0} took a chance. {1}: {2}",
            players[currentPlayersTurn].firstName,
            card.Title,
            card.Description);
    }

    void AIShop()
    {
        int prevMoney = score.playerScores[currentPlayersTurn].money;

        for (int i = 0; i < 3; i++)
        {
            int thingToBuy = Random.Range(0, shopping.items.Length);
            shopping.items[thingToBuy].onClick.Invoke();
            if (prevMoney != score.playerScores[currentPlayersTurn].money)
            {   // They successfully bought a thing
                break;
            }
        }

        // Show AI turn info here
        Debug.LogFormat("Player {0} chose to shop. They spent ${1}.",
            players[currentPlayersTurn].firstName,
            prevMoney - score.playerScores[currentPlayersTurn].money);

        turnInProgress = false;
    }

    public void ChangeMoney(int amount)
    {
        score.playerScores[currentPlayersTurn].money += amount;
        if (score.playerScores[currentPlayersTurn].money < 0)
            score.playerScores[currentPlayersTurn].money = 0;
        UpdatePlayerInfo();
    }

    public void SetMoney(int amount)
    {
        score.playerScores[currentPlayersTurn].money = amount;
        if (score.playerScores[currentPlayersTurn].money < 0)
            score.playerScores[currentPlayersTurn].money = 0;
        UpdatePlayerInfo();
    }

    void SetupShop(bool allItems = true)
    {
        var ic = GetComponent<Items>(); // ic = Item controller

        if (allItems)
        {
            foreach (Button b in shopping.items)
            {
                Text t = b.GetComponentInChildren<Text>();
                Items.ItemContainer i = ic.items[Random.Range(0, ic.items.Length)];

                t.text = i.title + "\n<color=green>$" + i.value.ToString() + "</color>";
                b.image.sprite = i.image;

                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(() =>
                {
                    int cost = i.value;

                    if (score.playerScores[currentPlayersTurn].money >= cost) // we have the $$$
                    {
                        ChangeMoney(-cost); // Update the Money and MoneyUI
                        // play a sound
                        score.playerScores[currentPlayersTurn].inventory.Add(i);
                        UpdatePlayerInfo();
                    }
                });
            }
        }
        else
        {
            Button b = shopping.items[Random.Range(0, shopping.items.Length)];

            Text t = b.GetComponentInChildren<Text>();
            Items.ItemContainer i = ic.items[Random.Range(0, ic.items.Length)];

            t.text = i.title + "\n<color=green>$" + i.value.ToString() + "</color>";
            b.image.sprite = i.image;

            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() =>
            {
                int cost = i.value;

                if (score.playerScores[currentPlayersTurn].money >= cost) // we have the $$$
                {
                    ChangeMoney(-cost); // Update the Money and MoneyUI
                                        // play a sound
                    score.playerScores[currentPlayersTurn].inventory.Add(i);
                    UpdatePlayerInfo();
                }
            });
        }
    }

    IEnumerator ShopTurn()
	{
        {   // Set up the other buttons
            shopping.clothing.onClick.RemoveAllListeners();
            shopping.food.onClick.RemoveAllListeners();
            shopping.hygiene.onClick.RemoveAllListeners();

            // Add button listeners
            shopping.clothing.onClick.AddListener(() =>
            {
                if (score.playerScores[0].money >= 20) // HARDCODED costs $20
                {
                    ChangeMoney(-20); // Update the Money and MoneyUI
                    score.playerScores[0].clothes += 10;
                    UpdatePlayerInfo();
                }
            });
            shopping.food.onClick.AddListener(() =>
            {
                if (score.playerScores[0].money >= 20) // HARDCODED costs $20
                {
                    ChangeMoney(-20); // Update the Money and MoneyUI
                    score.playerScores[0].food += 10;
                    UpdatePlayerInfo();
                }
            });
            shopping.hygiene.onClick.AddListener(() =>
            {
                if (score.playerScores[0].money >= 20) // HARDCODED costs $20
                {
                    ChangeMoney(-20); // Update the Money and MoneyUI
                    score.playerScores[0].hygiene += 10;
                    UpdatePlayerInfo();
                }
            });
        }

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
        var c = chance;

        var cp = GetComponent<CardProvider>();

        var card = cp.GetCard(); // also calls the function here

        c.icon.sprite = card.Image;
        c.title.text = card.Title;
        c.description.text = card.Description;

        if (card.endsTurn)
            backButton.gameObject.SetActive(false);

        var t = c.proceed.GetComponentInChildren<Text>();
        t.text = (card.endsTurn ? "End Turn" : "Next");
        c.proceed.onClick.AddListener(() => {
            if (!card.endsTurn) back_onClick();
            else chance.container.SetActive(false);
        });

		while (chance.container.activeInHierarchy) {
			// Chance Turn

			yield return new WaitForFixedUpdate ();
		}

        SendMessage(card.Function.ToString(), card.value);

        // Should we end the turn?
        turnInProgress = !card.endsTurn;

        yield return null;
	}
}
