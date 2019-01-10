using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Fight : MonoBehaviour
{
    public const int ActionsPerTurn = 6;
    public const int MaxPlayers = 1;
    
    public static Player ActivePlayer;
    public static List<Player> Players = new List<Player>();
    private static Queue<Player> _turnOrder = new Queue<Player>();

    public static ActionType[] TurnActions { get; private set; }
    public static int ActionNumber { get; private set; }

    public static void JoinFight(Player player)
    {
        Players.Add(player);
        Board.AddToken(player,0);

        if (Players.Count == MaxPlayers) StartFight();
    }
    public static void StartFight()
    {
        Board.Build();

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

    public static void UseActions(ActionType type)
    {
        foreach (var player in Players)
        {
            player.RpcAddAction(type);
        }
        TurnActions[ActionNumber] = type;
        ActionNumber++;
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