using System;
using UnityEngine;

public class CardView : MonoBehaviour
{
    public enum CardViewType
    {
        PlayerCard,
        TableCard
    }
    
    private SpriteRenderer _renderer;
    [SerializeField]
    private CardModel _data;
    private CardSpriteContainer _cardContainer;
    private bool _isInitialize = false;
    [SerializeField]
    private bool _isActive = false;
    public bool IsInitialize => _isInitialize;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public CardModel Data => _data;
    private SpriteButton _spriteButton;
    private CardViewType _cardViewType;
    
    // events
    private Action<CardModel,bool> SelectCard;
    public event Action<CardModel,bool> OnSelectCard {
        add { SelectCard += value; }
        remove { SelectCard -= value; }
    }
    
    private Action ClickCard;
    public event Action OnClickCard{
        add { ClickCard += value; }
        remove { ClickCard -= value; }
    }

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _spriteButton = GetComponent<SpriteButton>();
        _spriteButton.OnClickSprite += ClickSpriteHandler;
    }
    
    void OnDestroy()
    {
        Clear();
    }

    public void Clear()
    {
        _renderer = null;
        _spriteButton.OnClickSprite -= ClickSpriteHandler;
        _spriteButton = null;
    }

    public void Init(CardSpriteContainer cardContainer,CardModel data)
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        _cardContainer = cardContainer;
        _data = data;
        _isInitialize = true;
    }
    
    public void UpdateData(CardModel data)
    {
        _data = data;
    }
    
    public void ShowFront()
    {
        if (_renderer == null) {
            Debug.LogError("ShowFront CardView Sprite Renderer is null!");
            return;
        }
        //Debug.Log($"ShowFront check: cards {_cardContainer.cards[_data.Rank].GetValue(_data.Suits)}");
        _renderer.sprite = (Sprite) _cardContainer.cards[_data.Rank].GetValue(_data.Suits);
    }
    
    public void ShowBack()
    {
        if (_renderer == null) {
            Debug.LogError("ShowBack CardView Sprite Renderer is null!");
            return;
        }
        
        _renderer.sprite = _cardContainer.cardBack;
    }

    public void ToggleActive()
    {
        if (_cardViewType == CardViewType.PlayerCard)
        {
            _isActive = !_isActive;
            SelectCard?.Invoke(_data,_isActive);    
        }else ClickCard?.Invoke();
    }

    public void SetCardViewType(CardViewType cardViewType)
    {
        _cardViewType = cardViewType;
    }
    
    private void ClickSpriteHandler()
    {
        ToggleActive();
    }
}
