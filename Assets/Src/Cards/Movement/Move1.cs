using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Move1 : Card {

    public override async void Play()
    {
        var options = Fight.Board.GetTilesWithinRadius(1, Fight.ActivePlayer.Token.Tile.Coord);
        options.Remove(Fight.ActivePlayer.Token.Tile);
        var tile = await Fight.Board.SelectTile(options);
        if (tile == null) return;
        Fight.ActivePlayer.Token.Tile = tile;
        Fight.TurnTimer.AddSecond(SecondType.Movement);
        Discard();
    }

    public override async void Discard()
    {
        Fight.ActivePlayer.MovementDeck.DiscardPile.Insert(this);
    }
}
