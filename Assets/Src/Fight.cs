using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    public static Board Board;
    public static Player ActiveBoss;
    public static Player ActivePlayer;

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
        ActiveBoss = Boss;
        ActivePlayer = Player1;
    }
}
