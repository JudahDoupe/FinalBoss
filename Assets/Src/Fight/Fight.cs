using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        ActivePlayer?.SetActive(false);
        TurnTimer.Clear();

        if (ActivePlayer == Boss)
            ActivePlayer = Player1;
        else if (ActivePlayer == Player1)
            ActivePlayer = Player2;
        else
            ActivePlayer = Boss;
        
        if(ActivePlayer.Token.Tile == null)
        {
            Tile tile;
            if (ActivePlayer == Boss)
                tile = Board.GetRandomTile();
            else
                tile = await Board.SelectTile(Enumerable.ToList(Board.Tiles.Values).ToList());
            ActivePlayer.Token.Tile = tile;
            ActivePlayer.Token.transform.position = tile.transform.position;
        }


        if (ActivePlayer != Boss)
            ActivePlayer.SetActive(true);
    }

}
