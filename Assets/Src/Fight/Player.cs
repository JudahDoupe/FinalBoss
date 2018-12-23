using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer { get { return FindObjectsOfType<Player>().FirstOrDefault(x => x.isLocalPlayer); } }

    public List<Deck> Decks;
    public Hand Hand;
    public UIState UI;
    public Camera Camera;

    [SyncVar]
    public int Health = 20;
    [SyncVar]
    public int Initiative = 0;

    private void Start()
    {
        UI.HideAll();
        CmdJoinGame();

        if (!isLocalPlayer) Camera.gameObject.SetActive(false);
    }
    private IEnumerator DrawCards()
    {
        if (Hand.Cards.Count < 6)
        {
            UI.ShowDrawState();
            yield return new WaitUntil(() => Hand.Cards.Count >= 6);
        }
        UI.ShowPlayState();
    }

    /* MESSAGES TO SERVER */

    [Command]
    private void CmdJoinGame()
    {
        if (Fight.Players.Contains(this)) return;
        Fight.JoinFight(this);
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
    [Command]
    public void CmdEndTurn()
    {
        Fight.NextTurn();
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

    /* MESSAGES FROM SERVER */

    [TargetRpc]
    public void TargetStartTurn(NetworkConnection connectionToClient)
    {
        CmdSetupBoard();
    }
    [TargetRpc]
    public void TargetStartDrawPhase(NetworkConnection connectionToClient)
    {
        foreach (var deck in Decks)
        {
            deck.Shuffle();
        }
        StartCoroutine(DrawCards());
    }
    [TargetRpc]
    public void TargetWatchGame(NetworkConnection connectionToClient)
    {
        UI.ShowObserveState();
    }
    [TargetRpc]
    public void TargetFinishPlayingCard(NetworkConnection connectionToClient, bool cardWasPlayed)
    {
        if (cardWasPlayed)
        {
            Hand.DiscardActiveCard();
        }
        Hand.CardBeingPlayed = null;
        Hand.SetVisible(true);
    }

    [ClientRpc]
    public void RpcAddAction(ActionType type)
    {
        UI.ActionCounter.AddAction(type);
    }
    [ClientRpc]
    public void RpcClearActions()
    {
        UI.ActionCounter.ClearActions();
    }
}
