using System;
using System.Collections.Generic;

[Serializable]
public class PlayerCard
{
    public int TotalPoints;
    public int TotalCard;
    public List<CardModel> Cards = new List<CardModel>();
}