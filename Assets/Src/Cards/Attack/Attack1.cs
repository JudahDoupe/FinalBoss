using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Attack1 : Card
{
    [Command]
    public override async void CmdPlay()
    {
        var options = Board.GetNeighbors(Player.Token.Coord);
        var tile = await Board.SelectTile(options);

        var damagee = Fight.Players.SingleOrDefault(x => x.Token.Coord == tile.Coord);
        if (damagee != null) damagee.RpcDamage(1);
         
        Fight.UseAction(ActionType.Attack);
        Player.Initiative += 2;

        IsBeingPlayed = false; 
    }

}
