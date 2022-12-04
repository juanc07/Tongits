using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerCardView : MonoBehaviour
{
    // Basic things
    [SerializeField] private PlayerCard _data;
    [SerializeField] private List<GameObject> _cardViews;
    private CardSpriteContainer _cardContainer;
    private GameObject _cardPrefab;
    private List<CardModel> _originalCardModel;
    private bool _isSorted = false;
    private List<Vector3> _cardPositions;

    // Placement positioning
    public Vector3 start;
    public float cardOffset = 0.2f;
    public Vector3 AnchorOffset;
    private float _cardWidth = 0f;
    private float _selectCardOffsetY = 0.3f;

    // Animations
    public bool IsAnimateSort = false;
    public Ease easeType;
    public int loop;
    private int _tweenCountCompleted = 0;
    private int _tweenCountTotal = 0;
    private bool _isAnimating = false;
    public float SortTweenDuration = 0.7f; 
    public bool IsAnimating => _isAnimating;
    public int TotalCard => _data.TotalCard;
    public int TotalPoints => _data.TotalPoints;
    
    public void Initialize(CardSpriteContainer cardContainer, GameObject cardPrefab)
    {
        DOTween.Init(true, true, null);

        var cardPrefabRenderer = cardPrefab.GetComponent<SpriteRenderer>();
        _cardWidth = cardPrefabRenderer.bounds.size.x;

        _data = new PlayerCard();
        _cardViews = new List<GameObject>();
        _cardPositions = new List<Vector3>();

        _cardContainer = cardContainer;
        _cardPrefab = cardPrefab;

        UpdateAnchor(0);
    }

    public void Clear()
    {
        _data.Cards?.Clear();
        _cardViews?.Clear();
        _cardPositions?.Clear();
        _originalCardModel?.Clear();
        RemoveAllCardViews();
        _data.TotalPoints = 0;
    }

    public List<CardModel> GetAndRemoveActiveCard()
    {
        List<CardModel> popDataCollection = new List<CardModel>();

        for (var i = _cardViews.Count - 1; i >= 0; i--)
        {
            CardView cardView = _cardViews[i].GetComponent<CardView>();
            if (cardView.IsActive)
            {
                // get and push the active data
                popDataCollection.Add(cardView.Data);

                // remove the active card from the original card model for sorting
                for (var j = 0; j < _originalCardModel.Count; j++)
                {
                    if (_originalCardModel[j].Suits == cardView.Data.Suits
                        && _originalCardModel[j].Rank == cardView.Data.Rank)
                    {
                        _originalCardModel.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        return popDataCollection;
    }

    // Destroy Single CardView from the bottom
    public void DestroyCardUI()
    {
        CardModel popData = null;
        if (_cardViews.Count > 0)
        {
            var cardView = _cardViews[0].GetComponent<CardView>();
            cardView.OnSelectCard -= SelectCardHandler;

            _cardViews[0].gameObject.transform.parent = null;
            Destroy(_cardViews[0].gameObject);
            _data.Cards.RemoveAt(0);
            _cardViews.RemoveAt(0);

            _data.TotalPoints =  TongitsHelper.ComputeTotalScore(_data.Cards);
            _data.TotalCard = _data.Cards.Count;
            
            AnimateRearrangeUICollection();
        }
    }

    /// <summary>
    /// Will remove the data card and destroy the actual card game object
    /// </summary>
    /// <param name="targetView"></param>
    /// <param name="targetScale"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public void RemoveAndDestroyActiveCardUI(Vector3 targetPosition,Vector3 targetScale,int targetCardCount
        ,float duration, Action animationComplete)
    {
        for (var i = _cardViews.Count - 1; i >= 0; i--)
        {
            var cardView = _cardViews[i].GetComponent<CardView>();
            if (cardView.IsActive)
            {
                // un parent the game object
                cardView.gameObject.transform.parent = null;
                cardView.OnSelectCard -= SelectCardHandler;
                MoveCardAndDestroy(cardView.gameObject, targetPosition, targetScale,targetCardCount, duration, animationComplete);
                // remove the card view from the card view collection
                _cardViews.RemoveAt(i);
                cardView.IsActive = false;
            }
        }

        _data.TotalPoints =  TongitsHelper.ComputeTotalScore(_data.Cards);
        _data.TotalCard = _data.Cards.Count;
        
        AnimateRearrangeUICollection();
    }
    
    public Vector3 GetRightMostPosition()
    {
        if (_cardViews.Count <= 0) return transform.position;
        float co = cardOffset * _cardViews?.Count ?? 0;
        // we needed to modify the z because we have an blocking click issue when clicking cards
        Vector3 tempPosition = start + new Vector3(co, 0f, ((_cardViews?.Count ?? 1f) * 0.01f) * -1f);
        return transform.TransformPoint(tempPosition);
    }

    public void CreateCardView(CardModel cardData, bool isAnimateTransparency = false)
    {
        if (cardData == null)
        {
            Debug.LogError($"[PlayerCardView:CreateCardView] cardData is null!");
            return;
        }

        ResetActiveCardView();

        float co = cardOffset * _cardViews?.Count ?? 0;
        // we needed to modify the z because we have an blocking click issue when clicking cards
        Vector3 tempPosition = start + new Vector3(co, 0f, ((_cardViews?.Count ?? 1f) * 0.01f) * -1f);
        GameObject cardCopy = Instantiate(_cardPrefab, this.transform, true);
        cardCopy.transform.localPosition = tempPosition;

        var cardView = cardCopy.GetComponent<CardView>();
        cardView.SetCardViewType(CardView.CardViewType.PlayerCard);
        cardView.Init(_cardContainer, cardData);

        var cardSpriteRender = cardView.GetComponent<SpriteRenderer>();
        cardSpriteRender.sortingOrder = (_cardViews?.Count ?? 0) + 1;
        // hide it 1st, if you wanted to animate transparency
        if (isAnimateTransparency) cardSpriteRender.color = new Color(1f, 1f, 1f, 0f);
        cardView.ShowFront();
        cardView.OnSelectCard += SelectCardHandler;

        if (_data.Cards.Count > 0)
        {
            _data.Cards.Resize(_data.Cards.Count);
            _cardViews.Resize(_cardViews.Count);
            _cardPositions.Resize(_cardPositions.Count);

            _data.Cards.Insert(_data.Cards.Count, cardData);
            _cardViews.Insert(_cardViews.Count, cardCopy);
            _cardPositions.Insert(_cardPositions.Count, tempPosition);
        }
        else
        {
            _data.Cards.Add(cardData);
            _cardViews.Add(cardCopy);
            _cardPositions.Add(tempPosition);
        }

        if (isAnimateTransparency)
            AnimateTransparency(cardCopy);

        _data.TotalPoints =  TongitsHelper.ComputeTotalScore(_data.Cards);
        _data.TotalCard = _data.Cards.Count;
        
        AnimateRecenterContainer();
        UpdateOriginalSortData();
    }

    public void CreateCardViewCollection(List<CardModel> cardModels, bool isAnimateTransparency = false)
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
            var co = cardOffset * i;

            // we needed to modify the z because we have an blocking click issue when clicking cards
            Vector3 tempPosition = start + new Vector3(co, 0f, ((_cardViews?.Count ?? 1f) * 0.01f) * -1f);
            GameObject cardCopy = Instantiate(_cardPrefab, this.transform, true);
            cardCopy.transform.localPosition = tempPosition;

            var cardView = cardCopy.GetComponent<CardView>();
            cardView.SetCardViewType(CardView.CardViewType.PlayerCard);
            cardView.Init(_cardContainer, _data.Cards[i]);

            var cardSpriteRender = cardView.GetComponent<SpriteRenderer>();
            cardSpriteRender.sortingOrder = (_cardViews?.Count ?? 0) + 1;

            // hide it 1st, if you wanted to animate transparency
            if (isAnimateTransparency)
                cardSpriteRender.color = new Color(1f, 1f, 1f, 0f);

            cardView.ShowFront();
            cardView.OnSelectCard += SelectCardHandler;

            if (_cardViews.Count > 0)
            {
                _cardViews.Resize(_cardViews.Count);
                _cardPositions.Resize(_cardPositions.Count);

                _cardViews.Insert(_cardViews.Count, cardCopy);
                _cardPositions.Insert(_cardPositions.Count, tempPosition);
            }
            else
            {
                _cardViews.Add(cardCopy);
                _cardPositions.Add(tempPosition);
            }

            if (isAnimateTransparency)
                AnimateTransparency(cardCopy);
        }

        _data.TotalPoints =  TongitsHelper.ComputeTotalScore(_data.Cards);
        _data.TotalCard = _data.Cards.Count;
        
        AnimateRecenterContainer();
        UpdateOriginalSortData();
    }
    
    public void InitViewCollectionData(bool isNew)
    {
        if (_data.Cards.Count > 0)
        {
            ResetActiveCardView();
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

            // updates the original card model for sorting
            if (isNew) _originalCardModel = new List<CardModel>(_data.Cards);
            RecenterContainer();
        }
    }

    public bool SortWithSuit()
    {
        if (!_isSorted)
        {
            Debug.Log("sort with suit and Rank");
            _data.Cards = CardSorter.SortDescending(_data.Cards);
            _data.Cards = CardSorter.SortDescendingWithSuits(_data.Cards);
            _isSorted = true;
        }
        else
        {
            Debug.Log("revert old sort");
            ResetToOriginalSort();
            _isSorted = false;
        }

        return _isSorted;
    }

    public bool Sort()
    {
        if (_data.Cards.Count > 0)
        {
            if (!_isSorted)
            {
                _data.Cards = CardSorter.SortDescending(_data.Cards);
                _isSorted = true;
            }
            else
            {
                Debug.Log("revert old sort");
                ResetToOriginalSort();
                _isSorted = false;
            }
        }
        else _isSorted = false;

        return _isSorted;
    }

    /// <summary>
    /// Only call this to undo the sorting of cards
    /// </summary>
    public void ResetToOriginalSort(bool forceUpdate = false)
    {
        if (_isSorted || forceUpdate)
        {
            _data.Cards = new List<CardModel>(_originalCardModel);
            Debug.Log($"reset original sort data success new data count: {_data.Cards.Count}");
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
                        .SetEase(easeType)
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

    public void AnimateSortUsingSequence()
    {
        if (!_isAnimating && _cardViews.Count > 0)
        {
            SetAllPositionToCenter();
            var s = DOTween.Sequence();

            _isAnimating = true;
            _tweenCountTotal = _cardViews.Count;
            _tweenCountCompleted = 0;

            for (var i = _cardViews.Count - 1; i >= 0; i--)
            {
                if (_cardViews[i].transform != null)
                {
                    s.Append(_cardViews[i].transform.DOLocalMove(_cardPositions[i], 0.3f)
                        .SetEase(easeType)
                        .OnComplete(() =>
                        {
                            _tweenCountCompleted++;
                            if (_tweenCountCompleted == _tweenCountTotal)
                            {
                                _isAnimating = false;
                            }

                            _cardViews[i].transform.DOKill();
                        }));
                }
            }

            s.Play();
        }
    }

    private void RemoveAllCardViews()
    {
        foreach (Transform child in transform)
        {
            var cardView = child.GetComponent<CardView>();
            cardView.OnSelectCard -= SelectCardHandler;
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Rearrange the card view placement
    /// </summary>
    private void RearrangeUICollection()
    {
        start.x = 0;
        for (var i = 0; i < _data.Cards.Count; i++)
        {
            if (_cardViews[i] != null)
            {
                float co = cardOffset * i;
                // we needed to modify the z because we have an blocking click issue when clicking cards 
                Vector3 tempPosition = start + new Vector3(co, 0f, (i * 0.01f) * -1f);
                _cardViews[i].transform.localPosition = tempPosition;
                _cardViews[i].GetComponent<SpriteRenderer>().sortingOrder = i + 1;
            }
        }

        RecenterContainer();
    }

    /// <summary>
    /// Tween and Rearrange the card view placement
    /// </summary>
    private void AnimateRearrangeUICollection()
    {
        start.x = 0;
        for (var i = 0; i < _cardViews.Count; i++)
        {
            if (_cardViews[i] != null)
            {
                float co = cardOffset * i;
                // we needed to modify the z because we have an blocking click issue when clicking cards 
                Vector3 tempPosition = start + new Vector3(co, 0f, (i * 0.01f) * -1f);
                _cardViews[i].GetComponent<SpriteRenderer>().sortingOrder = i + 1;

                _cardViews[i].transform.DOLocalMove(tempPosition, 0.3f)
                    .SetEase(easeType);
            }
        }

        RecenterContainer();
    }

    /// <summary>
    /// Update the original data for sorting purpose
    /// Best to call this when adding a new data 
    /// </summary>
    private void UpdateOriginalSortData()
    {
        _originalCardModel = new List<CardModel>(_data.Cards);
    }

    /// <summary>
    /// Tween and Recenter the player View Container
    /// </summary>
    private void AnimateRecenterContainer()
    {
        var totalWidth = SpriteRendererHelper.GetTotalChildWidth(transform);
        // percentage of target width based on offset x
        var targetOffset = cardOffset / _cardWidth;
        // get the half of the targetPercentage
        var widthPercent = targetOffset * 0.5f;
        // compute the extra offset for x based on width percent
        var widthOffset = _cardWidth * widthPercent;
        var centerX = ((totalWidth * widthPercent) - widthOffset) * -1f;

        var newPos = CameraViewportHandler.Instance.BottomCenter +
                     new Vector3(centerX + AnchorOffset.x, AnchorOffset.y, AnchorOffset.z);

        if (!transform.position.Equals(newPos))
        {
            transform.DOMove(newPos, 0.3f)
                .SetEase(easeType)
                .SetLoops(loop);
        }
    }

    /// <summary>
    /// Recenter the player View Container
    /// </summary>
    private void RecenterContainer()
    {
        var totalWidth = SpriteRendererHelper.GetTotalChildWidth(transform);
        // percentage of target width based on offset x
        var targetOffset = cardOffset / _cardWidth;
        // get the half of the targetPercentage
        var widthPercent = targetOffset * 0.5f;
        // compute the extra offset for x based on width percent
        var widthOffset = _cardWidth * widthPercent;
        var centerX = ((totalWidth * widthPercent) - widthOffset) * -1f;

        UpdateAnchor(centerX);
    }

    // This is the reason why the playerCard will anchor on the exact x and y position no matter what screen size
    private void UpdateAnchor(float centerX)
    {
        var newPos = CameraViewportHandler.Instance.BottomCenter +
                     new Vector3(centerX + AnchorOffset.x, AnchorOffset.y, AnchorOffset.z);

        if (!transform.position.Equals(newPos))
            transform.position = newPos;
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

    // tween and destroy the card game object
    private void MoveCardAndDestroy(GameObject target, Vector3 targetPosition, Vector3 targetScale,int targetCardCount, float duration,
        Action animationComplete)
    {
        target.transform.DOLocalMove(
                new Vector3(targetPosition.x,
                    targetPosition.y,
                    targetPosition.z + (-0.01f * targetCardCount)), duration)
            .SetEase(easeType);

        target.transform.DOScale(targetScale, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                target.transform.DOKill();
                Destroy(target);
                animationComplete();
            });
    }

    private void AnimateTransparency(GameObject target)
    {
        target.GetComponent<SpriteRenderer>().DOFade(1f, 0.2f);
    }

    private void ResetActiveCardView()
    {
        if (_cardViews.Count > 0)
        {
            for (var i = 0; i < _cardViews.Count; i++)
            {
                var cardView = _cardViews[i].GetComponent<CardView>();
                if (cardView && cardView.IsActive)
                {
                    cardView.IsActive = false;
                    cardView.transform.localPosition = new Vector3(cardView.transform.localPosition.x, 0,
                        cardView.transform.localPosition.z);
                }
            }
        }
    }

    private void SelectCardHandler(CardModel selectedCardData, bool isActive)
    {
        for (var i = 0; i < _cardViews.Count; i++)
        {
            var cardView = _cardViews[i].GetComponent<CardView>();
            if (cardView.Data.Suits == selectedCardData.Suits && cardView.Data.Rank == selectedCardData.Rank)
            {
                if (isActive)
                    cardView.transform.localPosition = new Vector3(cardView.transform.localPosition.x,
                        _selectCardOffsetY, cardView.transform.localPosition.z);
                else
                    cardView.transform.localPosition = new Vector3(cardView.transform.localPosition.x, 0,
                        cardView.transform.localPosition.z);
            }
        }
    }
}