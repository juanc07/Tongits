using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCardSprite : MonoBehaviour
{
    public CardSpriteContainer cardContainer;

    [Tooltip("1 to 13 represent Ace to King")]
    [Range(1,13)]
    public int rank;
    [Tooltip("0 to 3 represent Clover, Diamond, Heart and Spade in this order")]
    [Range(0,3)]
    public int suit;
    public float delay = 3f;
    public float repeatRate = 0.1f;
    public bool isLoadBackCard; 
    public bool isRandom;
    public bool isLoop;
    
    private SpriteRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        // change card after some duration
        if (isLoop) InvokeRepeating(nameof(ChangeCard),delay,repeatRate); 
        else Invoke(nameof(ChangeCard),delay);
    }

    public void ChangeCard()
    {
        Debug.Log("ChangeCard!");
        if (!isLoadBackCard)
        {
            if (isRandom)
            {
                rank = Random.Range(1, 13);
                suit = Random.Range(0, 3);
            }
            _renderer.sprite = (Sprite) cardContainer.cards[rank].GetValue(suit);    
        }
        else
        {
            _renderer.sprite = (Sprite) cardContainer.cardBack;
        }
    }
}
