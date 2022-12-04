using System.Collections.Generic;
using UnityEngine;

public class CardDataTest : MonoBehaviour
{
    private CardDeck _cardDeck;
    public List<CardModel> cards = new List<CardModel>();
    public CardSpriteContainer CardContainer;
    public List<CardView> targetCards;
    private bool _isDeckCreated = false;
    public bool IsSorted = false;
    public bool IsAscending = false;
    public bool IsCheckSuit = false;
    public bool isLoop;
    public float delay = 3f;
    public float repeatRate = 0.1f;
    // Start is called before the first frame update
    void Awake()
    {
        _cardDeck = new CardDeck();
        _cardDeck.Init();
        if (isLoop) InvokeRepeating(nameof(RandomizeCards),delay,repeatRate); 
        else Invoke(nameof(RandomizeCards),delay);
    }

    private void RandomizeCards()
    {
        if (!_cardDeck.IsDeckCreated)_cardDeck.CreateDeck();
        cards = _cardDeck.GetData();
        
        Debug.Log($"CardLength: {cards.Count}");
        Debug.Log($"index 0 suit: {cards[0].Suits}");
        Debug.Log($"index 0 Rank: {cards[0].Rank}");
        _cardDeck.ShuffleDeck();

        if (IsSorted) {
            if (IsCheckSuit)cards = CardSorter.SortDescendingWithSuits(_cardDeck.GetData());
            cards = CardSorter.SortDescending(_cardDeck.GetData());
        }

        Debug.Log($"Shuffled CardLength: {cards.Count}");

        /*for (var i =0; i < CardDeck.Count; i++)
        {
            Debug.Log($"Shuffled index {i} suit: {CardDeck[i].Suits}");
            Debug.Log($"Shuffled index {i} Rank: {CardDeck[i].Rank}");
        }*/

        
        /*for (var i =0; i < 11; ++i)
        {
            Debug.Log($"Shuffled index {i} suit: {CardDeck[i].Suits}");
            Debug.Log($"Shuffled index {i} Rank: {CardDeck[i].Rank}");
            targetCards[i].Init(CardContainer,CardDeck[i]);
            targetCards[i].ShowFront();
        }*/
        
        for (var i =0; i < targetCards.Count; ++i)
        {
            targetCards[i].Init(CardContainer,cards[i]);
            targetCards[i].ShowFront();
        }
    }
}
