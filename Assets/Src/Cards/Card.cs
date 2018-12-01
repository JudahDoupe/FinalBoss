using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Card : UIObject
{
    public bool IsBeingPlayed;

    public int SecondsToPlay = 0;
    public Player Player;
    public ActionType Type;

    public void Click()
    {
        if (!IsPlayable()) return;

        StartCoroutine(Play());
    }

    public abstract void CmdPlay();

    public virtual void Discard()
    {
        Player.Decks[Type].Discard(this);
    }
    public virtual bool IsPlayable()
    {
        return !Player.IsPlayingCard &&
               Fight.ActionNumber + SecondsToPlay <= Fight.TurnActions.Length;
    }

    private IEnumerator Play()
    {
        Player.IsPlayingCard = IsBeingPlayed = true;
        CmdPlay();
        yield return new WaitUntil(() => IsBeingPlayed == false);
        Discard();
        Player.IsPlayingCard = false;
    }
}
