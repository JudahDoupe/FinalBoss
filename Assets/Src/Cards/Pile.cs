using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pile : MonoBehaviour {

    public Queue<Card> Cards = new Queue<Card>();
    [HideInInspector]
    public Deck Deck;

    void Start()
    {
        Deck = Deck ?? transform.parent.GetComponent<Deck>();
    }

    public Card Draw()
    {
        return Cards.Count > 0 ? Cards.Dequeue() : null;
    }
    public void Insert(Card card)
    {
        Cards.Enqueue(card);
        card.transform.parent = transform;
        card.transform.position = transform.position;
    }
    public void Shuffle()
    {
        var shuffled = Cards.OrderBy(a => Guid.NewGuid()).ToArray();
        Cards.Clear();
        foreach (var card in shuffled)
        {
            Cards.Enqueue(card);
        }
    }
}
