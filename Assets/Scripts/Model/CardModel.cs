using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardModel
{
    // from 0 to 3
    [SerializeField]
    public int Suits;
    // from 1 to 13
    [SerializeField]
    public int Rank;
}
