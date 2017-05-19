using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct Card
{
    public string Title;
    public string Description;
    public Functions Function;
    public Sprite Image;
    public string value;

    public bool endsTurn;

    [Header("Will always be at least 1")]
    public int countInDeck;
}

[System.Serializable]
public enum Functions
{
    f_GoToJail,
    f_GetMoney,
    f_SetMoney,
    f_MissTurn,
}

public class CardProvider : MonoBehaviour
{
    public Card[] cardTemplates;

    private Card[] generatedCards;
    private int currentCardIndex;

    private GameController gc;
    private ScoreController sc;

	// Use this for initialization
	void Start ()
    {
        gc = GetComponent<GameController>();
        sc = GetComponent<ScoreController>();

        List<Card> newCardList = new List<Card>();

        foreach (Card c in cardTemplates)
        {
            for (int i = 0; i < Mathf.Max(1, c.countInDeck); i++)
            {
                newCardList.Add(c);
            }
        }

        generatedCards = newCardList.ToArray();

        currentCardIndex = generatedCards.Length - 1;

        Shuffle();
	}
	
    public Card GetCard()
    {
        if (currentCardIndex < 0)
        {
            generatedCards = generatedCards.OrderBy(v => Random.value).ToArray();
            currentCardIndex = generatedCards.Length - 1;
        }

        var card = generatedCards[currentCardIndex--];
        return card;
    }

    public void Shuffle()
    {
        generatedCards = generatedCards.OrderBy(v => Random.value).ToArray();
        currentCardIndex = generatedCards.Length - 1;
    }

    void f_SetMoney(string a_value)
    {
        int currPlayer = gc.currentPlayersTurn; // if it's 0, this is the main player

        sc.playerScores[currPlayer].money = int.Parse(a_value);
        if (sc.playerScores[currPlayer].money < 0)
            sc.playerScores[currPlayer].money = 0;
    }

    void f_GetMoney(string a_value)
    {
        int currPlayer = gc.currentPlayersTurn; // if it's 0, this is the main player

        sc.playerScores[currPlayer].money += int.Parse(a_value);
        if (sc.playerScores[currPlayer].money < 0)
            sc.playerScores[currPlayer].money = 0;
    }

    void f_GoToJail(string a_value)
    {
        int currPlayer = gc.currentPlayersTurn; // if it's 0, this is the main player

        sc.playerScores[currPlayer].missedTurns = int.Parse(a_value);
        sc.playerScores[currPlayer].inJail = true;
    }

    void f_MissTurn(string a_value)
    {
        int currPlayer = gc.currentPlayersTurn; // if it's 0, this is the main player

        sc.playerScores[currPlayer].missedTurns = int.Parse(a_value);
    }
}