using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpecialBomb : Card {

    [Command]
    public override async void CmdPlay()
    {
        var options = Board.GetTilesWithinRadius(3, Player.Token.Coord);
        Board.GetTilesWithinRadius(1, Player.Token.Coord).ForEach( x => options.Remove(x) );

        var tile = await Board.SelectTile(Player.connectionToClient, options);
        if (tile == null) return;
        var tilesToRemove = Board.GetTilesWithinRadius(1, tile.Coord);
        tilesToRemove.ForEach(t => Board.RemoveTile(t.Coord));  

        for (int i = 0; i < SecondsToPlay; i++)
            Fight.UseAction(ActionType.Special);
        
        Player.Initiative += 1;

        IsBeingPlayed = false;
    }
}
