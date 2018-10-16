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

    public static Player Player1;
    public static Player Player2;
    public static Player Boss;

    public Player _Player1;
    public Player _Player2;
    public Player _Boss;

    void Start()
    {
        Player1 = _Player1;
        Player2 = _Player2;
        Boss = _Boss;
        Board = FindObjectOfType<Board>();
        TurnTimer = FindObjectOfType<TurnTimer>();
        NextTurn();
    }

    public static async void NextTurn()
    {
        ActivePlayer?.SetUIActive(false);

        TurnTimer.Clear();

        IncrementActivePlayer();

        if (ActivePlayer.Token.Tile == null) await Board.PlaceToken(ActivePlayer.Token);

        if (ActivePlayer != Boss) ActivePlayer.SetUIActive(true);
    }
    public static async void EndGame()
    {
        Debug.Log("Game Over");
        if(Boss.Health == 0)
        {
            Debug.Log("You Win!");
        }
        else
        {
            Debug.Log("You Lose");
        }
        Application.Quit();
    }
    
    private static void IncrementActivePlayer()
    {
        if (ActivePlayer == Boss)
            ActivePlayer = Player1;
        else if (ActivePlayer == Player1)
            ActivePlayer = Player2;
        else
            ActivePlayer = Boss;
    }

}
