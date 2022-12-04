using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerTableView : MonoBehaviour
{
    public UIAnchorHelper.AnchorType anchorType;
    
    // Basic things
    [SerializeField] private PlayerCard _data;
    [SerializeField] private List<GameObject> _cardViews;
    private CardSpriteContainer _cardContainer;
    private GameObject _cardPrefab;
    private List<Vector3> _cardPositions;

    // card local scale
    public float Size = 0.5f;
    
    // Placement positioning
    public Vector3 start;
    public float cardOffset = 0.2f;
    public Vector3 AnchorOffset;
    private float _cardWidth = 0f;
    private float _selectCardOffsetY = 0.3f;

    // Animations
    public Ease TweenEaseType;
    public int loop;
    public float SortTweenDuration = 0.3f;
    
    private int _tweenCountCompleted = 0;
    private int _tweenCountTotal = 0;
    private bool _isAnimating = false;
    public int TotalCard => _data.TotalCard;
    
    public void Initialize(CardSpriteContainer cardContainer, GameObject cardPrefab)
    {
        DOTween.Init(true, true, null);

        var cardPrefabRenderer = cardPrefab.GetComponent<SpriteRenderer>();
        _cardWidth = cardPrefabRenderer.bounds.size.x;
        Debug.Log($"_cardWidth: {_cardWidth}");
        _cardWidth = _cardWidth * Size;

        _data = new PlayerCard();
        _cardViews = new List<GameObject>();
        _cardPositions = new List<Vector3>();

        _cardContainer = cardContainer;
        _cardPrefab = cardPrefab;

        //UpdateAnchor(0);
        ReAnchorContainer();
    }

    public void Clear()
    {
        _data.Cards?.Clear();
        _cardViews?.Clear();
        _cardPositions?.Clear();
        RemoveAllCardViews();
        _data.TotalPoints = 0;
    }

    /// <summary>
    /// Creates one CardView inside Player Table Card View
    /// </summary>
    /// <param name="cardData"></param>
    public void CreateCardView(CardModel cardData)
    {
        if (cardData == null)
        {
            Debug.LogError($"[PlayerCardView:CreateCardView] cardData is null!");
            return;
        }

        var createCardObject= CreateCardUI(cardData);

        if (_data.Cards.Count > 0)
        {
            _data.Cards.Resize(_data.Cards.Count);
            _cardViews.Resize(_cardViews.Count);
            _cardPositions.Resize(_cardPositions.Count);

            _data.Cards.Insert(_data.Cards.Count, createCardObject.CardData);
            _cardViews.Insert(_cardViews.Count, createCardObject.CardObject);
            _cardPositions.Insert(_cardPositions.Count, createCardObject.CardPosition);
        }
        else
        {
            _data.Cards.Add(createCardObject.CardData);
            _cardViews.Add(createCardObject.CardObject);
            _cardPositions.Add(createCardObject.CardPosition);
        }

        _data.TotalCard = _data.Cards.Count;
        _data.TotalPoints =  TongitsHelper.ComputeTotalScore(_data.Cards);
        AnimateReAnchorContainer();
    }

    /// <summary>
    /// Creates Player Table Card Views using collection of Card Models
    /// Perfect for initialization of 2 or more data model at once 
    /// </summary>
    /// <param name="cardModels"></param>
    public void InitCreateCardViewCollection(List<CardModel> cardModels)
    {
        if (cardModels == null)
        {
            Debug.LogError($"[PlayerCardView:InitCreateCardViewCollection] cardModels is null!");
            return;
        }

        // set the collection of new card data model collection
        _data.Cards = cardModels;

        for (var i = 0; i < _data.Cards.Count; i++)
        {
            var createCardObject= CreateCardUI(_data.Cards[i]);

            if (_cardViews.Count > 0)
            {
                _cardViews.Resize(_cardViews.Count);
                _cardPositions.Resize(_cardPositions.Count);

                _cardViews.Insert(_cardViews.Count, createCardObject.CardObject);
                _cardPositions.Insert(_cardPositions.Count, createCardObject.CardPosition);
            }
            else
            {
                _cardViews.Add(createCardObject.CardObject);
                _cardPositions.Add(createCardObject.CardPosition);
            }
        }

        _data.TotalCard = _data.Cards.Count;
        _data.TotalPoints =  TongitsHelper.ComputeTotalScore(_data.Cards);
        AnimateReAnchorContainer();
    }

    public void CreateCardViewCollection(List<CardModel> cardModels)
    {
        if (cardModels == null)
        {
            Debug.LogError($"[PlayerCardView:InitCreateCardViewCollection] cardModels is null!");
            return;
        }

        for (var i = 0; i < cardModels.Count; i++ )
        {
            CreateCardView(cardModels[i]);
        }
    }
    
    public void InitViewCollectionData()
    {
        if (_data.Cards.Count > 0)
        {
            _cardPositions.Clear();
            for (var i = 0; i < _data.Cards.Count; i++)
            {
                if (_cardViews[i] != null)
                {
                    var cardView = _cardViews[i].GetComponent<CardView>();
                    if (cardView.IsActive)
                    {
                        // reset the active check to false again
                        cardView.IsActive = false;
                        // move down the card view if its actively selected
                        cardView.transform.localPosition = new Vector3(cardView.transform.localPosition.x,
                            0, cardView.transform.localPosition.z);
                    }

                    if (!cardView.IsInitialize) cardView.Init(_cardContainer, _data.Cards[i]);
                    else cardView.UpdateData(_data.Cards[i]);

                    cardView.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
                    cardView.ShowFront();
                    _cardPositions.Add(cardView.transform.localPosition);
                }
            }
            AnimateReAnchorContainer();
        }
    }
    
    public void AnimateSort()
    {
        if (!_isAnimating && _cardViews.Count > 0)
        {
            SetAllPositionToCenter();

            _isAnimating = true;
            _tweenCountTotal = _cardViews.Count;
            _tweenCountCompleted = 0;

            for (var i = _cardViews.Count - 1; i >= 0; i--)
            {
                if (_cardViews[i].transform != null)
                {
                    _cardViews[i].transform.DOLocalMove(_cardPositions[i], SortTweenDuration)
                        .SetEase(TweenEaseType)
                        .OnComplete(() =>
                        {
                            _tweenCountCompleted++;
                            if (_tweenCountCompleted == _tweenCountTotal)
                            {
                                _isAnimating = false;
                            }

                            _cardViews[i].transform.DOKill();
                        });
                }
            }
        }
    }
    
    public void Sort(bool isReverse=false)
    {
        if (_data.Cards?.Count > 0)
        {
            _data.Cards = CardSorter.SortDescending(_data.Cards);
            if(isReverse) _data.Cards.Reverse();
        }
    }

    public Vector3 GetLastCardPosition()
    {
        return _cardPositions.Count > 0 ? transform.TransformPoint(_cardPositions[^1]) : transform.position;
    }

    private CreateCardModel CreateCardUI(CardModel cardData)
    {
        float co = cardOffset * _cardViews?.Count ?? 0;
        // we needed to modify the z because we have an blocking click issue when clicking cards
        Vector3 tempPosition = start + new Vector3(co, 0f, ((_cardViews?.Count ?? 1f) * 0.01f) * -1f);
        GameObject cardCopy = Instantiate(_cardPrefab, this.transform, true);
        cardCopy.transform.localPosition = tempPosition;
        cardCopy.transform.localScale = new Vector3(Size,Size,Size);

        var cardView = cardCopy.GetComponent<CardView>();
        cardView.SetCardViewType(CardView.CardViewType.TableCard);
        cardView.Init(_cardContainer, cardData);

        var cardSpriteRender = cardView.GetComponent<SpriteRenderer>();
        cardSpriteRender.sortingOrder = (_cardViews?.Count ?? 0) + 1;
        
        cardView.ShowFront();
        cardView.OnClickCard += ClickCardHandler;

        return new CreateCardModel(cardCopy,cardData,tempPosition);
    }
    
    // This is the reason why the playerCard will anchor on the exact x and y position no matter what screen size
    private void UpdateAnchor(float targetX)
    {
        
        var newPos = UIAnchorHelper.GetAnchorType(anchorType) +
                     new Vector3(targetX + AnchorOffset.x, AnchorOffset.y, AnchorOffset.z);

        if (!transform.position.Equals(newPos))
            transform.position = newPos;
    }

    private void RemoveAllCardViews()
    {
        foreach (Transform child in transform)
        {
            var cardView = child.GetComponent<CardView>();
            cardView.OnClickCard -= ClickCardHandler;
            Destroy(child.gameObject);
        }
    }
    
    /// <summary>
    /// ReAnchor the player View Container
    /// </summary>
    private void ReAnchorContainer()
    {
        var totalWidth = SpriteRendererHelper.GetTotalChildWidth(transform);
        var targetX =0f;
        if (anchorType == UIAnchorHelper.AnchorType.TopLeft)
        {
            targetX = _cardWidth * 0.5f;    
        }else if (anchorType == UIAnchorHelper.AnchorType.TopRight)
        {
            targetX = (totalWidth * 0.5f) * -1f;
        }else if (anchorType == UIAnchorHelper.AnchorType.MiddleCenter)
        {
            targetX = _cardWidth * 0.5f;
        }

        UpdateAnchor(targetX);
    }

    /// <summary>
    /// Tween and ReAnchor the player View Container
    /// </summary>
    private void AnimateReAnchorContainer()
    {
        var totalWidth = SpriteRendererHelper.GetTotalChildWidth(transform);
        var targetX =0f;
        if (anchorType == UIAnchorHelper.AnchorType.TopLeft)
        {
            targetX = _cardWidth * 0.5f;    
        }else if (anchorType == UIAnchorHelper.AnchorType.TopRight)
        {
            targetX = (totalWidth * 0.5f) * -1f;
        }else if (anchorType == UIAnchorHelper.AnchorType.MiddleCenter)
        {
            targetX = _cardWidth * 0.5f;
        }

        var newPos =  UIAnchorHelper.GetAnchorType(anchorType) +
                     new Vector3(targetX + AnchorOffset.x, AnchorOffset.y, AnchorOffset.z);

        if (!transform.position.Equals(newPos))
        {
            transform.DOMove(newPos, 0.1f)
                .SetEase(TweenEaseType);
        }
    }
    
    private void SetAllPositionToCenter()
    {
        if (_cardPositions.Count >= 2)
        {
            var targetIndex = 0;
            if (_cardPositions.Count % 2 == 0) targetIndex = _cardPositions.Count / 2;
            else targetIndex = (_cardPositions.Count / 2) + 1;

            var targetPos = _cardPositions[targetIndex];
            targetPos.x = (_cardPositions[targetIndex].x * 0.5f);

            foreach (var t in _cardViews)
            {
                t.transform.localPosition = targetPos;
            }
        }
    }
    
    private void ClickCardHandler()
    {
        Sort();
        InitViewCollectionData();
        AnimateSort();
    }
}
