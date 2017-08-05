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
    public UnityEvent cardEvent;
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
    private PlayerStats ps;

	// Use this for initialization
	void Start ()
    {
        gc = GetComponent<GameController>();
        ps = gc.player.c.stats;

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
        ps.money = int.Parse(a_value);
        if (ps.money < 0)
            ps.money = 0;
    }

    void f_GetMoney(string a_value)
    {
        ps.money += int.Parse(a_value);
        if (ps.money < 0)
            ps.money = 0;
    }
}