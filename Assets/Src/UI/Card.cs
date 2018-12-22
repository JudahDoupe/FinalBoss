using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Card : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public string Name;
    public ActionType Type;

    [HideInInspector]
    public Player Player;

    public bool Selected { get; private set; }
    public RectTransform UITransform { get; private set; }
    private Vector3 _targetPos;
    private Quaternion _targetRot;

    void Awake()
    {
        UITransform = transform.GetComponent<RectTransform>();
    }
    void Update()
    {
        UITransform.anchoredPosition = Vector3.Lerp(transform.localPosition, _targetPos, Time.deltaTime * 5);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _targetRot, Time.deltaTime * 100);
    }

    public void Play()
    {
        Player.CmdPlayCard(Name);
        Player.Hand.CardBeingPlayed = this;
    }
    public void Discard()
    {
        Player.Decks[Type].Discard(this);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Selected = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Selected = false;
    }
}
