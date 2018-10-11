using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Move2 : Card
{

    public override async void Play()
    {
        var options = Fight.Board.GetTilesWithinRadius(2, Fight.ActivePlayer.Token.Tile.Coord);
        options.Remove(Fight.ActivePlayer.Token.Tile);
        var tile = await Fight.Board.SelectTile(options);
        if (tile == null) return;
        Fight.ActivePlayer.Token.Tile = tile;
        Discard();
    }

    public override async void Discard()
    {
        Fight.ActivePlayer.MovementDeck.Discard(this);
    }
}
