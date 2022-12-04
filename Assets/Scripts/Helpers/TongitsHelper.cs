using System.Collections.Generic;

public class TongitsHelper
{
    /// <summary>
    /// Compute the total score of the card view is holding
    /// </summary>
    public static int ComputeTotalScore(List<CardModel> cardCollection)
    {
        var tempTotalPoints = 0;
        foreach (var card in cardCollection)
        {
            if (card.Rank > 9) tempTotalPoints += 10;
            else tempTotalPoints++;
        }

        return tempTotalPoints;
    }
}
