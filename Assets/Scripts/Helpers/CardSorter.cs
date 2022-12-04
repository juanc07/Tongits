using System.Collections.Generic;
using UnityEngine;

public class CardSorter
{
    /// <summary>
    /// Sorts in Increasing order
    /// </summary>
    /// <returns></returns>
    public static List<CardModel> SortDescending(List<CardModel> cards)
    {
        for (var i = 1; i < cards.Count; i++)
        {
            var data = cards[i];
            var j = i - 1;

            /* below while loop traverse all the elements which are before the
             key element from right to left or breaks if finds a small element 
             than key */
            while (j > -1 && cards[j].Rank > data.Rank)
            {
                cards[j + 1] = cards[j]; // moving the element to next position
                j--;
            }

            cards[j + 1] = data; // **Imp, It is inserting key element on right position
        }

        return cards;
    }

    /// <summary>
    /// Sorts in decreasing order
    /// </summary>
    /// <returns></returns>
    public static List<CardModel> SortAscending(List<CardModel> cards)
    {
        for (var i = 1; i < cards.Count; i++)
        {
            var data = cards[i];
            var j = i - 1;
            while (j > -1 && cards[j].Rank < data.Rank)
            {
                cards[j + 1] = cards[j];
                j--;
            }

            cards[j + 1] = data;
        }

        return cards;
    }

    public static List<List<CardModel>> LookForCombination(List<CardModel> cards)
    {
        var combination = new List<List<CardModel>>();
        for (var i = 0; i < cards.Count; i++)
        {
            var j = 0;
            var tempCombination = new List<CardModel>();
            while (j < cards.Count)
            {
                if (cards[i].Rank == cards[j].Rank)
                {
                    tempCombination.Add(cards[i]);
                }

                j++;
            }

            if (tempCombination.Count >= 3)
            {
                Debug.Log("add combination!!");
                combination.Add(tempCombination);
            }
        }

        return combination;
    }

    private static int CompareTo(CardModel target,CardModel that)
    {
        if (target.Suits > that.Suits)return 1;
        if (target.Suits < that.Suits)return -1;
        if (target.Rank > that.Rank) return 1;
        if (target.Rank < that.Rank) return -1;
        return 0;
    }

    public static List<CardModel> SortDescendingWithSuits(List<CardModel> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cards.Count - 1; j++)
            {
                if (CompareTo(cards[j], cards[j + 1])==1)
                {
                    var temp = cards[j + 1];
                    cards[j + 1] = cards[j];
                    cards[j] = temp;
                }
            }
        }
        return cards;
    }
    
}