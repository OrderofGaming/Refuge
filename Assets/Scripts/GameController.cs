using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game Menus
[System.Serializable]
public class GameMenu
{
	public GameObject container;
	public Button school, job, applyJob, support;
}

[System.Serializable]
public class StandardMenu
{
    public GameObject container;
    public Button money, stranger, food;
}

[System.Serializable]
public class StartMenu
{
    public GameObject container;
    public Button newGame, loadGame, exitGame;
    public Text titleText;
}

[System.Serializable]
public class SchoolMenu
{
    public GameObject container;
    public Button transit, walkLong, walkShort;
}

[System.Serializable]
public class ApplyJobMenu
{
    public GameObject container;
    public Button coldCall, friend, onlineJob;
}

[System.Serializable]
public class JobMenu
{
    public GameObject container;
    public Button transit, walkLong, walkShort;
}

[System.Serializable]
public class SupportMenu
{
    public GameObject container;
    public Button friends, family, services;
}

//Standard Menus
[System.Serializable]
public class MoneyMenu
{
    public GameObject container;
    public Button steal, panhandle, dayLabour;
}

[System.Serializable]
public class StrangerMenu
{
    public GameObject container;
    public Button pedestrian, homeless, shopOwner;
}

[System.Serializable]
public class FoodMenu
{
    public GameObject container;
    public Button scavenge, fastFood, refuge;
}

//--------------------------------------------------------------//

[System.Serializable]
public class TradeInterface
{
    public GameObject container;
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
	public int hygiene;
    public Text hygieneText;

    public int wellbeing;
    public Text wellbeingText;

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

    private bool inGameMenu = false;
    private bool inStandardMenu = false;

    [Header("Time of Day")]
    public int timeOfDay = 7;
    public int dayTimeStart = 7;
    public Text timeText;
    public Text AM_PM;

    [Header("Turn Counter")]
    public int turnCounter = 1;
    public Text turnText;

    [Header("Interfaces")]
	public GameObject raycastBlock;
    public GameObject allContainers;
    public GameObject interfaceButtons;
    public GameObject gameMenuBack;
    public GameObject standardMenuBack;

	public UIInterface mainUI;

	public GameMenu gameMenu;
    public StandardMenu standardMenu;
    public StartMenu startMenu;

    public SchoolMenu schoolMenu;
    public ApplyJobMenu applyJobMenu;
    public JobMenu jobMenu;
    public SupportMenu supportMenu;

    public MoneyMenu moneyMenu;
    public StrangerMenu strangerMenu;
    public FoodMenu foodMenu;

    public ChanceInterface chance;

    public Button backButton;
    public Button moneyButton;

    public GameObject endScreen;

    [Header("Main player")]
	public PlayerInfo player;

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

	void Start()
    { 
        SetupGame();
	}
    
    public void SetupGame()
    {
        SetContainersActive();
        SetUIActive();
        SetContainersInactive();
        SetUIInactive();

        raycastBlock.SetActive(true);
        showStartMenu();

        RandomizePlayer(player);
    }

    public void ExitGame()
    {
        SetContainersInactive();
        SetUIInactive();

        raycastBlock.SetActive(false);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
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

	}

    public void UpdateUI()
    {
        timeText.text = ((timeOfDay > 12) ? timeOfDay - 12 : timeOfDay).ToString() + ":00";
        AM_PM.text = (timeOfDay >= 12) ? "PM" : "AM";

        mainUI.moneyText.text = "$" + player.c.stats.money.ToString("N0");
        mainUI.moneyIcon.isOn = player.c.stats.money == 0 ? false : true;

        mainUI.hygieneText.text = mainUI.hygiene.ToString();
        mainUI.wellbeingText.text = mainUI.wellbeing.ToString();


        if (timeOfDay >= 24)
        {
            EndPlayerTurn();
        }

    }

    //End Turn functions
    void endTurn_onClick()
    {
        EndPlayerTurn();
    }

    public void EndPlayerTurn()
    {
        turnCounter += 1;
        turnText.text = turnCounter.ToString();
        resetTime();

        SetContainersInactive();
        showGameMenu();

        UpdatePlayerInfo();
        UpdateUI();
    }

    //Game time functions         
    public void resetTime()
    {
        timeOfDay = dayTimeStart;
    }

    public void setTime(int newTime)
    {
        timeOfDay = newTime;
    }

    public int getTime()
    {
        return timeOfDay;
    }

    public void SetContainersInactive()
    {
        foreach (Transform child in allContainers.transform)
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
    }

    public void SetUIInactive()
    {
        interfaceButtons.SetActive(false);
    }

    public void SetUIActive()   
    {
        interfaceButtons.SetActive(true);

        if (inGameMenu == true)
        {
            gameMenuBack.SetActive(true);
            standardMenuBack.SetActive(false);
        }

        if (inStandardMenu == true)
        {
            gameMenuBack.SetActive(false);
            standardMenuBack.SetActive(true);
        }
    }

    //--------------------------------------------------------------//

    // Start Menu functions
    public void showStartMenu()
    {
        startMenu.container.SetActive(true);
    }

