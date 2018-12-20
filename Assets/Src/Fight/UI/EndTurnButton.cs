using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EndTurnButton : MonoBehaviour {

	public void Click()
    {
        GetComponentInParent<Player>()?.CmdEndTurn();
    }
}
