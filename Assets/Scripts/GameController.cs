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
public class ActivityMenu
{
    public GameObject container;
    public List<ActivityButton> activityButtons;
}

[System.Serializable]
public class SleepMenu
{
    public GameObject container;
    public Button home, shelter;
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

[System.Serializable]
public class PopupWindow
{
    public GameObject container;
    public Text popUpText;
    public Button nextStandardButton, nextGameButton, nextActivityButton, nextSleepButton;
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

    public Button swapButton;
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
    private bool inActivityMenu = false;
    private bool inSleepMenu = false;

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
    public List<Text> activityText;

    [Header("Interfaces")]
	public GameObject raycastBlock;
    public GameObject allMenus;
    public GameObject interfaceButtons;
    public GameObject gameMenuBack;
    public GameObject standardMenuBack;
    public GameObject activityMenuBack;
    public GameObject sleepMenuBack;

	public UIInterface mainUI;

    public PopupWindow popUpWindow;

	public GameMenu gameMenu;
    public StandardMenu standardMenu;
    public ActivityMenu activityMenu;
    public SleepMenu sleepMenu;
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
        player.c.stats.hygiene = 70;
        player.c.stats.wellbeing = 60;
        player.c.stats.money = 30;
        player.c.stats.hasHome = false;
    }

	void RandomizePlayer(PlayerInfo a_player)
	{
		//// Male or female?
		//a_player.c.isMale = Random.Range (0, 2) == 0 ? true : false;

		//if (a_player.c.isMale) {
		//	a_player.firstName = maleNames [Random.Range (0, maleNames.Length)] + " " + ((char)(65 + Random.Range(0, 26))).ToString() + ".";
		//} else {
		//	a_player.firstName = femaleNames [Random.Range (0, femaleNames.Length)] + " " + ((char)(65 + Random.Range(0, 26))).ToString() + ".";
		//}
		//// Not sure which numbers to use
		//a_player.age = Random.Range (16, 23);

		////int fontSize = a_player.infoLabel.fontSize;

		//// Add the info
		////a_player.infoLabel.text = a_player.firstName +
		////	"\n<size=" + (fontSize / 2).ToString() +">" + (a_player.c.isMale ? "Male, " : "Female, ") +
		////a_player.age.ToString () + "</size>";
		
		//// Pick a random hairstyle
		//a_player.c.hairdo = Random.Range (1, 7);

		//// A random shirt color
		//a_player.c.shirtColor = Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.2f, 1.0f, 1.0f, 1.0f);
		//// Pick a random haircolor
		//a_player.c.hairColor = m_haircolors [Random.Range (0, m_haircolors.Length)];

		//// Pick a random skintone
		//a_player.c.skinTone = m_skintones [Random.Range (0, m_skintones.Length)];

		//// Pick a random sleeve length
		//a_player.c.sleeveLength = Random.Range(0, 4);

		//// Start smiling!
		//a_player.c.smile = 3;

		//// Update everyone
		//a_player.c.UpdateCharacter ();
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

        if (player.c.stats.hygiene > 100)
            player.c.stats.hygiene = 100;
        if (player.c.stats.hygiene <= 0)
            player.c.stats.hygiene = 0;
        if (player.c.stats.wellbeing > 100)
            player.c.stats.wellbeing = 100;
        if (player.c.stats.wellbeing <= 0)
            player.c.stats.wellbeing = 0;

        mainUI.hygiene = player.c.stats.hygiene;
        mainUI.wellbeing = player.c.stats.wellbeing;
 
        mainUI.wellBeingBar.fillAmount = mainUI.wellbeing / 100.0f;
        mainUI.hygieneWheel.fillAmount = mainUI.hygiene / 100.0f;

        player.employment.text = player.c.stats.isEmployed ? "<color=lime>Employed</color>" : "<color=white>Unemployed</color>";
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
        mainUI.swapButton.gameObject.SetActive(false);
    }

