using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Fight : MonoBehaviour
{
    public const int ActionsPerTurn = 5;
    public const int MaxPlayers = 1;
    
    //Fight
    public static bool GameInProgress = false;
    public static Player ActivePlayer;
    public static List<Player> Players = new List<Player>();
    private static Queue<Player> _turnOrder = new Queue<Player>();

    public static void JoinFight(Player player)
    {
        Players.Add(player);
        Board.SelectedTiles.Add(player, new TaskCompletionSource<Tile>());
        Board.AddToken(player,0);
        if (Players.Count == MaxPlayers)
        {
            StartFight();
        }
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
        EndTurn();
    }
    public static void EndTurn()
    {
        if (!_turnOrder.Any()) { EndRound();}
        Players.ForEach(p => p.TargetEndTurn(p.connectionToClient));

        ClearActions();

        ActivePlayer = _turnOrder.Dequeue();
        ActivePlayer.TargetStartTurn(ActivePlayer.connectionToClient); 
    }
    public static void EndRound()
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
        player.TargetPlayCard(connectionToClient, isPlayable);
        if (!isPlayable) return;
        switch (cardName)
        {
            case "Move1":
                await CardExecutor.Move(player,1);
                UseActions(ActionType.Movement,1);
                player.Initiative += 1;
                break;
            case "Move2":
                await CardExecutor.Move(player,2);
                UseActions(ActionType.Movement, 2);
                player.Initiative += 2;
                break;
            case "Move3":
                await CardExecutor.Move(player,3);
                UseActions(ActionType.Movement, 3);
                player.Initiative += 3;
                break;
            case "Attack1":
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
    }
    private static bool IsCardPlayable(string cardName)
    {
        var cardActions = 1;
        return ActionNumber + cardActions < ActionsPerTurn;
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
        TurnActions = new[] { ActionType.Neutral, ActionType.Neutral, ActionType.Neutral, ActionType.Neutral, ActionType.Neutral };
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