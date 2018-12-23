using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Card : Selectable
{
    public string Name;
    public ActionType Type;

    [HideInInspector]
    public Player Player;

    public new bool IsHighlighted { get; private set; }
    public RectTransform UITransform { get; private set; }
    private Vector3 _targetPos;
    private Quaternion _targetRot;

    private BaseEventData m_BaseEvent;

    void Awake()
    {
        UITransform = transform.GetComponent<RectTransform>();
    }
    void Update()
    {
        IsHighlighted = IsHighlighted(m_BaseEvent) || IsPressed();
        UITransform.anchoredPosition = Vector3.Lerp(transform.localPosition, _targetPos, Time.deltaTime * 5);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _targetRot, Time.deltaTime * 100);
    }

    public void Play()
    {
        if(!Player.Hand.IsPlayable()) return;
        Player.CmdPlayCard(Name);
        Player.Hand.CardBeingPlayed = this;
        Player.Hand.SetVisible(false);
    }
    public void Discard()
    {
        Player.Decks.First(d => d.Type == Type).Discard(this);
    }

    public void MoveTo(Vector3 position, Vector3 eulerAngle)
    {
        var rotation = Quaternion.Euler(eulerAngle);
        if (Vector3.Distance(position, _targetPos) < 0.00001f &&
            Quaternion.Angle(rotation, _targetRot) < 0.00001f) return;

        _targetPos = position;
        _targetRot = rotation;
    }
    public void SnapTo(Vector3 position, Vector3 eulerAngle)
    {
        MoveTo(position, eulerAngle);
        UITransform.anchoredPosition = _targetPos;
        transform.localRotation = _targetRot;
    }
    public void SetVisible(bool isVisible)
    {
        GetComponent<Button>().enabled = isVisible;
        GetComponent<Image>().enabled = isVisible;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isVisible);
        }
    }
}
