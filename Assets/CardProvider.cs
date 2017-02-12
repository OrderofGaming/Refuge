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
    public Image Image;
}

[System.Serializable]
public enum Functions
{
    GoToJail,
    GetMoney,
}

public class CardProvider : MonoBehaviour
{
    public Card[] cardTemplates;

    public int cardsToGenerate;
    
    private Card[] generatedCards;
    private int currentCardIndex;

	// Use this for initialization
	void Start ()
    {
        generatedCards = new Card[cardsToGenerate];

        for (int i = 0; i < cardsToGenerate; i++)
        {
            int templateNumber = Random.Range(0, cardTemplates.Length);

            generatedCards[i] = cardTemplates[templateNumber];
        }

        currentCardIndex = generatedCards.Length - 1;
        //InvokeRepeating("GetCard", 0, 1);
	}
	
    public void GetCard()
    {
        if (currentCardIndex < 0)
        {
            generatedCards = generatedCards.OrderBy(v => Random.value).ToArray();
            currentCardIndex = generatedCards.Length - 1;
            //OnEmpty.RemoveAllListeners();
        }

        var card = generatedCards[currentCardIndex--];

        SendMessage(card.Function.ToString());
    }
}
