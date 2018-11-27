using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Move1 : Card {

    [Command]
    public override async void CmdPlay()
    {

        Debug.Log("Move1");
        var options = Board.GetTilesWithinRadius(1, Player.Token.Coord);
        options.Remove(Board.GetTile(Player.Token.Coord));
        var tile = await Board.SelectTile(options);
        if (tile == null) return;
        Player.Token.Coord = tile.Coord;
        Fight.UseSecond(SecondType.Movement);
        Player.Initiative += 1;

        IsBeingPlayed = false;
    }
}
