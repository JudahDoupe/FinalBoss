using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Move1 : Card {

    public override async void Play()
    {
        var options = Board.GetTilesWithinRadius(1, Player.Token.Coord);
        options.Remove(Board.GetTile(Player.Token.Coord));
        var tile = await Board.SelectTile(options);
        if (tile == null) return;
        Player.Token.Coord = tile.Coord;
        Player.TurnTimer.AddSecond(SecondType.Movement);
        Player.Initiative += 1;
        Discard();
    }

    public override async void Discard()
    {
        Player.MovementDeck.DiscardPile.Insert(this);
    }
}
