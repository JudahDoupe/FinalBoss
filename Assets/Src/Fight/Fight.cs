using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Fight : MonoBehaviour
{
    public static Board Board;
    public static Player ActivePlayer;
    public static TurnTimer TurnTimer;
    public static List<Player> Players;
    public static TurnPhase TurnPhase = 0;

    private static Queue<Player> _turnOrder = new Queue<Player>();

    void Start()
    {
        Players = FindObjectsOfType<Player>().ToList();
        Board = FindObjectOfType<Board>();
        TurnTimer = FindObjectOfType<TurnTimer>();
        NextTurn();
    }

    public static async void EndPhase()
    {
        if(TurnPhase == TurnPhase.Draw)
        {
            TurnPhase = TurnPhase.Play;
        }
        else if(TurnPhase == TurnPhase.Play)
        {
            await NextTurn();
        }
    }
    public static async Task NextTurn()
    {
        ActivePlayer?.SetUIActive(false);

        if (!_turnOrder.Any())
            NextRound();

        TurnTimer.Clear();
        ActivePlayer = _turnOrder.Dequeue();

        if (ActivePlayer.Token.Tile == null) await Board.PlaceToken(ActivePlayer.Token);

        if (!ActivePlayer.IsAI) ActivePlayer.SetUIActive(true);
        TurnPhase = TurnPhase.Draw;
    }
    public static void NextRound()
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
