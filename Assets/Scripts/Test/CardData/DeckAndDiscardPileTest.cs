using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckAndDiscardPileTest : MonoBehaviour
{
    public CardSpriteContainer CardContainer;
    public GameObject CardPrefab;
    public Button DrawButton;
    public Button SortButton;
    public CardDeckView CardDeckView;
    public DiscardPileView DiscardPileView;
    public List<PlayerCardView> PlayerCardViews;
    public List<PlayerTableView> PlayerTableViews;

    private bool _isDeckCreated = false;
    public int DrawCardLimit = 13;
    [SerializeField] private int _deckCardCount = 0;
    [SerializeField] private int _discardPileCount = 0;
    public float initDelay = 0.1f;
    public bool IsSorted = false;
    [SerializeField]
    public List<List<CardModel>> combination;

    // Player Tables Cards

    // Other Player Cards

    // TablePot/Jackpot

    // Player Profile UI

    // Start is called before the first frame update
    void Awake()
    {
        DrawButton.enabled = false;
        SortButton.enabled = false;
        Invoke(nameof(Initialize), initDelay);
    }

    void OnDestroy()
    {
        Clear();
    }

    public void Clear()
    {
        CardDeckView.OnCardDeckClick -= ClickCardDeckHandler;
        DiscardPileView.OnDiscardPileClick -= ClickDiscardPileHandler;
    }

    private void Initialize()
    {
        CardDeckView.CreateDeck(true);
        DrawButton.enabled = true;
        SortButton.enabled = true;
        DiscardPileView.Initialize(CardContainer, CardPrefab);
        
        PlayerCardViews[0].Clear();
        PlayerCardViews[0].Initialize(CardContainer, CardPrefab);
        
        var cardDataCollection = CardDeckView.GetCollection(13);
        //cardDataCollection = CardSorter.SortDescendingWithSuits(cardDataCollection);


        //PlayerCardViews[0].CreateCardViewCollection(cardDataCollection);
        //PlayerCardViews[0].Sort();
        //PlayerCardViews[0].InitViewCollectionData(false);
        //PlayerCardViews[0].AnimateSort();
        
                
        PlayerTableViews[0].Clear();
        PlayerTableViews[1].Clear();
        PlayerTableViews[2].Clear();
        
        // Main Player player table view
        //cardDataCollection = CardDeckView.GetCollection(6);
        PlayerTableViews[0].Initialize(CardContainer,CardPrefab);
        //PlayerTableViews[0].InitCreateCardViewCollection(cardDataCollection);
        //PlayerTableViews[0].Sort(true);
        //PlayerTableViews[0].InitViewCollectionData();
        //PlayerTableViews[0].AnimateSort();

        // 2nd player table view
        PlayerTableViews[1].Initialize(CardContainer,CardPrefab);
        // creates 13 card views inside player table view 
        for (var i=0;i < 13;i++)
        {
            PlayerTableViews[1].CreateCardView(CardDeckView.DrawCard());    
        }

        PlayerTableViews[1].Sort();
        PlayerTableViews[1].InitViewCollectionData();
        PlayerTableViews[1].AnimateSort();

        // 3rd player table view
        PlayerTableViews[2].Initialize(CardContainer,CardPrefab);
        
        cardDataCollection = CardDeckView.GetCollection(10);
        Debug.Log($"cardDataCollection: {cardDataCollection}");
        
        PlayerTableViews[2].InitCreateCardViewCollection(cardDataCollection);
        PlayerTableViews[2].Sort(true);
        PlayerTableViews[2].InitViewCollectionData();
        PlayerTableViews[2].AnimateSort();

        CardDeckView.OnCardDeckClick += ClickCardDeckHandler;
        DiscardPileView.OnDiscardPileClick += ClickDiscardPileHandler;
    }

    public void DrawCard()
    {
        if (CardDeckView.TotalCard > 0)
        {
            if (PlayerCardViews[0].TotalCard < DrawCardLimit)
            {
                PlayerCardViews[0].ResetToOriginalSort();
                IsSorted = false;
                var targetNextPosition = PlayerCardViews[0].GetRightMostPosition();

                CardDeckView.DrawCardWithAnimation(
                    PlayerCardViews[0].transform,
                    targetNextPosition, new Vector3(1f, 1f, 1f), 0.3f,
                    (cardModel) =>
                    {
                        PlayerCardViews[0].CreateCardView(cardModel, true);
                    });
            }
            //else Debug.Log($"reached max card draw {_cardCount}");
        }
        else Debug.Log($"No more card in deck {CardDeckView.TotalCard}");

        _deckCardCount = CardDeckView.TotalCard;
    }

    private void DrawFromDiscardPile()
    {
        PlayerCardViews[0].ResetToOriginalSort();
        IsSorted = false;
        var targetNextPosition = PlayerCardViews[0].GetRightMostPosition();

        DiscardPileView.DrawCardWithAnimation(
            targetNextPosition, new Vector3(1f, 1f, 1f), 0.3f,
            (cardModel) =>
            {
                PlayerCardViews[0].CreateCardView(cardModel, false);
                _discardPileCount = DiscardPileView.TotalCard;
            });
    }

    public void SortPlayerHand()
    {
        if (!PlayerCardViews[0].IsAnimating && PlayerCardViews[0].TotalCard > 0)
        {
            IsSorted = PlayerCardViews[0].Sort();
            PlayerCardViews[0].InitViewCollectionData(false);
            PlayerCardViews[0].AnimateSort();
        }
    }

    public void RemoveCard()
    {
        var activeCardViewCollection = PlayerCardViews[0].GetAndRemoveActiveCard();
        PlayerCardViews[0].ResetToOriginalSort(true);
        IsSorted = false;

        var tweenCardCount = 0;
        PlayerCardViews[0].RemoveAndDestroyActiveCardUI(
            DiscardPileView.transform.position,
            new Vector3(0.5f, 0.5f, 0.5f),DiscardPileView.TotalCard,
            0.3f, () =>
            {
                tweenCardCount++;
                if (tweenCardCount == activeCardViewCollection.Count)
                {
                    DiscardPileView.PushAndCreateCardCollection(activeCardViewCollection);
                }

                _discardPileCount = DiscardPileView.TotalCard;
            });
    }

    public void MendCard()
    {
        var activeCardViewCollection = PlayerCardViews[0].GetAndRemoveActiveCard();
        PlayerCardViews[0].ResetToOriginalSort(true);
        IsSorted = false;

        var tweenCardCount = 0;
        PlayerCardViews[0].RemoveAndDestroyActiveCardUI(
            PlayerTableViews[0].GetLastCardPosition(),
            new Vector3(0.5f, 0.5f, 0.5f), PlayerCardViews[0].TotalCard,
            0.3f, () =>
            {
                tweenCardCount++;
                if (tweenCardCount == activeCardViewCollection.Count)
                {
                    PlayerTableViews[0].CreateCardViewCollection(activeCardViewCollection);
                    //DiscardPileView.PushAndCreateCardCollection(activeCardViewCollection);
                }
            });
    }

    private void ClickCardDeckHandler()
    {
        DrawCard();
    }

    private void ClickDiscardPileHandler()
    {
        DrawFromDiscardPile();
    }
}