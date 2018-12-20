using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Vector3 Offset = new Vector3(0, 2, -2);

    void LateUpdate()
    {
        if(Board.GetToken(Fight.ActivePlayer) != null)
            transform.position = Vector3.Lerp(transform.position, Board.GetToken(Fight.ActivePlayer).transform.position + Offset, Time.deltaTime);
    }
}
