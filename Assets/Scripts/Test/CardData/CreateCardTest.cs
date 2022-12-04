using System.Collections.Generic;
using UnityEngine;

public class CreateCardTest : MonoBehaviour
{
    public CardSpriteContainer CardContainer;
    public GameObject CardPrefab;
    private CardDeck _cardDeck;
    public List<PlayerCardView> PlayerCardViews;
    private bool _isDeckCreated = false;
    public int cardCount = 12;
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
        _cardDeck.CreateDeck();
        _cardDeck.ShuffleDeck();
        PlayerCardViews[0].Clear();

        var cardDataCollection = _cardDeck.GetCards(cardCount);
        PlayerCardViews[0].Initialize(CardContainer,CardPrefab);
        PlayerCardViews[0].CreateCardViewCollection(cardDataCollection);
        PlayerCardViews[0].InitViewCollectionData(true);
    }
}
