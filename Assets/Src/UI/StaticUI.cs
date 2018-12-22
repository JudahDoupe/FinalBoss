using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticUI : MonoBehaviour
{
    public bool IsHidden;
    public RectTransform Rect;

    void Start()
    {
        Rect = GetComponent<RectTransform>();
    }
}
