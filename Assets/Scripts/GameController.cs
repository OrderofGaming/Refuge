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
}

[System.Serializable]
public class TitleMenu
{
    public GameObject container;
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
    [Header("Stats")]
	public int hygiene;
    public Image hygieneWheel;

    public int wellbeing;
    public Image wellBeingBar;

	public Text moneyText;
}

public class GameController : MonoBehaviour {

	[System.Serializable]
	public struct PlayerInfo
	{
		public string firstName;
		public int age;

		public Character c;
		public Image backgroundImage;
        public Text employment;
	}

    private bool inGameMenu = false;
    private bool inStandardMenu = false;

    [Header("Time of Day")]
    public TimeUtility time;
    public Text timeText;
    public Text AM_PM;

    [Header("Status")]
    public Text lateForSchool;
    public Text lateForWork;

    [Header("Turn Counter")]
    public int turnCounter = 1;
    public Text turnText;

    [Header("Activities")]
    public ActivityUtility activities;

    [Header("Interfaces")]
	public GameObject raycastBlock;
    public GameObject allMenus;
    public GameObject interfaceButtons;
    public GameObject gameMenuBack;
    public GameObject standardMenuBack;

	public UIInterface mainUI;

	public GameMenu gameMenu;
    public StandardMenu standardMenu;
    public StartMenu startMenu;
    public TitleMenu titleMenu;

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
        lateForWork.gameObject.SetActive(false);
        lateForSchool.gameObject.SetActive(false);

        time.SetTime(time.wakeUpTime);
        RandomizePlayer(player);
        InitializePlayer();
        UpdateUI();
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

    void InitializePlayer()
    {
        player.c.stats.hygiene = 80;
        player.c.stats.wellbeing = 75;
        player.c.stats.money = 50;
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

		//int fontSize = a_player.infoLabel.fontSize;

		// Add the info
		//a_player.infoLabel.text = a_player.firstName +
		//	"\n<size=" + (fontSize / 2).ToString() +">" + (a_player.c.isMale ? "Male, " : "Female, ") +
		//a_player.age.ToString () + "</size>";
		
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

    public void spendMoney(int cost)
    {
        player.c.stats.money -= cost;
        UpdateUI();
    }

    public void UpdateTime()
    {
        int hour = time.GetHour();
        int minutes = (time.tickCounter % (60 / time.tickSize)) * time.tickSize;

        timeText.text = ((hour > 12) ? hour - 12 : hour).ToString() + ":" + ((minutes < 10) ? "0" : "") + minutes.ToString();
        AM_PM.text = (hour >= 12) ? "PM" : "AM";
    }

    public void UpdatePlayerInfo()
    {
        mainUI.moneyText.text = "$" + player.c.stats.money.ToString("N0");

        mainUI.hygiene = player.c.stats.hygiene;
        mainUI.wellbeing = player.c.stats.wellbeing;
        mainUI.wellBeingBar.fillAmount = mainUI.wellbeing / 100.0f;
        mainUI.hygieneWheel.fillAmount = mainUI.hygiene / 100.0f;

        player.employment.text = player.c.stats.isEmployed ? "Employed" : "Unemployed";
    }

    public void UpdatePlayerStatus()
    {
        lateForWork.gameObject.SetActive(true);
        lateForSchool.gameObject.SetActive(true);

        if (player.c.stats.wentToWork)
        {
            lateForWork.gameObject.SetActive(true);
            lateForWork.text = player.c.stats.onTimeWork ? "On Time for Work" : "Late for Work";
            lateForWork.color = player.c.stats.onTimeWork ? Color.blue : Color.red;
        }
        else
        {
            lateForWork.gameObject.SetActive(false);
        }

        if (player.c.stats.attendedSchool)
        {
            lateForSchool.gameObject.SetActive(true);
            lateForSchool.text = player.c.stats.onTimeSchool ? "On Time for School" : "Late for School";
            lateForSchool.color = player.c.stats.onTimeSchool ? Color.blue : Color.red;
        }
        else
        {
            lateForSchool.gameObject.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        if (time.GetHour() >= 24)
            EndPlayerTurn();

        UpdateTime();
        UpdatePlayerInfo();
        UpdatePlayerStatus();
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

        ComputeSleep();
        ResetTime();

        SetContainersInactive();
        showGameMenu();

        UpdateUI();

        LogPlayerInfo();

        player.c.stats.wentToWork = false;
        player.c.stats.attendedSchool = false;
        lateForWork.gameObject.SetActive(false);
        lateForSchool.gameObject.SetActive(false);
    }

    public void LogPlayerInfo()
    {
        Debug.Log("Wellbeing = " + player.c.stats.wellbeing.ToString());
        Debug.Log("Hygiene = " + player.c.stats.hygiene.ToString());
        Debug.Log("Sleep Quality = " + player.c.stats.lastNightSleep.ToString());
    }

    public void ComputeSleep()
    {
        if (player.c.stats.wellbeing <= 35)
            player.c.stats.lastNightSleep = PlayerStats.sleepQuality.BAD;
        else if (player.c.stats.wellbeing > 70)
            player.c.stats.lastNightSleep = PlayerStats.sleepQuality.GOOD;
        else
            player.c.stats.lastNightSleep = PlayerStats.sleepQuality.NEUTRAL;
    }

    public void PopulateActivityList()
    {

    }

    //Game time functions         
    public void ResetTime()
    {
        if (player.c.stats.lastNightSleep == PlayerStats.sleepQuality.GOOD)
            time.SetTime(7);
        else if (player.c.stats.lastNightSleep == PlayerStats.sleepQuality.NEUTRAL)
            time.SetTime(Random.Range(7.0f, 8.0f));
        else
        {
            float chance = Random.Range(0.0f, 100.0f);
            if (chance >= 11)
            {
                time.SetTime(Random.Range(8.0f, 11.0f));
            }
            else
            {
                time.SetTime(7, 0);
                player.c.stats.wellbeing -= 25;
            }
        }
    }

    public void SetContainersInactive()
    {
        foreach (Transform child in allMenus.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void SetContainersActive()
    {
        foreach (Transform child in allMenus.transform)
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
        titleMenu.container.SetActive(true);
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
    
    public void schoolTravel_onClick(int minutes)
    {
        SetContainersInactive();

        time.AdvanceTime(0, minutes);
        player.c.stats.onTimeSchool = time.IsBefore(8, 31);

        time.SetTime(15, 30);
        player.c.stats.attendedSchool = true;

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
        player.c.stats.isEmployed = true;
        time.SetTime(17);
        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void applyJob_friend_onClick()
    {
        float chance = Random.Range(0.0f, 100.0f);
        player.c.stats.isEmployed = chance >= 50.0f;
        time.SetTime(17);
        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void applyJob_applyOnline_onClick()
    {
        float chance = Random.Range(0.0f, 100.0f);
        player.c.stats.isEmployed = chance >= 50.0f;
        time.SetTime(17);
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

    public void jobTravel_onClick(int minutes)
    {
        time.AdvanceTime(0, minutes);
        player.c.stats.onTimeWork = time.IsBefore(8, 1);

        Debug.Log(player.c.stats.onTimeWork.ToString());

        time.SetTime(17);
        player.c.stats.wentToWork = true;

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

        time.AdvanceTime(1, 15);

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
