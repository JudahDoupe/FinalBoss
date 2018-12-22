using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCounter : StaticUI {

    public Color Movement;
    public Color Attack;
    public Color Special;
    public Color Neutral;

    public List<Image> Actions;

    public void Update()
    {
        Rect.anchoredPosition = Vector2.Lerp(Rect.anchoredPosition, new Vector2(0, IsHidden ? 10 : -10), Time.smoothDeltaTime * 5);

        for (int i = 0; i < Fight.ActionsPerTurn; i++)
        {
            Color color;
            try
            {
                switch (GetComponentInParent<Player>().Actions[i])
                {
                    case ActionType.Movement:
                        color = Movement;
                        break;
                    case ActionType.Attack:
                        color = Attack;
                        break;
                    case ActionType.Special:
                        color = Special;
                        break;
                    case ActionType.Neutral:
                        color = Neutral;
                        break;
                    default:
                        color = Neutral;
                        break;
                }
            }
            catch
            {
                color = Neutral;
            }
            Actions[i].color = color;
        }
    }
}