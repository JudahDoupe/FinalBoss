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

        foreach(var card in GetComponentsInChildren<Card>())
        {
            Insert(card);
        }
    }

    public Card Draw()
    {
        if (Deck.Player.Hand.NumCards >= 5)
            return null;
        return Cards.Count > 0 ? Cards.Dequeue() : null;
    }
    public void Insert(Card card)
    {
        Cards.Enqueue(card);
        card.transform.parent = transform;
        card.transform.localPosition = Vector3.zero;
        card.transform.localEulerAngles = Vector3.zero;
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
