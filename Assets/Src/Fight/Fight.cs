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
    public static TurnPhase TurnPhase = 0;

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
    public static async void EndPhase()
    {
        switch (TurnPhase)
        {
            case TurnPhase.Draw:
                TurnPhase = TurnPhase.Play;
                break;
            case TurnPhase.Play:
                await EndTurn();
                break;
        }
    }
    public static async Task EndTurn()
    {
        if (!_turnOrder.Any()) { EndRound();}

        Players.ForEach(p => p.RpcEndTurn());
        ActivePlayer = _turnOrder.Dequeue();
        ActivePlayer.RpcStartTurn();

        TurnPhase = TurnPhase.Draw;
    }
    public static async void EndRound()
    {
        var players = new List<Player>(Players);
        players.OrderBy(x => x.Initiative);
        _turnOrder = new Queue<Player>(players);
    }
    public static async void EndGame()
    {
        Application.Quit();
    }
    
}
public enum TurnPhase
{
    Draw,
    Play
}
