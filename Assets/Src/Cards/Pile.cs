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
    private void Update()
    {
        foreach(var card in Cards)
        {
            card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, Vector3.zero, 3 * Time.deltaTime);
            card.transform.localEulerAngles = Vector3.Lerp(card.transform.localEulerAngles, Vector3.zero, 3 * Time.deltaTime);
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
