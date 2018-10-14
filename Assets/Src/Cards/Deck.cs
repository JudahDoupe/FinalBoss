using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public DrawPile DrawPile;
    public DiscardPile DiscardPile;
    public Player Player;


    public void Start()
    {
        Player = transform.parent.GetComponent<Player>();
        DrawPile = DrawPile ?? GetComponentInChildren<DrawPile>();
        DiscardPile = DiscardPile ?? GetComponentInChildren<DiscardPile>();
    }

    public void Reshuffle()
    {
        DiscardPile.Shuffle();
        var numCards = DiscardPile.Cards.Count;
        for (int i=0; i < numCards; i++)
        {
            DrawPile.Insert(DiscardPile.Draw());
        }
    }
}
