using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardDeckView : MonoBehaviour
{
    [SerializeField]
    private CardDeck _cardDeck;
    public CardSpriteContainer CardContainer;
    public GameObject _cardPrefab;
    public Ease easeType;
    private GameObject _card;
    private CardView _cardView;
    private SpriteRenderer _cardRender;

    // events
    private Action CardDeckClick;

    public event Action OnCardDeckClick
    {
        add { CardDeckClick += value; }
        remove { CardDeckClick -= value; }
    }

    private TextMeshPro _numberText;
    private SpriteButton _spriteButton;
    private bool _isAnimating = false;

    public bool IsAnimating => _isAnimating;
    public int TotalCard => _cardDeck.TotalCard;

    void Awake()
    {
        DOTween.Init(true, true, null);   
    }

    // Start is called before the first frame update
    void Start()
    {
        _cardDeck = new CardDeck();
        _cardDeck.Init();
        
        _numberText = GetComponentInChildren<TextMeshPro>();
        _numberText.text = "";

        _spriteButton = GetComponentInChildren<SpriteButton>();
        _spriteButton.OnClickSprite += ClickSpriteHandler;
    }

    void OnDestroy()
    {
        Clear();
    }

    public void Clear()
    {
        _cardDeck.ClearData();
        _spriteButton.OnClickSprite -= ClickSpriteHandler;
        _spriteButton = null;
    }

    public void CreateDeck(bool isShuffle)
    {
        _cardDeck.CreateDeck();
        if (isShuffle)_cardDeck.ShuffleDeck();
        SetTextDisplay(_cardDeck.TotalCard.ToString());
    }
    
    public List<CardModel> GetCollection(int total)
    {
        var cards = _cardDeck.GetCards(total);
        SetTextDisplay(_cardDeck.TotalCard.ToString());
        return cards;
    }

    public CardModel DrawCard()
    {
        var newCardData = _cardDeck.Pop();
        SetTextDisplay(_cardDeck.TotalCard.ToString());
        return newCardData; 
    }
    
    public void DrawCardWithAnimation(Transform targetView,
        Vector3 targetPosition,Vector3 targetScale,float duration, Action<CardModel> onAnimationComplete)
    {
        if (!IsAnimating)
        {
            _isAnimating = true;
            var newCardData = _cardDeck.Pop();
            CreateCard(newCardData);
            MoveCard(
                targetView,
                targetPosition,
                targetScale,
                duration, ()=>
                {
                    SetTextDisplay(_cardDeck.TotalCard.ToString());
                    onAnimationComplete?.Invoke(newCardData);
                });    
        }
    }

    private void SetTextDisplay(string value)
    {
        _numberText.text = value;
    }

    public void CreateCard(CardModel cardData)
    {
        if (!_card)
        {
            _card = Instantiate(_cardPrefab, this.transform, true);
            _card.transform.position = transform.position;
            _card.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            _cardView = _card.GetComponent<CardView>();
            _card.GetComponent<BoxCollider2D>().enabled = false;
            _cardRender = _card.GetComponent<SpriteRenderer>();
            _cardRender.color = new Color(1f, 1f, 1f, 1f);
            _cardView.Init(CardContainer, cardData);
            _cardView.ShowBack();
        }
        else
        {
            _cardView.UpdateData(cardData);
            _cardView.ShowBack();
        }
    }

    public void MoveCard(Transform targetView, Vector3 targetPosition, Vector3 targetScale, float duration,
        TweenCallback onAnimationComplete)
    {
        _cardView.ShowBack();
        // reset back the card alpha to normal
        _cardRender.color = new Color(1f, 1f, 1f, 1f);
        _card.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), duration)
            .SetEase(easeType);

        // enlarging the card from scaled down version
        _card.transform.DOScale(targetScale, duration).SetEase(easeType)
            .OnComplete(() =>
            {
                _cardRender.sortingOrder = targetView.childCount;
                // flip card animation 
                _card.transform.DOScale(new Vector3(0f, 1f, 1f), duration).SetEase(easeType).OnUpdate(() =>
                {
                    // show the actual card front when scale x is 0.25 or lower
                    if (_card.transform.localScale.x <= 0.25f) _cardView.ShowFront();
                }).OnComplete(() =>
                {
                    // reset back to the card deck view position
                    _card.transform.position = transform.position;
                    // reset back to scaled down card version
                    _card.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    // hiding the card again by changing alpha to zero
                    _card.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    
                    _isAnimating = false;
                    onAnimationComplete();
                });
            });
    }

    private void ClickSpriteHandler()
    {
        // trigger draw card here
        CardDeckClick?.Invoke();
    }
}