using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int Health = 20;
    [SyncVar]
    public int Initiative = 0;

    public List<Deck> Decks;
    public Hand Hand;
    public UIState UI;
    public Camera Camera;

    public static Player LocalPlayer { get { return FindObjectsOfType<Player>().FirstOrDefault(x => x.isLocalPlayer); } }

    private TaskCompletionSource<Tile> SelectedTile = new TaskCompletionSource<Tile>();

    private void Start()
    {
        UI.ShowLoadingState();
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

    /* Should only be run on server */

    public async Task<Tile> SelectTile(List<Tile> options)
    {
        if (options.Count == 0)
        {
            Debug.Log("Cannot select tile is no ooptions are provided");
            return null;
        }
        foreach (var tile in options)
        {
            tile.TargetIsSelectable(connectionToClient, true);
        }

        SelectedTile = new TaskCompletionSource<Tile>();
        await SelectedTile.Task;
        var rtn = SelectedTile.Task.Result;
    
        foreach (var tile in options)
        {
            tile.TargetIsSelectable(connectionToClient, false);
        }
        return rtn;
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
            var tile = await SelectTile(Board.GetAllTiles());
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
        SelectedTile.SetResult(tile);
    }
    [Command]
    public void CmdPlayCard(string cardName)
    {
        CardExecutor.PlayCard(connectionToClient, cardName);
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
