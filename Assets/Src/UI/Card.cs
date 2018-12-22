using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Card : MovableUI, IPointerExitHandler, IPointerEnterHandler
{
    public string CardName;

    public Player Player;
    public ActionType Type;

    public void Play()
    {
        Player.Hand.SelectedCard = this;
        Player.CmdPlayCard(CardName);
    }

    public void Discard()
    {
        Player.Decks[Type].Discard(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var hand = GetComponentInParent<Hand>();
        if (hand == null) return;
        hand.SelectedCard = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var hand = GetComponentInParent<Hand>();
        if (hand == null) return;
        hand.SelectedCard = null;
    }
}
