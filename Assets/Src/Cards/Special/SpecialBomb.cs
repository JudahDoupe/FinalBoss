using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBomb : Card {

    public override async void Play()
    {
        var options = Fight.Board.GetTilesWithinRadius(3, Fight.ActivePlayer.Token.Tile.Coord);
        Fight.Board.GetTilesWithinRadius(1, Fight.ActivePlayer.Token.Tile.Coord).ForEach( x => options.Remove(x) );

        var tile = await Fight.Board.SelectTile(options);
        if (tile == null) return;
        var tilesToRemove = Fight.Board.GetTilesWithinRadius(1, tile.Coord);
        tilesToRemove.ForEach(t => Fight.Board.RemoveTile(t.Coord));  

        for (int i = 0; i < SecondsToPlay; i++)
            Fight.TurnTimer.AddSecond(SecondType.Special);

        Player.Initiative += 1;

        Discard();
    }

    public override void Discard()
    {
        Fight.ActivePlayer.SpecialDeck.DiscardPile.Insert(this);
    }
}
