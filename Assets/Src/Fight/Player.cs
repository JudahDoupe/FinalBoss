using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public Token Token;

    public Deck MovementDeck;
    public Deck AttackDeck;
    public Deck SpecialDeck;
    public TurnTimer TurnTimer;

    public Hand Hand;

    public bool IsAI = false;
    public int Health = 20;
    public int Initiative = 0;

    public void Start()
    {
        CmdJoinGame();
    }

    public void SetUiActive(bool isActive)
    {
        Hand?.gameObject.SetActive(isActive);
        AttackDeck?.gameObject.SetActive(isActive);
        MovementDeck?.gameObject.SetActive(isActive);
        SpecialDeck?.gameObject.SetActive(isActive);
        TurnTimer?.gameObject.SetActive(isActive);
    }

    public void Damage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Health = 0;
        CmdEndGame();
    }

    /* MESSAGES TO SERVER */

    [Command]
    private void CmdJoinGame()
    {
        Fight.Join(this);
        Token = Instantiate(Token);
        NetworkServer.Spawn(Token.gameObject);
        Token.Coord = null;
    }
    [Command]
    private void CmdEndGame()
    {
        Fight.EndGame();
    }
    [Command]
    private async void CmdPlaceToken()
    {
        SetUiActive(false);
        var tile = await Board.SelectTile(Board.GetAllTiles());
        Token.SetCoord(tile.Coord);
        SetUiActive(true);
    }

    /* MESSAGES FROM SERVER */

    [ClientRpc]
    public void RpcStartTurn()
    {
        SetUiActive(true);
        if (Token.Coord == null) CmdPlaceToken();
    } 
    [ClientRpc]
    public void RpcEndTurn()
    {
        SetUiActive(false);
    }
}
