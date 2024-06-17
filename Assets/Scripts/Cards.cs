using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    public string cardType;
    public int cardValue;

    public CardInfo cardInfo;
    public CardsManager playerCardsManager;
    public OpponentCardsManager opponentCardsManager;

    [SerializeField] private Text type_Text;
    [SerializeField] private Text value_Text;

    private void Start()
    {
        UpdateCard();
    }

    public Cards(string type, int value)
    {
        cardType = type;
        cardValue = value;
    }

    public string CardType
    {
        get { return cardType; }
        set { cardType = value; }
    }

    public int CardValue
    {
        get { return cardValue; }
        set { cardValue = value; }
    }

    public void UpdateCard()
    {
        if (cardInfo != null)
        {
            cardValue = cardInfo.value;
            CardType = cardInfo.type;
            type_Text.text = cardType;
            value_Text.text = cardValue.ToString();
        }
        else
        {
            Debug.LogError($"{name} - CardInfo is not assigned.");
        }
    }
}
