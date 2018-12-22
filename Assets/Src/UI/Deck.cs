using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Deck : StaticUI
{
    private readonly Queue<Card> _cards = new Queue<Card>();
    private readonly Queue<Card> _discardedCards = new Queue<Card>();
    private Player _player;

    public ActionType Type;

    private void Start()
    {
        _player = transform.GetComponentInParent<Player>();
        foreach (var card in GetComponentsInChildren<Card>())
        {
            card.Player = _player;
            Discard(card);
        }
        Shuffle();
    }
    private void Update()
    {
        GetComponent<Button>().interactable = _cards.Any();
    }

    public void Draw()
    {
        if (_player.Hand.NumCards >= 6) return;
        var card = _cards.Count > 0 ? _cards.Dequeue() : null;
        if (card == null) return;
        card.IsHidden = false;
        _player.Hand.AddCard(card);
    }
    public void Discard(Card card)
    {
        //Should be enquing into the discard pile
        _cards.Enqueue(card);
        card.SetParent(transform);
        card.SnapTo(Vector3.zero, Vector3.zero);
        card.IsHidden = true;
    }
    public void Shuffle()
    {
        var shuffled = _discardedCards.OrderBy(a => Guid.NewGuid()).ToArray();
        _discardedCards.Clear();
        foreach (var card in shuffled)
        {
            _cards.Enqueue(card);
        }
    }
}
