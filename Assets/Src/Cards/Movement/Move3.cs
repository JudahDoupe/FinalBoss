using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Move3 : Card
{
    [Command]
    public override async void CmdPlay()
    {
        var options = Board.GetTilesWithinRadius(3, Player.Token.Coord);
        options.Remove(Board.GetTile(Player.Token.Coord));
        var tile = await Board.SelectTile(options);
        if (tile == null) return;
        Player.Token.Coord = tile.Coord;
        Fight.UseAction(ActionType.Movement);
        Fight.UseAction(ActionType.Movement);
        Fight.UseAction(ActionType.Movement);
        Player.Initiative += 2;

        IsBeingPlayed = false;
    }
}
