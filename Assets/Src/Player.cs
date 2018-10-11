using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Token Token;

    public Deck MovementDeck;
    public Deck AttackDeck;
    public Deck SpecialDeck;

    public Transform Hand;

    private List<Card> _hand = new List<Card>();

    public void AddCardToHand(Card card)
    {
        _hand.Add(card);
        card.transform.parent = Hand;
        card.transform.localPosition = Vector3.zero;
    }
}
