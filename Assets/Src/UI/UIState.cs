using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIState : MonoBehaviour
{
    public ActionCounter ActionCounter;
    public GameObject Decks;
    public Hand Hand;
    public Button Next;

    public void HideAll()
    {
        SetHandVisible(false);
        SetActionTimerVisible(false);
        SetDecksVisible(false);
        SetButtonsVisible(false);
    }

    public void ShowDrawState()
    {
        SetHandVisible(true);
        SetActionTimerVisible(false);
        SetDecksVisible(true);
        SetButtonsVisible(true);
    }
    public void ShowObserveState()
    {
        SetHandVisible(false);
        SetActionTimerVisible(true);
        SetDecksVisible(false);
        SetButtonsVisible(false);
    }
    public void ShowPlayState()
    {
        SetButtonsVisible(true);
        SetActionTimerVisible(true);
        SetDecksVisible(false);
        SetHandVisible(true);
    }

    public void SetHandVisible(bool isVisible)
    {
        Hand.SetVisible(isVisible);
    }
    public void SetActionTimerVisible(bool isVisible)
    {
        ActionCounter.SetVisible(isVisible);
    }
    public void SetDecksVisible(bool isVisible)
    {
        Decks.SetActive(isVisible);
    }
    public void SetButtonsVisible(bool isVisible)
    {
        Next.gameObject.SetActive(isVisible);
    }
}
