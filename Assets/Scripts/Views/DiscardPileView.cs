using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DiscardPileView : MonoBehaviour
{
    public Ease easeType;
    public CardView StaticCardView;
    private DiscardPile _discardPile;
    private bool _isAnimating = false;
    private CardSpriteContainer _cardContainer;
    private GameObject _cardPrefab;
    [SerializeField] private int _totalCard = 0;

    private GameObject _card;
    private CardView _cardView;
    private SpriteRenderer _cardRender;
    private SpriteRenderer _staticCardRenderer;

    // events
    //private Action<GameObject> DiscardPileClick;
    private Action DiscardPileClick;

    //public event Action<GameObject> OnDiscardPileClick
    public event Action OnDiscardPileClick
    {
        add { DiscardPileClick += value; }
        remove { DiscardPileClick -= value; }
    }

    private SpriteButton _spriteButton;

    public int TotalCard
    {
        get
        {
            _totalCard = _discardPile.TotalCard;
            return _totalCard;
        }
    }

    public bool IsAnimating => _isAnimating;

    // Start is called before the first frame update
    void Start()
    {
        _discardPile = new DiscardPile();
        _discardPile.Init();

        _spriteButton = GetComponentInChildren<SpriteButton>();
        _spriteButton.OnClickSprite += ClickSpriteHandler;
    }

    void OnDestroy()
    {
        Clear();
    }

    public void Initialize(CardSpriteContainer cardContainer, GameObject cardPrefab)
    {
        _cardContainer = cardContainer;
        _cardPrefab = cardPrefab;
        _staticCardRenderer = StaticCardView.GetComponent<SpriteRenderer>();
        UpdateNext();
    }

    public void Clear()
    {
        _discardPile.ClearData();
        _spriteButton.OnClickSprite -= ClickSpriteHandler;
        _spriteButton = null;
    }

    public void PushCardCollection(List<CardModel> cardDataCollection)
    {
        _discardPile.PushCollection(cardDataCollection);
    }

    public void PushAndCreateCardCollection(List<CardModel> cardDataCollection)
    {
        _discardPile.PushCollection(cardDataCollection);
        CreateCard(cardDataCollection[cardDataCollection.Count - 1]);
    }

    public void PushCard(CardModel cardData)
    {
        _discardPile.Push(cardData);
    }

    public void PushAndCreateCard(CardModel cardData)
    {
        _discardPile.Push(cardData);
        CreateCard(cardData);
    }

    private void CreateCard(CardModel cardData)
    {
        if (!_card)
        {
            StaticCardView.Init(_cardContainer, cardData);
            StaticCardView.ShowFront();
            
            // we needed to modify the z because we have an blocking click issue when clicking cards
            _card = Instantiate(_cardPrefab, this.transform, false);
            _card.transform.position = transform.position;
            _card.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            _cardView = _card.GetComponent<CardView>();
            _card.GetComponent<BoxCollider2D>().enabled = false;
            _cardRender = _card.GetComponent<SpriteRenderer>();
            _cardRender.color = new Color(1f, 1f, 1f, 1f);
            _cardView.Init(_cardContainer, cardData);
            _cardView.ShowFront();
        }
        else UpdateNext();
    }

    private void UpdateNext()
    {
        if (_discardPile.TotalCard == 0) {
            // nothing to show hide the card
            _staticCardRenderer.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            // there's some card to show, so show it
            _staticCardRenderer.color = new Color(1f, 1f, 1f, 1f);
            StaticCardView.UpdateData(_discardPile.Peek());
            StaticCardView.ShowFront();
        }
    }

    private void ResetCardView()
    {
        if (_card)
        {
            _card.transform.position = transform.position;
            _card.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _cardRender.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    public void DrawCardWithAnimation(Vector3 targetPosition, Vector3 targetScale,
        float duration, Action<CardModel> onAnimationComplete)
    {
        if (!IsAnimating && _discardPile.TotalCard > 0 )
        {
            Debug.Log($"DrawCardWithAnimation1");
            _isAnimating = true;
            _cardRender.color = new Color(1f, 1f, 1f, 1f);
            var data = _discardPile.Pop();
            _totalCard = _discardPile.TotalCard;
            UpdateNext();

            _card.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), duration)
                .SetEase(easeType);

            // enlarging the card from scaled down version
            _card.transform.DOScale(targetScale, duration).SetEase(easeType)
                .OnComplete(() =>
                {
                    _isAnimating = false;
                    ResetCardView();
                    onAnimationComplete?.Invoke(data);
                });
        }
    }

    private void ClickSpriteHandler()
    {
        DiscardPileClick?.Invoke();
    }
}