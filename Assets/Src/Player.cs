using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Token Token;

    public Deck MovementDeck;
    public Deck AttackDeck;
    public Deck SpecialDeck;

    public Hand Hand;

    public int Health = 20;

    public void SetActive(bool isActive)
    {
        Hand?.gameObject.SetActive(isActive);
        AttackDeck?.gameObject.SetActive(isActive);
        MovementDeck?.gameObject.SetActive(isActive);
        SpecialDeck?.gameObject.SetActive(isActive);
    }

    public void Damage(int amount)
    {
        Health -= amount;
    }
}