    public void newGame_onClick()
	{
		SetContainersInactive();
        showGameMenu();
	}

    public void options_onClick()
    {

    }

    public void exit_onClick()
    {
        ExitGame();
    }

    //--------------------------------------------------------------//

    //Game Menu functions
    public void showGameMenu()
    {
        inGameMenu = true;
        inStandardMenu = false;

        SetUIActive();
        gameMenu.container.SetActive(true);

        gameMenu.applyJob.gameObject.SetActive(!player.c.stats.isEmployed);
    }

    public void gameMenu_back_onClick()
    {
        SetContainersInactive();
        showGameMenu();
    }

    //Standard Menu functions
    public void showStandardMenu()
    {
        inGameMenu = false;
        inStandardMenu = true;

        SetUIActive();
        standardMenu.container.SetActive(true);
    }

    public void standardMenu_back_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    //--------------------------------------------------------------//

    //School Menu functions
    public void school_onClick()
    {
        showSchoolMenu();
    }

    public void showSchoolMenu()
    {
        SetContainersInactive();
        schoolMenu.container.SetActive(true);
    }
    
    public void schoolTransit_onClick()
    {
        SetContainersInactive();
        timeOfDay += 8;
        showStandardMenu();
        UpdateUI();
    }

    public void schoolWalkLong_onClick()
    {
        SetContainersInactive();
        timeOfDay += 8;
        showStandardMenu();
        UpdateUI();
    }

    public void schoolWalkShort_onClick()
    {
        SetContainersInactive();
        timeOfDay += 8;
        showStandardMenu();
        UpdateUI();
    }
    
    //--------------------------------------------------------------//

    //Apply for Job functions
    public void applyJob_onClick()
    {
        showApplyJobMenu();
    }

    public void showApplyJobMenu()
    {
        SetContainersInactive();
        applyJobMenu.container.SetActive(true);
    }

    public void applyJob_coldCall_onClick()
    {
        float chance = Random.Range(0.0f, 100.0f);
        player.c.stats.isEmployed = chance >= 50.0f;
        timeOfDay += 8;
        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void applyJob_friend_onClick()
    {
        float chance = Random.Range(0.0f, 100.0f);
        player.c.stats.isEmployed = chance >= 50.0f;
        timeOfDay += 8;
        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void applyJob_applyOnline_onClick()
    {
        float chance = Random.Range(0.0f, 100.0f);
        player.c.stats.isEmployed = chance >= 50.0f;
        timeOfDay += 8;
        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    //--------------------------------------------------------------//

    //Job functions
    public void job_onClick()
    {
        showJobMenu();
    }
    
    public void showJobMenu()
    {
        SetContainersInactive();
        jobMenu.container.SetActive(true);
    }

    public void jobTransit_onClick()
    {
        player.c.stats.money += 50;
        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void jobWalkShort_onClick()
    {
        player.c.stats.money += 50;
        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void jobWalkLong_onClick()
    {
        player.c.stats.money += 50;
        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    //--------------------------------------------------------------//

    //Support functions
    public void support_onClick()
    {
        showSupportMenu();
    }

    public void showSupportMenu()
    {
        SetContainersInactive();
        supportMenu.container.SetActive(true);
    }

    public void supportFriends_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    public void supportFamily_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    public void supportServices_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    //--------------------------------------------------------------//

    //Money functions
    public void money_onClick()
    {
        showMoneyMenu();
    }

    public void showMoneyMenu()
    {
        SetContainersInactive();
        moneyMenu.container.SetActive(true);
    }

    public void moneySteal_onClick()
    {
        float chance = Random.Range(0.0f, 100.0f);
        if (chance >= 40.0f)
            player.c.stats.money += 15;
        else
        {
            player.c.stats.money -= 15;
            player.c.stats.wellbeing -= 10;
        }

        player.c.stats.hygiene -= 5;

        timeOfDay += 2;

        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void moneyPanhandle_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    public void moneyLabour_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    //--------------------------------------------------------------//

    //Stranger functions
    public void stranger_onClick()
    {
        showStrangerMenu();
    }

    public void showStrangerMenu()
    {
        SetContainersInactive();
        strangerMenu.container.SetActive(true);
    }

    public void strangerPedestrian_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    public void strangerHomeless_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    public void strangerShopOwner_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    //--------------------------------------------------------------//
    
    //Food functions    
    public void food_onClick()
    {
        showFoodMenu();
    }

    public void showFoodMenu()
    {
        SetContainersInactive();
        foodMenu.container.SetActive(true);
    }

    public void foodScavenge_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    public void foodFastFood_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    public void foodRefuge_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    //--------------------------------------------------------------//

    //Player stat change functions
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

    //Deprecated
    public void chance_onClick()
    {
        SetContainersInactive();
        chance.container.SetActive(true);

        ChanceTurn();
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
            if (!card.endsTurn) gameMenu_back_onClick();
            else chance.container.SetActive(false);
        });

        SendMessage(card.Function.ToString(), card.value);
        card.cardEvent.Invoke();

        UpdatePlayerInfo();
	}
}
