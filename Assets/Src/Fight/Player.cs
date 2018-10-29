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

    public bool IsAI = false;

    public int Health = 20;
    public int Initiative = 0;

    public void SetUIActive(bool isActive)
    {
        Hand?.gameObject.SetActive(isActive);
        AttackDeck?.gameObject.SetActive(isActive);
        MovementDeck?.gameObject.SetActive(isActive);
        SpecialDeck?.gameObject.SetActive(isActive);
    }

    public void Damage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Health = 0;
        Fight.EndGame();
    }
}
