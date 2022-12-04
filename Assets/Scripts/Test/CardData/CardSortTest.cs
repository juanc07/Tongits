using System.Collections.Generic;
using UnityEngine;

public class CardSortTest : MonoBehaviour
{
    public CardSpriteContainer CardContainer;
    public GameObject CardPrefab;
    private CardDeck _cardDeck;
    public List<PlayerCardView> PlayerCardViews;
    public int cardCount = 12;
    private bool _isDeckCreated = false;
    public bool IsSorted = false;
    public float delay = 3f;
    // Start is called before the first frame update
    void Awake()
    {
        _cardDeck = new CardDeck();
        _cardDeck.Init();
        Invoke(nameof(RandomizeCards),delay);
    }

    private void RandomizeCards()
    {
        _cardDeck.CreateDeck();
        _cardDeck.ShuffleDeck();
        PlayerCardViews[0].Clear();

        var cardDataCollection = _cardDeck.GetCards(cardCount);
        PlayerCardViews[0].Initialize(CardContainer,CardPrefab);
        PlayerCardViews[0].CreateCardViewCollection(cardDataCollection);
        PlayerCardViews[0].InitViewCollectionData(true);
    }

    public void SortPlayerHand()
    {
        IsSorted = PlayerCardViews[0].Sort();
        PlayerCardViews[0].InitViewCollectionData(false);
    }
}
