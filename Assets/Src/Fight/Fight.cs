using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Fight : MonoBehaviour
{
    //Fight
    public static bool GameInProgress = false;
    public static int MaxPlayers = 1;
    public static Player ActivePlayer;
    public static List<Player> Players = new List<Player>();
    private static Queue<Player> _turnOrder = new Queue<Player>();

    private static Fight Instance;

    private void Start()
    {
        Instance = this;
    }

    public static void JoinFight(Player player)
    {
        Players.Add(player);
        if (Players.Count == MaxPlayers)
        {
            Instance.StartCoroutine(StartFight());
        }
    }
    public static IEnumerator StartFight()
    {
        yield return new WaitUntil(() => Players.All(x => x.Token.Coord != null));
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