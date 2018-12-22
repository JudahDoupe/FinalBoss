using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCounter : MonoBehaviour {

    public Color Movement;
    public Color Attack;
    public Color Special;
    public Color Neutral;

    public List<Image> Actions;

    private Player _player;
    private RectTransform _uiTransform;
    private Vector2 _targetPos;

    void Start()
    {
        _uiTransform = GetComponent<RectTransform>();
        _player = GetComponentInParent<Player>();
        _targetPos = _uiTransform.anchoredPosition;
    }
    void Update()
    {
        _uiTransform.anchoredPosition = Vector2.Lerp(_uiTransform.anchoredPosition, _targetPos, Time.smoothDeltaTime * 5);

        for(int i = 0; i < Actions.Count; i++)
        {
            if (i >= _player.Actions.Count)
            {
                Actions[i].color = Neutral;
            }
            else
            {
                switch (_player.Actions[i])
                {
                    case ActionType.Movement:
                        Actions[i].color = Movement;
                        break;
                    case ActionType.Attack:
                        Actions[i].color = Attack;
                        break;
                    case ActionType.Special:
                        Actions[i].color = Special;
                        break;
                    case ActionType.Neutral:
                        Actions[i].color = Neutral;
                        break;
                    default:
                        Actions[i].color = Neutral;
                        break;
                }
            }
        }
    }

    public void SetVisible(bool isVisible)
    {
        _targetPos = new Vector2(0, isVisible ? -10 : 10);
    }
}