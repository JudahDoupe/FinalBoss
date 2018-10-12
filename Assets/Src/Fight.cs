using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fight : MonoBehaviour
{
    public static Board Board;
    public static Player ActivePlayer;

    public static Player Player1;
    public static Player Player2;
    public static Player Boss;

    public Player _Player1;
    public Player _Player2;
    public Player _Boss;

    public bool Next;

    void Start()
    {
        Player1 = _Player1;
        Player2 = _Player2;
        Boss = _Boss;
        Board = FindObjectOfType<Board>();
        ActivePlayer = Boss;
    }
    private void Update()
    {
        if (Next)
        {
            NextTurn();
            Next = false;
        }
    }

    public static async void NextTurn()
    {
        SetPlayerActive(ActivePlayer, false);

        if (ActivePlayer == Boss)
            ActivePlayer = Player1;
        else if (ActivePlayer == Player1)
            ActivePlayer = Player2;
        else if (ActivePlayer == Player2)
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

        if(ActivePlayer != Boss)
            SetPlayerActive(ActivePlayer, true);
    }

    private static void SetPlayerActive(Player player, bool isActive)
    {
        player.Hand?.gameObject.SetActive(isActive);
        player.AttackDeck?.gameObject.SetActive(isActive);
        player.MovementDeck?.gameObject.SetActive(isActive);
        player.SpecialDeck?.gameObject.SetActive(isActive);
    }
}
