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
    private int index;

    private RectTransform _uiTransform;
    private Vector2 _targetPos;

    void Awake()
    {
        _uiTransform = GetComponent<RectTransform>();
        _targetPos = _uiTransform.anchoredPosition;
    }
    void Update()
    {
        _uiTransform.anchoredPosition = Vector2.Lerp(_uiTransform.anchoredPosition, _targetPos, Time.smoothDeltaTime * 5);
    }

    public void AddAction(ActionType type)
    {
        switch (type)
        {
            case ActionType.Movement:
                Actions[index].color = Movement;
                break;
            case ActionType.Attack:
                Actions[index].color = Attack;
                break;
            case ActionType.Special:
                Actions[index].color = Special;
                break;
            case ActionType.Neutral:
                Actions[index].color = Neutral;
                break;
            default:
                Actions[index].color = Neutral;
                break;
        }
        index++;
    }
    public void ClearActions()
    {
        foreach (var action in Actions)
        {
            action.color = Neutral;
        }

        index = 0;
    }

    public void SetVisible(bool isVisible)
    {
        _targetPos = new Vector2(0, isVisible ? -10 : 10);
    }
}