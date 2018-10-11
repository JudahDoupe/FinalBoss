using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : Pile {

    void Click()
    {
        var card = Deck.Draw();
        if (card != null)
        {
            Fight.ActivePlayer.AddCardToHand(card);
        }
        else
        {
            Debug.Log("No Cards left To draw");
        }
    }
}
