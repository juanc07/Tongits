using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDrawTest : MonoBehaviour
{
    public CardSpriteContainer CardContainer;
    public GameObject CardPrefab;
    private CardDeck _cardDeck;
    public Button DrawButton;
    public Button SortButton;
    public List<PlayerCardView> PlayerCardViews;
    private bool _isDeckCreated = false;
    public int DrawCardLimit = 13;
    [SerializeField]
    private int _cardCount = 0;
    [SerializeField]
    private int _deckCardCount = 0;
    public float initDelay = 0.1f;
    public bool IsSorted = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        DrawButton.enabled = false;
        SortButton.enabled = false;

        _cardDeck = new CardDeck();
        _cardDeck.Init();
        Invoke(nameof(Initialize),initDelay);
    }

    private void Initialize()
    {
        _cardDeck.CreateDeck();
        _cardDeck.ShuffleDeck();
        PlayerCardViews[0].Clear();
        PlayerCardViews[0].Initialize(CardContainer,CardPrefab);
        DrawButton.enabled = true;
        SortButton.enabled = true;
    }

    public void DrawCard()
    {
        if (_cardDeck.TotalCard > 0 )
        {
            if (_cardCount < DrawCardLimit)
            {
                PlayerCardViews[0].ResetToOriginalSort();
                IsSorted = false;
                PlayerCardViews[0].CreateCardView(_cardDeck.Pop());
                _cardCount = PlayerCardViews[0].TotalCard;
            }
            else
            {
                Debug.Log($"reached max card draw {_cardCount}");
            }
        }
        else Debug.Log($"No more card in deck {_cardDeck.TotalCard}");
        _deckCardCount = _cardDeck.TotalCard;
    }
    
    public void SortPlayerHand()
    {
        if (!PlayerCardViews[0].IsAnimating)
        {
            IsSorted = PlayerCardViews[0].Sort();
            PlayerCardViews[0].InitViewCollectionData(false);    
        }
    }

    public void RemoveCard()
    {
        PlayerCardViews[0].ResetToOriginalSort();
        IsSorted = false;
        PlayerCardViews[0].DestroyCardUI();
        _cardCount = PlayerCardViews[0].TotalCard;
    }
}
