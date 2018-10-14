using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : Pile {

    void Click()
    {
        var card = Draw();
        if (card != null)
        {
            Deck.Player.Hand.AddCard(card);
        }
        else
        {
            Debug.Log("Cannot draw any more cards");
        }
    }
}
