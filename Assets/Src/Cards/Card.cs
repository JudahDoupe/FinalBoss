using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Card : MonoBehaviour
{
    public void Click()
    {
        Play();
    }

    public virtual async void Play()
    {
    }

    public virtual async void Discard()
    {
    }
}
