using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CardExecutor : MonoBehaviour{

    public static async Task Move(Player player, int distance)
    {
        var options = Board.GetTilesWithinRadius(distance, Board.GetToken(player).Coord);
        options.Remove(Board.GetTile(Board.GetToken(player).Coord));
        var tile = await Board.SelectTile(player, options);

        Board.MoveToken(player, tile.Coord);
    }
    public static async Task Attack(Player player, int damage)
    {
        var options = Board.GetNeighbors(Board.GetToken(player).Coord);
        var tile = await Board.SelectTile(player, options);

        var damagee = Fight.Players.SingleOrDefault(x => Board.GetToken(x).Coord == tile.Coord);
        if (damagee != null) damagee.Health -= damage;
    }
    public static async Task Bomb(Player player)
    {
        var options = Board.GetTilesWithinRadius(5, Board.GetToken(player).Coord);
        Board.GetTilesWithinRadius(1, Board.GetToken(player).Coord).ForEach(x => options.Remove(x));

        var tile = await Board.SelectTile(player, options);
        if (tile == null) return;
        var tilesToRemove = Board.GetTilesWithinRadius(1, tile.Coord);
        tilesToRemove.ForEach(t => Board.RemoveTile(t.Coord));
    }
}