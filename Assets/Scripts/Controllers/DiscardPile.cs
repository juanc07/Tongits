using System.Collections.Generic;
using UnityEngine;

public class DiscardPile
{
    private List<CardModel> _cardData;
    public int TotalCard => _cardData?.Count ?? 0;

    // initialize the deck with its dependencies
    public void Init()
    {
        _cardData = new List<CardModel>();
    }

    public List<CardModel> GetCollection(int count)
    {
        Debug.Log($"before remove chunk: {_cardData.Count}");
        List<CardModel> dataCollection = new List<CardModel>();
        while (count > 0)
        {
            var data = _cardData[0];
            dataCollection.Add(data);
            _cardData.RemoveAt(0);
            count--;
        }

        Debug.Log($"After remove chunk: {_cardData.Count}");
        return dataCollection;
    }
    
    public CardModel Peek()
    {
        CardModel peekData = null;
        if (_cardData.Count > 0)
        {
            peekData = _cardData[_cardData.Count-1];
            Debug.Log($"[DiscardPile]:Peek Suits: {peekData.Suits} Rank: {peekData.Rank}");
        }else Debug.Log("[DiscardPile]: No more to Peek");
        return peekData;
    }

    // get one card from top of the deck
    public CardModel Pop()
    {
        CardModel popData = null;
        if (_cardData.Count > 0)
        {
            popData = _cardData[_cardData.Count-1];
            Debug.Log($"[DiscardPile]:Pop Suits: {popData.Suits} Rank: {popData.Rank}");
            _cardData.RemoveAt(_cardData.Count-1);
        }else Debug.Log("[DiscardPile]: No more to pop");
        return popData;
    }

    public void Push(CardModel newCardModel)
    {
        // always add at the top
        if (_cardData.Count>0)
        {
            _cardData.Resize(_cardData.Count);
            _cardData.Insert(_cardData.Count, newCardModel);    
        }
        else _cardData.Add(newCardModel);
    }

    public void PushCollection(List<CardModel> newCardModelCollection)
    {
        for (var i = 0; i < newCardModelCollection.Count; i++)
        {
            // always add at the top
            if (_cardData.Count>0)
            {
                _cardData.Resize(_cardData.Count);
                _cardData.Insert(_cardData.Count, newCardModelCollection[i]);    
            }
            else _cardData.Add(newCardModelCollection[i]);
        }
    }

    public void ClearData()
    {
        _cardData.Clear();
    }
}