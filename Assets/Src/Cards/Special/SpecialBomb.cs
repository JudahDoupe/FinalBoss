using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBomb : Card {

    public override async void Play()
    {
        var options = Board.GetTilesWithinRadius(3, Player.Token.Coord);
        Board.GetTilesWithinRadius(1, Player.Token.Coord).ForEach( x => options.Remove(x) );

        var tile = await Board.SelectTile(options);
        if (tile == null) return;
        var tilesToRemove = Board.GetTilesWithinRadius(1, tile.Coord);
        tilesToRemove.ForEach(t => Board.RemoveTile(t.Coord));  

        for (int i = 0; i < SecondsToPlay; i++)
            Player.TurnTimer.AddSecond(SecondType.Special);
        
        Player.Initiative += 1;

        Discard(); 
    }

    public override void Discard()
    {
        Player.SpecialDeck.DiscardPile.Insert(this);
    }
}
