using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public DrawPile DrawPile;
    public DiscardPile DiscardPile;

    private Player _player;

    public void Start()
    {
        var cards = new Queue<Card>(GetComponentsInChildren<Card>());
        foreach(var card in cards)
        {
            DrawPile.Insert(card);
        }
        DrawPile = DrawPile ?? GetComponentInChildren<DrawPile>();
        DiscardPile = DiscardPile ?? GetComponentInChildren<DiscardPile>();
        _player = transform.parent.GetComponent<Player>();
    }

    public Card Draw()
    {
        return DrawPile.Draw();
    }
    public void Discard(Card card)
    {
        DiscardPile.Insert(card);
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
