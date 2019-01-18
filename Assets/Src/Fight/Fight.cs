using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Fight : MonoBehaviour
{
    public const int ActionsPerTurn = 6;
    public static int MaxPlayers = 1;
    public static NetworkManager NetworkManager;
    
    public static Player ActivePlayer;
    public static List<Player> Players = new List<Player>();
    private static Queue<Player> _turnOrder = new Queue<Player>();

    public static ActionType[] TurnActions { get; private set; }
    public static int ActionNumber { get; private set; }

    void Start()
    {
        NetworkManager = GetComponent<NetworkManager>();
    }

    public void SetIpAddress(string ipAddress)
    {
        NetworkManager.networkAddress = ipAddress;
    }
    public void StartGame(int maxPlayers)
    {
        NetworkManager.networkAddress = "localhost";
        MaxPlayers = maxPlayers;
        NetworkManager.StartHost();
    }
    public void JoinGame()
    {
        NetworkManager.StartClient();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public static void JoinFight(Player player)
    {
        Players.Add(player);

        if (Players.Count == MaxPlayers) StartFight();
    }
    public static void StartFight()
    {
        Board.Build();
        System.Threading.Thread.Sleep(1000);

        EstablishTurnOrder();
        foreach (var player in Players)
        {
            Board.AddToken(player,0);
            player.TargetWatchGame(player.connectionToClient);
        }

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
        Players.ForEach(x => x.RpcClearInitiative());
    }
    public static void EndFight()
    {
        NetworkManager.StopServer();
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