﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Fight : MonoBehaviour
{
    public const int ActionsPerTurn = 6;
    public const int MaxPlayers = 2;
    
    //Fight
    public static Player ActivePlayer;
    public static List<Player> Players = new List<Player>();
    private static Queue<Player> _turnOrder = new Queue<Player>();

    public static void JoinFight(Player player)
    {
        Players.Add(player);
        Board.SelectedTiles.Add(player, new TaskCompletionSource<Tile>());
        Board.AddToken(player,0);

        if (Players.Count == MaxPlayers) StartFight();
    }
    public static void StartFight()
    {
        var size = 10;
        for (int x = -size; x <= size; x++)
        {
            for (int y = -size; y <= size; y++)
            {
                for (int z = -size; z <= size; z++)
                {
                    if (x + y + z == 0) Board.AddTile(new TileCoord(x, y, z));
                }
            }
        }

        EstablishTurnOrder();
        Players.ForEach(p => p.TargetWatchGame(p.connectionToClient));
        NextTurn();
    }
    public static void NextTurn()
    {
        ClearActions();
        ActivePlayer?.TargetWatchGame(ActivePlayer.connectionToClient);

        if (!_turnOrder.Any()) EstablishTurnOrder();

        ActivePlayer = _turnOrder.Dequeue();
        ActivePlayer.TargetStartTurn(ActivePlayer.connectionToClient); 
    }
    public static void EstablishTurnOrder()
    {
        var players = new List<Player>(Players);
        players.OrderBy(x => x.Initiative);
        _turnOrder = new Queue<Player>(players);
    }
    public static void EndFight()
    {
        Application.Quit();
    }

    //Cards
    public static async void PlayCard(NetworkConnection connectionToClient, string cardName)
    {
        var isPlayable = IsCardPlayable(cardName);
        var player = Players.First(x => x.connectionToClient == connectionToClient);
        if (!isPlayable)
        {
            player.TargetFinishPlayingCard(connectionToClient, false);
            return;
        }

        switch (cardName)
        {
            case "Dodge":
                await CardExecutor.Move(player,1);
                UseActions(ActionType.Movement,1);
                player.Initiative += 1;
                break;
            case "Walk":
                await CardExecutor.Move(player,2);
                UseActions(ActionType.Movement, 2);
                player.Initiative += 2;
                break;
            case "Run":
                await CardExecutor.Move(player,3);
                UseActions(ActionType.Movement, 3);
                player.Initiative += 3;
                break;
            case "Punch":
                await CardExecutor.Attack(player,1);
                UseActions(ActionType.Attack, 1);
                player.Initiative += 2;
                break;
            case "Bomb":
                await CardExecutor.Bomb(player);
                UseActions(ActionType.Special, 3);
                player.Initiative += 8;
                break;
        }

        player.TargetFinishPlayingCard(connectionToClient, true);
    }
    private static bool IsCardPlayable(string cardName)
    {
        var cardActions = 1;
        return ActionNumber + cardActions <= ActionsPerTurn;
    }

    //Action Counter
    public static ActionType[] TurnActions { get; private set; }
    public static int ActionNumber { get; private set; }

    public static void UseActions(ActionType type, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            foreach (var player in Players)
            {
                player.RpcAddAction(type);
            }
            TurnActions[ActionNumber] = type;
            ActionNumber++;
        }
    }
    public static void ClearActions()
    {
        foreach (var player in Players)
        {
            player.RpcClearActions();
        }
        TurnActions = new[] { ActionType.Neutral, ActionType.Neutral, ActionType.Neutral, ActionType.Neutral, ActionType.Neutral, ActionType.Neutral };
        ActionNumber = 0;
    }
}
public enum ActionType
{
    Movement,
    Attack,
    Special,
    Neutral
}