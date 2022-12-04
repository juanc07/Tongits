using UnityEngine;

public class CreateCardModel
{
    public GameObject CardObject { get; }

    public CardModel CardData { get; }

    public Vector3 CardPosition { get; }

    public CreateCardModel(GameObject cardObject,CardModel cardData,Vector3 cardPosition)
    {
        CardObject = cardObject;
        CardData = cardData;
        CardPosition = cardPosition;
    }
}
