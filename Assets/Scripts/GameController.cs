using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameMenu
{
	public GameObject container;
	public Button chance, endTurn;
}

[System.Serializable]
public class TradeInterface
{
    public GameObject container;
}

[System.Serializable]
public class MenuInterface
{
    public GameObject container;
    public Button newGame, loadGame, exitGame;
    public Text titleText;
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
public class UIInterface
{
	public UpdateLabel hygiene, wellbeing;

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

		public Character c;
		public Image backgroundImage;
		public Text infoLabel;
	}

	[Header("Interfaces")]
	public GameObject raycastBlock;
    public GameObject allContainers;
    public GameObject interfaceButtons;

	public UIInterface mainUI;

	public GameMenu gameMenu;
	public ChanceInterface chance;
    public MenuInterface startMenu;


    public Button backButton;
    public Button moneyButton;

    public GameObject endScreen;
    public Text goodEnding;
    public Text neutralEnding;
    public Text badEnding;

	[Header("Main player")]
	public PlayerInfo player;

	// This boolean dictates when the turn is over
	private bool turnInProgress;
    private bool endGame = false;

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

	void Start()
    { 
        SetupGame();
	}
    
    public void SetupGame()
    {
        SetContainersActive();
        SetContainersInactive();
        startMenu.container.SetActive(true);
        RandomizePlayer(player);
    }

	void RandomizePlayer(PlayerInfo a_player)
	{
		// Male or female?
		a_player.c.isMale = Random.Range (0, 2) == 0 ? true : false;

		if (a_player.c.isMale) {
			a_player.firstName = maleNames [Random.Range (0, maleNames.Length)] + " " + ((char)(65 + Random.Range(0, 26))).ToString() + ".";
		} else {
			a_player.firstName = femaleNames [Random.Range (0, femaleNames.Length)] + " " + ((char)(65 + Random.Range(0, 26))).ToString() + ".";
		}
		// Not sure which numbers to use
		a_player.age = Random.Range (16, 23);

		int fontSize = a_player.infoLabel.fontSize;

		// Add the info
		a_player.infoLabel.text = a_player.firstName +
			"\n<size=" + (fontSize / 2).ToString() +">" + (a_player.c.isMale ? "Male, " : "Female, ") +
		a_player.age.ToString () + "</size>";
		
		// Pick a random hairstyle
		a_player.c.hairdo = Random.Range (1, 7);

		// A random shirt color
		a_player.c.shirtColor = Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.2f, 1.0f, 1.0f, 1.0f);
		// Pick a random haircolor
		a_player.c.hairColor = m_haircolors [Random.Range (0, m_haircolors.Length)];

		// Pick a random skintone
		a_player.c.skinTone = m_skintones [Random.Range (0, m_skintones.Length)];

		// Pick a random sleeve length
		a_player.c.sleeveLength = Random.Range(0, 4);

		// Start smiling!
		a_player.c.smile = 3;

		// Update everyone
		a_player.c.UpdateCharacter ();
	}

	public void UpdatePlayerInfo()
	{
        mainUI.hygiene.UpdateValues((int)(player.c.stats.hygiene));
        mainUI.wellbeing.UpdateValues((int)(player.c.stats.wellbeing));

        mainUI.moneyText.text = "$" + player.c.stats.money.ToString("N0");

		mainUI.moneyIcon.isOn = player.c.stats.money == 0 ? false : true;
	}

    public void SetContainersInactive()
    {
        foreach (Transform child in allContainers.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in interfaceButtons.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void SetContainersActive()
    {
        foreach (Transform child in allContainers.transform)
        {
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in interfaceButtons.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void ActivateUI()
    {
        interfaceButtons.SetActive(true);
        foreach (Transform child in interfaceButtons.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
	public void back_onClick()
	{
		SetContainersInactive();
        raycastBlock.SetActive(true);
        backButton.gameObject.SetActive(false);
	}

	public void chance_onClick()
	{
		SetContainersInactive();
		chance.container.SetActive(true);
            
        ChanceTurn();
	}

    public void newGame_onClick()
	{
		SetContainersInactive();
		gameMenu.container.SetActive(true);
        ActivateUI();
	}

    void endTurn_onClick()
    {
        turnInProgress = false;
    }

    void endGameSequence()
    {
        turnInProgress = false;
        mainUI.roundCounter.text = "Game Over";
        mainUI.moneyIcon.image = null;
        mainUI.moneyText.text = null;
        moneyButton.gameObject.SetActive(false);

        endScreen.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);

        if(player.c.stats.governmentID)
        {
            goodEnding.gameObject.SetActive(true);
        }
        else if (player.c.stats.money >= 40)
        {
            neutralEnding.gameObject.SetActive(true);
        }
        else
        {
            badEnding.gameObject.SetActive(true);
        }
    }

    public void ChangeMoney(int amount)
    {
        player.c.stats.money += amount;
        if (player.c.stats.money < 0)
            player.c.stats.money = 0;
        UpdatePlayerInfo();
    }

    public void SetMoney(int amount)
    {
        player.c.stats.money = amount;
        if (player.c.stats.money < 0)
            player.c.stats.money = 0;
        UpdatePlayerInfo();
    }

    public void SetID(bool hasID)
    { 
        player.c.stats.governmentID = hasID;
    }

    public void EndPlayerTurn()
    {
        //inc turn number
        player.c.stats.hygiene -= 5;
        print("Ended my turn bitches");
    }

    public void AddWellBeing()
    {
        if (player.c.stats.money >= 20)
        {
            ChangeMoney(-20); // Update the Money and MoneyUI
            player.c.stats.wellbeing += 20;
            UpdatePlayerInfo();
        }
    }

    public void AddHygiene()
    {
        if (player.c.stats.money >= 20)
        {
            ChangeMoney(-20); // Update the Money and MoneyUI
            player.c.stats.hygiene += 20;
            UpdatePlayerInfo();
        }
    }

    public void AddID()
    {
        if (player.c.stats.money >= 150)
        {
            ChangeMoney(-150);
            player.c.stats.governmentID = true;
            UpdatePlayerInfo();
        }
    }

	public void ChanceTurn()
	{
        var c = chance;

        var cp = GetComponent<CardProvider>();

        var card = cp.GetCard(); // also calls the function here

        c.icon.sprite = card.Image;
        c.title.text = card.Title;
        c.description.text = card.Description;

        var t = c.proceed.GetComponentInChildren<Text>();
        t.text = (card.endsTurn ? "End Turn" : "Next");
        c.proceed.onClick.AddListener(() => {
            if (!card.endsTurn) back_onClick();
            else chance.container.SetActive(false);
        });

        SendMessage(card.Function.ToString(), card.value);
        card.cardEvent.Invoke();

        UpdatePlayerInfo();
	}
}