    public void SetUIActive()   
    {
        interfaceButtons.SetActive(true);

        if (inGameMenu)
        {
            gameMenuBack.SetActive(true);
            standardMenuBack.SetActive(false);
            activityMenuBack.SetActive(false);
            sleepMenuBack.SetActive(false);

            popUpWindow.nextGameButton.gameObject.SetActive(true);
            popUpWindow.nextStandardButton.gameObject.SetActive(false);
            popUpWindow.nextActivityButton.gameObject.SetActive(false);

            mainUI.swapButton.gameObject.SetActive(true);
        }

        if (inStandardMenu)
        {
            gameMenuBack.SetActive(false);
            standardMenuBack.SetActive(true);
            activityMenuBack.SetActive(false);
            sleepMenuBack.SetActive(false);

            popUpWindow.nextGameButton.gameObject.SetActive(false);
            popUpWindow.nextStandardButton.gameObject.SetActive(true);
            popUpWindow.nextActivityButton.gameObject.SetActive(false);
            popUpWindow.nextSleepButton.gameObject.SetActive(false);

            mainUI.swapButton.gameObject.SetActive(true);
        }
        
        if (inActivityMenu)
        {
            gameMenuBack.SetActive(false);
            standardMenuBack.SetActive(false);
            activityMenuBack.SetActive(true);
            sleepMenuBack.SetActive(false);

            popUpWindow.nextGameButton.gameObject.SetActive(false);
            popUpWindow.nextStandardButton.gameObject.SetActive(false);
            popUpWindow.nextActivityButton.gameObject.SetActive(true);
            popUpWindow.nextSleepButton.gameObject.SetActive(false);

            mainUI.swapButton.gameObject.SetActive(true);
        }

        if(inSleepMenu)
        {
            gameMenuBack.SetActive(false);
            standardMenuBack.SetActive(false);
            activityMenuBack.SetActive(false);
            sleepMenuBack.SetActive(true);

            popUpWindow.nextGameButton.gameObject.SetActive(false);
            popUpWindow.nextStandardButton.gameObject.SetActive(false);
            popUpWindow.nextActivityButton.gameObject.SetActive(false);
            popUpWindow.nextSleepButton.gameObject.SetActive(true);

            mainUI.swapButton.gameObject.SetActive(true);
        }
    }

    //Dice rolling & checking functions
    public int RollDie()
    {
        int result = Random.Range(0, 100);

        Debug.Log("Rolled: " + result);
        return result;
    }

    public int RollDie(int wellbeing, int hygiene)
    {
        int result = Random.Range(0, 100);
        int modifier = wellbeing + hygiene;
        Debug.Log("Modifier: " + modifier);

        result += modifier;

        if (result >= 100)
            result = 100;

        Debug.Log("Rolled: " + result);
        return result;
    }

    public int RollDie(int stat, float statpercent)
    {
        int result = Random.Range(0, 100);

        int statmodifier = (int)(stat * ((statpercent / 100.0f)));

        result += statmodifier;

        if (result >= 100)
            result = 100;

        return result;
    }

    public int RollDie(int wellbeing, float wbpercent, int hygiene, float hygpercent)
    {
        int result = Random.Range(0, 100);

        int wbmodifier = (int)(wellbeing * ((wbpercent / 100.0f)));
        int hygmodifier = (int)(hygiene * ((hygpercent / 100.0f)));

        int modifier = wbmodifier + hygmodifier;
        Debug.Log("Modifier: " + modifier);

        result += modifier;

        if (result >= 100)
            result = 100;

        Debug.Log("Rolled: " + result);
        return result;
    }

    //--------------------------------------------------------------//

    // PopUp Window Functions
    public void showPopUpWindow()
    {
        SetContainersInactive();
        popUpWindow.popUpText.text.Replace("\n", "\n");
        popUpWindow.container.SetActive(true);
    }

    public void popUpMenu_onClick()
    {
        showPopUpWindow();
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
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();
        gameMenu.container.SetActive(true);

        gameMenu.applyJob.gameObject.SetActive(!player.c.stats.isEmployed);
    }

    public void gameMenu_back_onClick()
    {
        SetContainersInactive();
        showGameMenu();
    }

    //--------------------------------------------------------------//

    //Standard Menu functions
    public void showStandardMenu()
    {
        inGameMenu = false;
        inStandardMenu = true;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();
        standardMenu.container.SetActive(true);
    }

    public void standardMenu_back_onClick()
    {
        SetContainersInactive();
        showStandardMenu();
    }

    //--------------------------------------------------------------//

