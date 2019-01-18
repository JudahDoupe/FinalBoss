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
    public Loading Loading;

    public void HideAll()
    {
        SetLoadingVisible(false);
        SetHandVisible(false);
        SetActionTimerVisible(false);
        SetDecksVisible(false);
        SetButtonsVisible(false);
    }

    public void ShowLoadingState()
    {
        SetLoadingVisible(true);
        SetHandVisible(false);
        SetActionTimerVisible(false);
        SetDecksVisible(false);
        SetButtonsVisible(false);
    }
    public void ShowDrawState()
    {
        SetLoadingVisible(false);
        SetHandVisible(true);
        SetActionTimerVisible(false);
        SetDecksVisible(true);
        SetButtonsVisible(true);
    }
    public void ShowObserveState()
    {
        SetLoadingVisible(false);
        SetHandVisible(false);
        SetActionTimerVisible(true);
        SetDecksVisible(false);
        SetButtonsVisible(false);
    }
    public void ShowPlayState()
    {
        SetLoadingVisible(false);
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
    public void SetLoadingVisible(bool isVisible)
    {
        Loading.SetVisible(isVisible);
    }
}
