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
    public List<ActionType> Actions = new List<ActionType>();

    public static Player LocalPlayer { get { return FindObjectsOfType<Player>().FirstOrDefault(x => x.isLocalPlayer); } }

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

        if (!isLocalPlayer)
        {
            Camera.gameObject.SetActive(false);
        }
    }
    private IEnumerator DrawCards()
    {
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
    public void CmdEndTurn()
    {
        Fight.EndTurn();
    }
    [Command]
    public void CmdSelectTile(int r, int q)
    {
        var tile = Board.GetTile(new TileCoord(r, q));
        Board.SelectedTiles[this].SetResult(tile);
    }
    [Command]
    public void CmdPlayCard(string cardName)
    {
        Fight.PlayCard(connectionToClient, cardName);
    }

    [Command]
    private void CmdJoinGame()
    {
        if (Fight.Players.Contains(this)) return;
        Fight.JoinFight(this);
    }
    [Command]
    private void CmdEndGame()
    {
        Fight.EndFight();
    }
    [Command]
    private async void CmdSetupBoard()
    {
        if (Board.GetToken(this).Coord == null)
        {
            var tile = await Board.SelectTile(this, Board.GetAllTiles());
            Board.MoveToken(this,tile.Coord,true);
        }
        TargetStartDrawPhase(connectionToClient);
    }

    /* MESSAGES FROM SERVER */

    [TargetRpc]
    public void TargetStartTurn(NetworkConnection connectionToClient)
    {
        CmdSetupBoard();
    }
    [TargetRpc]
    public void TargetStartDrawPhase(NetworkConnection connectionToClient)
    {
        SetUiActive(true);
        StartCoroutine(DrawCards());
    }

    [TargetRpc]
    public void TargetEndTurn(NetworkConnection connectionToClient)
    {
        SetUiActive(false);
        SetDecksActive(false);
    }
    [TargetRpc]
    public void TargetDamage(NetworkConnection connectionToClient, int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
            CmdEndGame();
        }
    }
    [TargetRpc]
    public void TargetPlayCard(NetworkConnection connectionToClient, bool isCardPlayable)
    {
        if (isCardPlayable)
        {
            Hand.SelectedCard.Discard();
        }
        Hand.SelectedCard = null;
    }

    [ClientRpc]
    public void RpcAddAction(ActionType type)
    {
        Actions.Add(type);
    }
    [ClientRpc]
    public void RpcClearActions()
    {
        Actions = new List<ActionType>();
    }
}
