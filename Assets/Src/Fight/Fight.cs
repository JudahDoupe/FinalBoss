using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Fight : MonoBehaviour
{
    //Fight
    public static Player ActivePlayer;
    public static List<Player> Players = new List<Player>();

    private static Queue<Player> _turnOrder = new Queue<Player>();

    public static void JoinFight(Player player)
    {
        Players.Add(player);
        if (Players.Count == 1) StartFight();
    }
    public static void StartFight()
    {
        EndTurn();
    }
    public static void EndTurn()
    {
        if (!_turnOrder.Any()) { EndRound();}
        Players.ForEach(p => p.RpcEndTurn());

        TurnActions = new[] {ActionType.Neutral, ActionType.Neutral, ActionType.Neutral, ActionType.Neutral, ActionType.Neutral};
        ActionNumber = 0;

        ActivePlayer = _turnOrder.Dequeue();
        ActivePlayer.RpcStartTurn();
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

    //Action Counter
    public static ActionType[] TurnActions { get; private set; }
    public static int ActionNumber { get; private set; }

    public static void UseAction(ActionType type)
    {
        TurnActions[ActionNumber] = type;
        ActionNumber++;
    }
}
public enum ActionType
{
    Movement,
    Attack,
    Special,
    Neutral
}