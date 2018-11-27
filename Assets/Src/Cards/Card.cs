using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Card : NetworkBehaviour
{
    protected bool IsBeingPlayed;

    public int SecondsToPlay = 0;
    public Player Player;
    public CardType Type;

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
               Player.TurnTimer.SecondIndex + SecondsToPlay <= Player.TurnTimer.Seconds.Count;
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

public enum CardType
{
    Movement,
    Attack,
    Special
}