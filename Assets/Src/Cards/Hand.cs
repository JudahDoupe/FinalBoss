﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public List<Card> Cards = new List<Card>();
    public int NumCards { get { return Cards.Count; } }

    public void AddCard(Card card)
    {
        if (card == null) return;
        card.transform.parent = transform;
        Cards.Add(card);
    }
    public void RemoveCard(Card card)
    {
        Cards.Remove(card);
    }

    private void Update()
    {
        for(int i = 0; i < NumCards; i++)
        {
            if(Cards[i].transform.parent != transform)
            {
                RemoveCard(Cards[i]);
                return;
            }

            var cardWidth = Cards[i].transform.localScale.x;
            var offset = i - (NumCards-1) / 2f;
            Cards[i].transform.localPosition = new Vector3(offset * cardWidth, -0.025f * Mathf.Abs(offset), 0);
            Cards[i].transform.localEulerAngles = new Vector3(0,0,-10 * offset);
        }
    }
}