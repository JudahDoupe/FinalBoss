using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public Dictionary<ActionType, Deck> Decks = new Dictionary<ActionType, Deck>();
    public Hand Hand { get; private set; }
    public TurnTimer TurnTimer { get; private set; }
    public Camera Camera { get; private set; }
    public Token Token;

    [SyncVar]
    public int Health = 20;
    [SyncVar]
    public int Initiative = 0;
    [SyncVar]
    public bool IsPlayingCard = false;

    private void Start()
    {
        foreach (var deck in gameObject.GetComponentsInChildren<Deck>())
        {
            Decks.Add(deck.Type,deck);
        }
        Hand = gameObject.GetComponentInChildren<Hand>();
        Hand.Player = this;
        TurnTimer = gameObject.GetComponentInChildren<TurnTimer>();
        Camera = gameObject.GetComponentInChildren<Camera>();

        SetUiActive(false);
        SetDecksActive(false);
        CmdJoinGame();

        if (!GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            Camera.gameObject.SetActive(false);
        }
    }
    private IEnumerator SetupTurn()
    {
        //Setup
        if (Token.Coord == null)
        {
            CmdPlaceToken();
            yield return new WaitUntil(() => Token.Coord != null);
        }
        SetUiActive(true);

        //Draw Cards
        SetDecksActive(true);
        yield return new WaitUntil(() => Hand.NumCards >= 6);
        SetDecksActive(false);
    }
    private void SetUiActive(bool isActive)
    {
        Hand?.gameObject.SetActive(isActive);
        TurnTimer?.gameObject.SetActive(isActive);
    }
    private void SetDecksActive(bool isActive)
    {
        foreach (var deck in Decks.Values)
        {
            deck.gameObject.SetActive(isActive);
        }
    }

    /* MESSAGES TO SERVER */

    [Command]
    private void CmdJoinGame()
    {
        Token = Instantiate(Token);
        NetworkServer.Spawn(Token.gameObject);
        Token.Coord = null;
        Token.Player = this;
        CmdPlaceToken();
        Fight.JoinFight(this);
    }
    [Command]
    private void CmdEndGame()
    {
        Fight.EndFight();
    }
    [Command]
    private async void CmdPlaceToken()
    {
        var tile = await Board.SelectTile(connectionToClient, Board.GetAllTiles());
        Token.SetCoord(tile.Coord);
    }

    /* MESSAGES FROM SERVER */

    [ClientRpc]
    public void RpcStartTurn()
    {
        StartCoroutine(SetupTurn());
    } 
    [ClientRpc]
    public void RpcEndTurn()
    {
        SetUiActive(false);
    }
    [ClientRpc]
    public void RpcDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
            CmdEndGame();
        }
    }
}
