using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Player Player;
    public List<Card> Cards = new List<Card>();
    public int NumCards { get { return Cards.Count; } }

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
        var highlightedCard = GetSelectedCard() ?? GetCardBeingHoveredOver();

        for (var i = 0; i < NumCards; i++)
        {
            var card = Cards[i];
            var cardDencity = card.transform.localScale.x * 0.75f;
            var horizontalOffset = i - (NumCards-1) / 2f;
            var verticalOffset = -0.0025f * horizontalOffset * horizontalOffset;
            var targetPos = new Vector3(horizontalOffset * cardDencity , verticalOffset, i * -0.001f);
            var targetRot = new Vector3(0, 0, -5 * horizontalOffset);

            if (card == highlightedCard)
            {
                var transformation = new Vector3(0,0.084f,-0.1f);
                var scale = new Vector3(0.75f, 0, 0);
                targetPos = Vector3.Scale(targetPos, scale) + transformation;
                Cards[i].SnapTo(targetPos, Vector3.zero);
            }
            else
            {
                Cards[i].MoveTo(targetPos,targetRot);
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
    private Card GetCardBeingHoveredOver()
    {
        RaycastHit hit;
        Ray ray = Player.Camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        return hit.collider?.gameObject.GetComponent<Card>();
    }
    private Card GetSelectedCard()
    {
        return Cards.FirstOrDefault(x => x.IsBeingPlayed);
    }
}
