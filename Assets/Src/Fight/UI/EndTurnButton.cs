using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EndTurnButton : NetworkBehaviour {

	public void Click()
    {
        CmdEndTurn();
    }

    [Command]
    public void CmdEndTurn()
    {
        Fight.EndTurn();
    }
}