    //Sleep Menu functions
    public void showSleepMenu()
    {
        inGameMenu = false;
        inStandardMenu = false;
        inActivityMenu = false;
        inSleepMenu = true;

        SetUIActive();

        SetContainersInactive();
        sleepMenu.container.SetActive(true);

        if (player.c.stats.hasHome)
        {
            sleepMenu.shelter.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    public void sleepMenu_back_onClick()
    {
        SetContainersInactive();
        SetUIActive();
        showSleepMenu();
    }

    public void sleep_Home_onClick()
    {
        popUpWindow.popUpText.text = "You go home to your soft bed and sleep peacefully.";
        player.c.stats.wellbeing += 20;

        UpdateUI();
        showPopUpWindow();
    }

    public void sleep_Shelter_onClick()
    {
        popUpWindow.popUpText.text = "You arrive at the local homeless shelter hoping to to find a place however when you attempt to head inside you find that there are no available spots. One of the staff members sighs and tells you that you should have come a lot earlier. You end up having to leave the shelter and look for another place to sleep. You find a small deserted back alley and settle down against a wall.\n\n<color=red>Wellbeing -25</color>\n<color=blue>Hygiene -20</color>";
        player.c.stats.wellbeing -= 25;
        player.c.stats.hygiene -= 20;

        UpdateUI();
        showPopUpWindow();
    }

    //--------------------------------------------------------------//

    //Activity Menu functions
    public void showActivityMenu()
    {
        inGameMenu = false;
        inStandardMenu = false;
        inActivityMenu = true;
        inSleepMenu = false;

        SetUIActive();
        UpdateActivityMenu();
        activityMenu.container.SetActive(true);
    }

    public void activityMenu_back_onClick()
    {
        SetContainersInactive();
        showActivityMenu();
    }

    public void UpdateActivityMenu()
    {
        for (int i = 0; i < activityMenu.activityButtons.Count; ++i)
        {
            activityMenu.activityButtons[i].SetActivityData(activities.listOfActivities[i]);
        }
    }

    public void menuSwap_onClick()
    {
        if (inStandardMenu && time.IsBefore(8, 30))
        {
            SetContainersInactive();
            showGameMenu();
            UpdateUI();
        }
        else if (inStandardMenu && !time.IsBefore(15, 30))
        {
            SetContainersInactive();
            showActivityMenu();
            UpdateUI();
        }
        else if (inGameMenu && time.IsBefore(8, 30))
        {
            SetContainersInactive();
            showStandardMenu();
            UpdateUI();
        }
        else if (inGameMenu && time.IsBefore(15, 30))
        {
            SetContainersInactive();
            showStandardMenu();
            UpdateUI();
        }
        else if (inActivityMenu && !time.IsBefore(15, 30))
        {
            SetContainersInactive();
            showStandardMenu();
            UpdateUI();
        }
        else if (inSleepMenu && time.IsBefore(8, 30))
        {
            SetContainersInactive();
            showGameMenu();
            UpdateUI();
        }
        else if (inSleepMenu && !time.IsBefore(15, 30))
        {
            SetContainersInactive();
            showStandardMenu();
            UpdateUI();
        }
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

    public void applyJob_coldCall_onClick(int difficultyCheck)
    {
        int result = RollDie(player.c.stats.wellbeing, 60.0f, player.c.stats.hygiene, 40.0f);

        if (result >= difficultyCheck)
        {
            player.c.stats.isEmployed = true;
            popUpWindow.popUpText.text = "You spent the day searching for a job. You managed to get lucky and score an entry level position at a local business. You start the next day. The sun is starting to set as you exit the building.\n\n<color=lime>Employed</color>";
        }
        else
        {
            popUpWindow.popUpText.text = "You spent the day searching for a job. Despite your best efforts, nobody was interested in hiring you. You'll have to try again another day. The sun is starting to set as you exit the building.";
        }

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.SetTime(17);
        SetContainersInactive();
        showPopUpWindow();

        //showStandardMenu();

        UpdateUI();
    }

    public void applyJob_friend_onClick(int difficultyCheck)
    {

        int result = RollDie(player.c.stats.wellbeing, 30.0f, player.c.stats.hygiene, 70.0f);

        if (result >= difficultyCheck)
        {
            player.c.stats.isEmployed = true;
            popUpWindow.popUpText.text = "You spent the day searching for a job. You managed to get lucky and score an entry level position at a local business. You start the next day. The sun is starting to set as you exit the building.";
        }
        else
        {
            popUpWindow.popUpText.text = "You spent the day searching for a job. Despite your best efforts, nobody was interested in hiring you. You'll have to try again another day. The sun is starting to set as you exit the building.";
        }

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.SetTime(17);
        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
    }

    public void applyJob_applyOnline_onClick(int difficultyCheck)
    {
        int result = RollDie(player.c.stats.wellbeing, 40.0f, player.c.stats.hygiene, 60.0f);

        if (result >= difficultyCheck)
        {
            player.c.stats.isEmployed = true;
            popUpWindow.popUpText.text = "You spent the day searching for a job. You managed to get lucky and score an entry level position at a local business. You start the next day. The sun is starting to set as you exit the building.";
        }
        else
        {
            popUpWindow.popUpText.text = "You spent the day searching for a job. Despite your best efforts, nobody was interested in hiring you. You'll have to try again another day. The sun is starting to set as you exit the building.";
        }

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.SetTime(17);
        SetContainersInactive();
        showPopUpWindow();

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
        int chance = RollDie();
        Debug.Log("Good/Bad Roll = " + chance);
        int numEvents = (int)(Mathf.Round(Random.Range(0.5f, 3.499f)));
        Debug.Log("Number of Events = " + numEvents);

        for (int i = 0; i < numEvents; ++i)
        {
            int nothingEvent = RollDie();
            Debug.Log("Roll " + (i+1) + " = " + nothingEvent);
            if (nothingEvent > 10)
            {
                int pickEvent = (int)(Mathf.Round(Random.Range(-0.499f, 3.499f)));
                Debug.Log("Pick Event = " + pickEvent);
                if (chance > 50)
                    switch (pickEvent)
                    {
                        case 0:
                            player.c.stats.wellbeing += (int)Random.Range(10, 60);
                            break;
                        case 1:
                            player.c.stats.hygiene += (int)Random.Range(10, 60);
                            break;
                        case 2:
                            player.c.stats.money += (int)Random.Range(10, 50);
                            break;
                        case 3:
                            player.c.stats.hasHome = true;
                            break;
                        default:
                            break;
                    }
                else
                {
                    switch (pickEvent)
                    {
                        case 0:
                            player.c.stats.wellbeing -= (int)Random.Range(10, 60);
                            break;
                        case 1:
                            player.c.stats.hygiene -= (int)Random.Range(10, 60);
                            break;
                        case 2:
                            player.c.stats.money -= (int)Random.Range(10, 50);
                            break;
                        case 3:
                            player.c.stats.hasHome = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Debug.Log("Nothing happened");
            }
        }

        time.SetTime(17);

        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void supportFamily_onClick()
    {
        int chance = RollDie();
        Debug.Log("Good/Bad Roll = " + chance);
        int numEvents = (int)(Mathf.Round(Random.Range(0.5f, 3.499f)));
        Debug.Log(numEvents);

        for (int i = 0; i < numEvents; ++i)
        {
            int nothingEvent = RollDie();
            Debug.Log("Roll " + (i + 1) + " = " + nothingEvent);
            if (nothingEvent > 10)
            {
                int pickEvent = (int)(Mathf.Round(Random.Range(-0.499f, 3.499f)));
                if (chance > 50)
                    switch (pickEvent)
                    {
                        case 0:
                            player.c.stats.wellbeing += (int)Random.Range(10, 60);
                            break;
                        case 1:
                            player.c.stats.hygiene += (int)Random.Range(10, 60);
                            break;
                        case 2:
                            player.c.stats.money += (int)Random.Range(10, 50);
                            break;
                        case 3:
                            player.c.stats.hasHome = true;
                            break;
                        default:
                            break;
                    }
                else
                {
                    switch (pickEvent)
                    {
                        case 0:
                            player.c.stats.wellbeing -= (int)Random.Range(10, 60);
                            break;
                        case 1:
                            player.c.stats.hygiene -= (int)Random.Range(10, 60);
                            break;
                        case 2:
                            player.c.stats.money -= (int)Random.Range(10, 50);
                            break;
                        case 3:
                            player.c.stats.hasHome = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Debug.Log("Nothing happened");
            }
        }

        time.SetTime(17);

        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
    }

    public void supportServices_onClick()
    {
        int chance = RollDie();
        int numEvents = (int)(Mathf.Round(Random.Range(0.5f, 3.499f)));

        for (int i = 0; i < numEvents; ++i)
        {
            int nothingEvent = RollDie();
            if (nothingEvent > 10)
            {
                int pickEvent = (int)(Mathf.Round(Random.Range(-0.499f, 3.499f)));
                if (chance > 50)
                    switch (pickEvent)
                    {
                        case 0:
                            player.c.stats.wellbeing += (int)Random.Range(10, 60);
                            break;
                        case 1:
                            player.c.stats.hygiene += (int)Random.Range(10, 60);
                            break;
                        case 2:
                            player.c.stats.money += (int)Random.Range(10, 50);
                            break;
                        case 3:
                            player.c.stats.hasHome = true;
                            break;
                        default:
                            break;
                    }
                else
                {
                    switch (pickEvent)
                    {
                        case 0:
                            player.c.stats.wellbeing -= (int)Random.Range(10, 60);
                            break;
                        case 1:
                            player.c.stats.hygiene -= (int)Random.Range(10, 60);
                            break;
                        case 2:
                            player.c.stats.money -= (int)Random.Range(10, 50);
                            break;
                        case 3:
                            player.c.stats.hasHome = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                
            }
        }

        time.SetTime(17);

        SetContainersInactive();
        showStandardMenu();

        UpdateUI();
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

    public void moneySteal_onClick(int difficultyCheck)
    {
        int skillCheck = RollDie(player.c.stats.wellbeing, 80.0f);

        if (skillCheck > difficultyCheck)
        {
            int prevMoney = player.c.stats.money;
            int prevWell = player.c.stats.wellbeing;
            int prevHyg = player.c.stats.hygiene;

            player.c.stats.money += Random.Range(5, 60);
            player.c.stats.wellbeing -= 15;
            player.c.stats.hygiene -= Random.Range(1, 10);

            int diffMoney = player.c.stats.money - prevMoney;
            int diffWell = prevWell - player.c.stats.wellbeing;
            int diffHygiene = prevHyg - player.c.stats.hygiene;

            popUpWindow.popUpText.text = "You managed to successfully steal some cash, however, you tripped and fell during your escape and running away has made you tired.\n\n<color=green>Money +" + diffMoney + "</color>\n<color=red>Wellbeing -" + diffWell + "</color>\n<color=blue>Hygiene -" + diffHygiene + "</color>";
        }
        else
        {
            int prevMoney = player.c.stats.money;
            int prevWell = player.c.stats.wellbeing;
            int prevHyg = player.c.stats.hygiene;

            player.c.stats.money -= Random.Range(15, player.c.stats.money);
            player.c.stats.wellbeing -= Random.Range(10, 20);
            player.c.stats.hygiene -= Random.Range(5, 15);

            int diffMoney = prevMoney - player.c.stats.money;
            int diffWell = prevWell - player.c.stats.wellbeing;
            int diffHyg = prevHyg - player.c.stats.hygiene;

            popUpWindow.popUpText.text = "You were caught attempting to steal money from some older kids and they beat you up and took some of your cash.\n\n<color=green>Money -" + diffMoney + "</color>\n<color=red>Wellbeing -" + diffWell + "\n</color><color=blue>Hygiene -" + diffHyg + "</color>";
        }

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(3);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
    }

    public void moneyPanhandle_onClick(int difficultyCheck)
    {
        int skillCheck = RollDie(player.c.stats.hygiene, 20.0f);

        if (skillCheck > difficultyCheck)
        {
            int prevMoney = player.c.stats.money;
            int prevWell = player.c.stats.wellbeing;
            int prevHyg = player.c.stats.hygiene;

            player.c.stats.money += Random.Range(5, 60);
            player.c.stats.wellbeing -= Random.Range(5, 10);
            player.c.stats.hygiene -= Random.Range(5, 10);

            int diffMoney = player.c.stats.money - prevMoney;
            int diffWell = prevWell - player.c.stats.wellbeing;
            int diffHyg = prevHyg - player.c.stats.hygiene;

            popUpWindow.popUpText.text = "You found an unoccupied corner on a busy street and panhandled for a few hours. Luck was in your favour and of the people passing by gave you some money.\n<color=green>Money +" + diffMoney + "</color>\n<color=red>Wellbeing -" + diffWell + "</color>\n<color=blue>Hyiene -" + diffHyg + "</color>"; 
        }
        else
        {
            int prevMoney = player.c.stats.money;
            int prevWell = player.c.stats.wellbeing;
            int prevHyg = player.c.stats.hygiene;

            player.c.stats.money -= Random.Range(5, 20);
            player.c.stats.wellbeing -= Random.Range(10, 20);
            player.c.stats.hygiene -= Random.Range(5, 15);

            int diffMoney = prevMoney - player.c.stats.money;
            int diffWell = prevWell - player.c.stats.wellbeing;
            int diffHyg = prevHyg - player.c.stats.hygiene;

            popUpWindow.popUpText.text = "You sat for hours outside a coffee shop but not a single person gave you anything. You had to use the bathroom and the owner wouldn't let you without buying something.\n\n<color=green>Money -" + diffMoney + "</color>\n<color=red>Wellbeing -" + diffWell + "</color>\n<color=blue>Hygiene -" + diffHyg + "</color>";
        }
       
        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(3);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
    }

    public void moneyLabour_onClick(int difficultyCheck)
    {
        int skillCheck = RollDie(player.c.stats.wellbeing, 60.0f, player.c.stats.hygiene, 20.0f);

        if (skillCheck > difficultyCheck)
        {
            int prevMoney = player.c.stats.money;
            int prevWell = player.c.stats.wellbeing;
            int prevHyg = player.c.stats.hygiene;

            player.c.stats.money += Random.Range(5, 60);
            player.c.stats.wellbeing -= Random.Range(5, 10);
            player.c.stats.hygiene -= Random.Range(5, 10);

            int diffMoney = player.c.stats.money - prevMoney;
            int diffWell = prevWell - player.c.stats.wellbeing;
            int diffHyg = prevHyg - player.c.stats.hygiene;

            popUpWindow.popUpText.text = "You found someone looking for extra hands and helped them out for a few hours.\n\n<color=green>Money +" + diffMoney + "</color>\n<color=red>Wellbeing -" + diffWell + "</color>\n<color=blue>Hyiene -" + diffHyg + "</color>";
        }
        else
        {
            int prevMoney = player.c.stats.money;
            int prevWell = player.c.stats.wellbeing;
            int prevHyg = player.c.stats.hygiene;

            player.c.stats.money -= Random.Range(5, 20);
            player.c.stats.wellbeing -= Random.Range(10, 20);
            player.c.stats.hygiene -= Random.Range(5, 15);

            int diffMoney = prevMoney - player.c.stats.money;
            int diffWell = prevWell - player.c.stats.wellbeing;
            int diffHyg = prevHyg - player.c.stats.hygiene;

            popUpWindow.popUpText.text = "You checked every place you knew, but nobody was looking for extra help.\n\n<color=green>Money -" + diffMoney + "</color>\n<color=red>Wellbeing -" + diffWell + "</color>\n<color=blue>Hygiene -" + diffHyg + "</color>";
        }

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(3);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
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
        int prevMoney = player.c.stats.money;
        int prevWell = player.c.stats.wellbeing;
        int prevHyg = player.c.stats.hygiene;

        player.c.stats.money += Random.Range(5, 60);
        player.c.stats.wellbeing += Random.Range(5, 40);
        player.c.stats.hygiene += Random.Range(5, 20);

        int diffMoney = player.c.stats.money - prevMoney;
        int diffWell = player.c.stats.wellbeing - prevWell;
        int diffHyg = player.c.stats.hygiene - prevHyg;

        popUpWindow.popUpText.text = "You were walking along the streets and encountered a stranger.\n\n<color=green>Money " + diffMoney + "</color>\n<color=red>Wellbeing " + diffWell + "</color>\n<color=blue>Hygiene " + diffHyg + "</color>";

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(3);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
    }

    public void strangerHomeless_onClick()
    {
        int prevMoney = player.c.stats.money;
        int prevWell = player.c.stats.wellbeing;
        int prevHyg = player.c.stats.hygiene;

        player.c.stats.money += Random.Range(5, 60);
        player.c.stats.wellbeing += Random.Range(5, 40);
        player.c.stats.hygiene += Random.Range(5, 20);

        int diffMoney = player.c.stats.money - prevMoney;
        int diffWell = player.c.stats.wellbeing - prevWell;
        int diffHyg = player.c.stats.hygiene - prevHyg;

        popUpWindow.popUpText.text = "You went to an area frequented by other homeless people and spent a few hours there.\n\n<color=green>Money " + diffMoney + "</color>\n<color=red>Wellbeing " + diffWell + "</color>\n<color=blue>Hygiene " + diffHyg + "</color>";

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(3);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
    }

    public void strangerShopOwner_onClick()
    {
        int prevMoney = player.c.stats.money;
        int prevWell = player.c.stats.wellbeing;
        int prevHyg = player.c.stats.hygiene;

        player.c.stats.money += Random.Range(5, 60);
        player.c.stats.wellbeing += Random.Range(5, 40);
        player.c.stats.hygiene += Random.Range(5, 20);

        int diffMoney = player.c.stats.money - prevMoney;
        int diffWell = player.c.stats.wellbeing - prevWell;
        int diffHyg = player.c.stats.hygiene - prevHyg;

        popUpWindow.popUpText.text = "You visited a number of local stores, buying and selling some of your extra things.\n\n<color=green>Money " + diffMoney + "</color>\n<color=red>Wellbeing " + diffWell + "</color>\n<color=blue>Hygiene " + diffHyg + "</color>";

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(3);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
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
        int chance = RollDie();

        if (chance > 74)
        {
            player.c.stats.wellbeing += 15;
            player.c.stats.hygiene -= 15;
            popUpWindow.popUpText.text = "You rummage through the garbage bins behind a restaurant downtown and find an almost completely uneaten meal. You don't smell good afterwards.\n\n<color=red>Wellbeing +15</color>\n<color=blue>Hygiene -15</color>";
        }
        else if (chance > 50)
        {
            player.c.stats.wellbeing += 10;
            player.c.stats.hygiene -= 15;
            popUpWindow.popUpText.text = "You rummage through the garbage bins behind a restaurant downtown and find a few edible scraps. You don't smell good afterwards.\n\n<color=red>Wellbeing +10</color>\n<color=blue>Hygiene -15</color>";
        }
        else
        {
            player.c.stats.wellbeing -= 10;
            player.c.stats.hygiene -= 15;
            popUpWindow.popUpText.text = "You rummage through the garbage bins behind a restaurant downtown and find nothing. You smell much worse for your efforts.\n\n<color=red>Wellbeing -10</color>\n<color=blue>Hygiene -15</color>";
        }

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(2);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
    }

    public void foodFastFood_onClick()
    {
        int prevMoney = player.c.stats.money;
        int prevWell = player.c.stats.wellbeing;
        int prevHyg = player.c.stats.hygiene;

        player.c.stats.money -= Random.Range(5, 15);
        player.c.stats.wellbeing += 10;

        int diffMoney = prevMoney - player.c.stats.money;
        int diffWell = player.c.stats.wellbeing - prevWell;

        popUpWindow.popUpText.text = "You spend some money to grab a quick meal from the local fast food joint. \n\n<color=green>Money -" +diffMoney + "</color>\n<color=red>Wellbeing +" + diffWell + "</color>";

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(2);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
    }

    public void foodRestaurant_onClick()
    {
        int prevMoney = player.c.stats.money;

        player.c.stats.money -= Random.Range(15, 30);
        player.c.stats.wellbeing += 20;

        int diffMoney = prevMoney - player.c.stats.money;

        popUpWindow.popUpText.text = "You decide to spend some of your limited funds on a quality meal. \n\n<color=green>Money -" + diffMoney + "</color>\n<color=red>Wellbeing + 20</color>";

        inStandardMenu = true;
        inGameMenu = false;
        inActivityMenu = false;
        inSleepMenu = false;

        SetUIActive();

        time.AdvanceTime(2);

        SetContainersInactive();
        showPopUpWindow();

        UpdateUI();
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
