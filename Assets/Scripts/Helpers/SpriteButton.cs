using System;
using UnityEngine;

public class SpriteButton : MonoBehaviour
{
    // events
    private Action ClickSprite;
    public event Action OnClickSprite {
        add { ClickSprite += value; }
        remove { ClickSprite -= value; }
    }
    
    private Collider2D _collider2D;
    private Transform _hit;
    //private CardView _cardView;

    private void Awake()
    {
        //_cardView =GetComponent<CardView>();
        _collider2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (IsTouched())
        {
            ClickSprite?.Invoke();
            //Debug.Log($"object clicked Suit: {_hit.transform.GetComponent<CardView>().Data.Suits} Rank: Suit: {_hit.transform.GetComponent<CardView>().Data.Rank}");
            //_cardView.ToggleActive();
        }
    }

    private bool IsTouched()
    {
        bool result = false;
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector2 touchPos = new Vector2(wp.x,wp.y);
                
                var hit  = Physics2D.Raycast(touchPos, Vector2.zero);
                
                if (_collider2D == Physics2D.OverlapPoint(touchPos))
                {
                    _hit = hit.transform;
                    result = true;
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos = new Vector2(wp.x,wp.y);
            var hit  = Physics2D.Raycast(mousePos, Vector2.zero);
            if (_collider2D == Physics2D.OverlapPoint(mousePos))
            {
                _hit = hit.transform;
                result = true;
            }
        }

        return result;
    }
}