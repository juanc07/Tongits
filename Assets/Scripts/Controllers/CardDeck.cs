using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class CardDeck
{
    [SerializeField]
    private List<CardModel> _cardData;
    public bool IsDeckCreated = false;
    public int TotalCard => _cardData?.Count ?? 0;

    // initialize the deck with its dependencies
    public void Init()
    {
        _cardData = new List<CardModel>();
    }

    // create new deck
    public void CreateDeck()
    {
        _cardData.Clear();
        for (var i = 0; i <= 3; i++)
        {
            for (var j = 1; j <= 13; j++)
            {
                CardModel newCard = new CardModel {Suits = i, Rank = j};
                _cardData.Add(newCard);
            }
        }

        IsDeckCreated = true;
    }

    // randomize the arrange of cards
    public void ShuffleDeck()
    {
        var cardCount = _cardData.Count - 1;
        for (var i = cardCount; i > 1; --i)
        {
            var randomIndex = Random.Range(0, i + 1);
            var temp = _cardData[randomIndex];
            _cardData[randomIndex] = _cardData[i];
            _cardData[i] = temp;
        }
    }

    public List<CardModel> GetCards(int count)
    {
        var dataCollection = new List<CardModel>();
        if (_cardData.Count >= count)
        {
            Debug.Log($"before remove chunk: {_cardData.Count}");
            while (count > 0)
            {
                var data = _cardData[0];
                dataCollection.Add(data);
                _cardData.RemoveAt(0);
                count--;
            }
            Debug.Log($"After remove chunk: {_cardData.Count}");    
        }else {
            Debug.Log($"[CardDeck:GetCards] no more card to get!");
        }
        
        return dataCollection;
    }

    // get one card from top of the deck
    public CardModel Pop()
    {
        var targetIndex = _cardData.Count - 1;
        var cardData = _cardData[targetIndex];
        _cardData.RemoveAt(targetIndex);
        return cardData;
    }
    
    // get one card from bottom of the deck
    public CardModel GetBottom()
    {
        var cardData = _cardData[0];
        _cardData.RemoveAt(0);
        return cardData;
    }
    
    // get one card via index
    public CardModel GetCard(int index)
    {
        if (_cardData.Count >= index)
        {
            var cardData = _cardData[index];
            _cardData.RemoveAt(index);
            return cardData;
        }
        return null;
    }
    
    // get random card
    public CardModel GetRandomCard()
    {
        var rnd = new System.Random();
        var randomIndex = rnd.Next(0, _cardData.Count - 1);
        var cardData = _cardData[randomIndex];
        _cardData.RemoveAt(randomIndex);
        return cardData;
    }

    // get a reference copy
    public List<CardModel> GetData()
    {
        return _cardData;
    }

    // get a clone copy
    public List<CardModel> GetCloneData()
    {
        var clone = new List<CardModel>(_cardData);
        return clone;
    }

    public void ClearData()
    {
        _cardData.Clear();
        IsDeckCreated = false;
    }
}