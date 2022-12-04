using System.Collections.Generic;

public class PlayerTableCard
{
    private int _totalCard;
    private List<CardModel> _cards = new List<CardModel>();
    public int TotalCard => _cards.Count;
    
    public void Push(CardModel cardData)
    {
        _cards.Add(cardData);
    }

    public void Clear()
    {
        _cards.Clear();
    }
}
