﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 Offset = new Vector3(0, 2, -2);

    void Update()
    {
        if(Fight.ActivePlayer?.Token.Tile != null)
            transform.position = Vector3.Lerp(transform.position, Fight.ActivePlayer.Token.transform.position + Offset, Time.deltaTime);
    }
}