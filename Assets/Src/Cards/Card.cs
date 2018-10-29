using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int SecondsToPlay = 0;
    public Player Player;

    public void Click()
    {
        if(IsPlayable())
            Play();
    }

    public virtual async void Play()
    {
    }

    public virtual async void Discard()
    {
    }

    public virtual bool IsPlayable()
    {
        return Fight.TurnTimer.SecondIndex + SecondsToPlay <= Fight.TurnTimer.Seconds.Count;
    }
}
