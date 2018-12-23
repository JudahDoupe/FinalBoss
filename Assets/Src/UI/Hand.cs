using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Hand : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();
    public Card CardBeingPlayed;

    private Player _player;
    private RectTransform _uiTransform;
    private Vector2 _targetPos;
    private bool _isVisible;

    void Awake()
    {
        _uiTransform = GetComponent<RectTransform>();
        _player = GetComponentInParent<Player>();
    }
    void Update()
    {
        _uiTransform.anchoredPosition = Vector2.Lerp(_uiTransform.anchoredPosition, _targetPos, Time.smoothDeltaTime * 5);

        for (var i = 0; i < Cards.Count; i++)
        {
            var card = Cards[i];
            var cardDencity = card.UITransform.sizeDelta.x * 0.75f;
            var horizontalOffset = i - (Cards.Count - 1) / 2f;
            var verticalOffset = -3 * horizontalOffset * horizontalOffset;
            var targetPos = new Vector3(horizontalOffset * cardDencity , verticalOffset, i * -0.001f);
            var targetRot = new Vector3(0, 0, -5 * horizontalOffset);

            if ((IsPlayable() && card.IsHighlighted) || card == CardBeingPlayed)
            {
                targetPos.y = card.UITransform.sizeDelta.y / 2;
                card.SnapTo(targetPos, Vector3.zero);
                card.transform.SetAsLastSibling();
                card.UITransform.localScale = new Vector3(1.4f,1.4f,1.4f);
            }
            else
            {
                card.MoveTo(targetPos,targetRot);
                card.transform.SetSiblingIndex(i);
                card.UITransform.localScale = new Vector3(1,1,1);
            }

        }
    }

    public void AddCard(Card card)
    {
        if (card == null) return;
        card.transform.parent = transform;
        Cards.Add(card);
    }
    public void DiscardActiveCard()
    {
        Cards.Remove(CardBeingPlayed);
        CardBeingPlayed.Discard();
        CardBeingPlayed = null;
    }
    public void SetVisible(bool isVisible)
    {
        _targetPos = new Vector2(0, isVisible ? 40 : -60);
        this._isVisible = isVisible;
    }

    public bool IsPlayable()
    {
        return _isVisible && CardBeingPlayed == null;
    }
}
