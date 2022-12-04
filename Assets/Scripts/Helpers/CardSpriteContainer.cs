using System;
using UnityEngine;

public class CardSpriteContainer : MonoBehaviour
{
    [Serializable] public class SpriteArrayStorage : SerializableDictionary.Storage<Sprite[]> {}
    [Serializable] public class IntSpriteArrayDictionary : SerializableDictionary<int, Sprite[], SpriteArrayStorage> {}

    public Sprite cardBack;
    public IntSpriteArrayDictionary cards;

    // Start is called before the first frame update
    void Start()
    {
        // you can acces the card data using this cards[2].GetValue(1)
        //Debug.Log($"cards {cards[2].GetValue(1)}");
    }
}
