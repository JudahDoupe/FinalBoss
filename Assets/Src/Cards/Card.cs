using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Card : UIObject
{
    public string CardName;

    public Player Player;
    public ActionType Type;

    public void Click()
    {
        Player.Hand.SelectedCard = this;
        Player.CmdPlayCard(CardName);
    }

    public void Discard()
    {
        Player.Decks[Type].Discard(this);
    }
}
