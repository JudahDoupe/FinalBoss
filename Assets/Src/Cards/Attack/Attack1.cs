using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : Card {

    public override async void Play()
    {
        Fight.Boss.Damage(1);
        Fight.TurnTimer.AddSecond(SecondType.Attack);
        Discard();
    }

    public override async void Discard()
    {
        Fight.ActivePlayer.AttackDeck.DiscardPile.Insert(this);
    }

    public override bool IsPlayable()
    {
        return base.IsPlayable() && Fight.Board.GetNeighbors(Fight.ActivePlayer.Token.Tile.Coord).Contains(Fight.Boss.Token.Tile);
    }
}
