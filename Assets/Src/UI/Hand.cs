using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Hand : StaticUI
{
    public Player Player;
    public List<Card> Cards = new List<Card>();
    public int NumCards { get { return Cards.Count; } }
    public Card SelectedCard;

    public void AddCard(Card card)
    {
        if (card == null) return;
        card.transform.parent = transform;
        Cards.Add(card);
    }
    public void RemoveCard(Card card)
    {
        Cards.Remove(card);
    }

    private void Update()
    {
        RemovePlayedCardsFromHand();
        Rect.anchoredPosition = Vector2.Lerp(Rect.anchoredPosition, new Vector2(0, IsHidden ? -60: 40), Time.smoothDeltaTime * 5);

        for (var i = 0; i < NumCards; i++)
        {
            var card = Cards[i];
            var cardDencity = card.Rect.sizeDelta.x * 0.75f;
            var horizontalOffset = i - (NumCards-1) / 2f;
            var verticalOffset = -3 * horizontalOffset * horizontalOffset;
            var targetPos = new Vector3(horizontalOffset * cardDencity , verticalOffset, i * -0.001f);
            var targetRot = new Vector3(0, 0, -5 * horizontalOffset);

            if (card == SelectedCard)
            {
                card.Rect.localScale = new Vector3(1.4f,1.4f,1.4f);
                targetPos.y = card.Rect.sizeDelta.y / 2;
                card.SnapTo(targetPos, Vector3.zero);
                card.transform.SetAsLastSibling();
            }
            else
            {
                card.MoveTo(targetPos,targetRot);
                card.transform.SetSiblingIndex(i);
                card.Rect.localScale = new Vector3(1,1,1);
            }

        }
    }

    private void RemovePlayedCardsFromHand()
    {
        foreach (var card in Cards.Where(x => x.transform.parent != transform).ToArray())
        {
            Cards.Remove(card);
        }
    }
}
