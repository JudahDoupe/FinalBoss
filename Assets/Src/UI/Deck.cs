using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public ActionType Type;

    private readonly Queue<Card> _drawPile = new Queue<Card>();
    private readonly Queue<Card> _discardPile = new Queue<Card>();
    private Player _player;
    private Button _button;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _button = GetComponent<Button>();
        foreach (var card in GetComponentsInChildren<Card>())
        {
            card.Player = _player;
            Discard(card);
        }
        Shuffle();
    }
    private void Update()
    {
        _button.interactable = _drawPile.Any();
    }

    public void Draw()
    {
        if (_player.Hand.Cards.Count >= 6 || !_drawPile.Any()) return;

        var card = _drawPile.Dequeue();
        card.SetVisible(true);
        _player.Hand.AddCard(card);
    }
    public void Discard(Card card)
    {
        _discardPile.Enqueue(card);
        card.transform.parent = transform;
        card.SnapTo(Vector3.zero, Vector3.zero);
        card.SetVisible(false);
    }
    public void Shuffle()
    {
        var shuffled = _discardPile.OrderBy(a => Guid.NewGuid()).ToArray();
        foreach (var card in shuffled)
        {
            _drawPile.Enqueue(card);
        }
        _discardPile.Clear();
    }
}
