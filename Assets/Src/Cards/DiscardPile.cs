using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : Pile {

    void Click()
    {
        if (Cards.Count > 0)
        {
            Deck.Reshuffle();
        }
        else
        {
            Debug.Log("No Cards in discard pile");
        }
    }
}
