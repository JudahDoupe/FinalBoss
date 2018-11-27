using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Fight : MonoBehaviour
{
    public static Player ActivePlayer;
    public static List<Player> Players = new List<Player>();

    private static Queue<Player> _turnOrder = new Queue<Player>();

    public static void Join(Player player)
    {
        Players.Add(player);
        if (Players.Count == 1) StartGame();
    }

    public static void StartGame()
    {
        Debug.Log("Starting Game");
        EndTurn();
    }
    public static void EndTurn()
    {
        if (!_turnOrder.Any()) { EndRound();}

        Players.ForEach(p => p.RpcEndTurn());
        ActivePlayer = _turnOrder.Dequeue();
        ActivePlayer.RpcStartTurn();
    }
    public static void EndRound()
    {
        var players = new List<Player>(Players);
        players.OrderBy(x => x.Initiative);
        _turnOrder = new Queue<Player>(players);
    }
    public static void EndGame()
    {
        Application.Quit();
    }

    public static void UseSecond(SecondType type)
    {
        foreach (var player in Players)
        {
            player.TurnTimer.RpcAddSecond(type);
        }
    }
}
