﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Dodge : CardExecution
{
    public Dodge()
    {
        Name = "Dodge";
        Initiative = 1;
        Actions = new List<ActionType>{ActionType.Movement};
    }
    public override async Task<bool> Play(Player player)
    {
        await Move(player, 1);
        return true;
    }
}
public class Walk : CardExecution
{
    public Walk()
    {
        Name = "Walk";
        Initiative = 2;
        Actions = new List<ActionType> { ActionType.Movement, ActionType.Movement };
    }
    public override async Task<bool> Play(Player player)
    {
        await Move(player, 2);
        return true;
    }
}
public class Run : CardExecution
{
    public Run()
    {
        Name = "Run";
        Initiative = 3;
        Actions = new List<ActionType> { ActionType.Movement, ActionType.Movement, ActionType.Movement };
    }
    public override async Task<bool> Play(Player player)
    {
        await Move(player, 3);
        return true;
    }
}
public class Punch : CardExecution
{
    public Punch()
    {
        Name = "Punch";
        Initiative = 2;
        Actions = new List<ActionType> { ActionType.Attack };
    }
    public override async Task<bool> Play(Player player)
    {
        await Attack(player, 2);
        return true;
    }
    public override bool isPlayable(Player player)
    {
        return base.isPlayable(player) && Board.GetNeighbors(Board.GetToken(player).Coord).Any(x => x.Token != null);
    }
}
public class Bomb : CardExecution
{
    public Bomb()
    {
        Name = "Bomb";
        Initiative = 5;
        Actions = new List<ActionType> { ActionType.Special, ActionType.Special, ActionType.Special };
    }
    public override async Task<bool> Play(Player player)
    {
        await Explode(player, 3);
        return true;
    }
}
public class BuildWall : CardExecution
{
    public BuildWall()
    {
        Name = "Build Wall";
        Initiative = 10;
        Actions = new List<ActionType> { ActionType.Special, ActionType.Special };
    }
    public override async Task<bool> Play(Player player)
    {
        var options = Board.GetNeighbors(Board.GetToken(player).Coord);
        var tile = await player.SelectTile(options);
        tile.Connections.First(conn => conn.From(tile) == Board.GetTile(Board.GetToken(player).Coord)).RpcBuildWall();
        return true;
    }
    public override bool isPlayable(Player player)
    {
        return base.isPlayable(player) && Board.GetNeighbors(Board.GetToken(player).Coord).Count > 1;
    }
}

public abstract class CardExecution
{
    public string Name = "Default";
    public int Initiative = 1;
    public List<ActionType> Actions = new List<ActionType> { ActionType.Neutral };

    public abstract Task<bool> Play(Player player);
    public virtual bool isPlayable(Player player)
    {
        return Fight.ActionNumber + Actions.Count <= Fight.ActionsPerTurn;
    }

    protected async Task Move(Player player, int distance)
    {
        var options = Board.GetTilesWithinDistance(distance, Board.GetToken(player).Coord);
        options.Remove(Board.GetTile(Board.GetToken(player).Coord));
        var tile = await player.SelectTile(options);
        if(tile != null) Board.MoveToken(player, tile.Coord);
        else Debug.Log("No tile was selected");
    }
    protected async Task Attack(Player player, int damage)
    {
        var options = Board.GetNeighbors(Board.GetToken(player).Coord).Where(x => x.Token != null).ToList();
        var tile = await player.SelectTile(options);

        tile.Token.Player.RpcAddDamage(damage);
    }
    protected async Task Explode(Player player, int radius)
    {
        var options = Board.GetTilesWithinRadius(radius, Board.GetToken(player).Coord);
        Board.GetTilesWithinRadius(1, Board.GetToken(player).Coord).ForEach(x => options.Remove(x));

        var tile = await player.SelectTile(options);
        if (tile == null) return;
        var tilesToRemove = Board.GetTilesWithinRadius(1, tile.Coord);
        tilesToRemove.ForEach(t => Board.RemoveTile(t.Coord));
    }
}