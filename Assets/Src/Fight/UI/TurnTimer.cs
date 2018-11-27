using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnTimer : NetworkBehaviour {

    public Material DefaultMaterial;
    public Material MovementMaterial;
    public Material AttackMaterial;
    public Material SpecialMaterial;
    public Material SkipMaterial;

    public List<Renderer> Seconds;
    public int SecondIndex = 0;

    [ClientRpc]
    public void RpcClear()
    {
        foreach (var second in Seconds)
        {
            second.material = DefaultMaterial;
        }
        SecondIndex = 0;
    }
    [ClientRpc]
    public void RpcAddSecond(SecondType type)
    {
        Material mat;
        switch (type)
        {
            case SecondType.Movement:
                mat = MovementMaterial;
                break;
            case SecondType.Attack:
                mat = AttackMaterial;
                break;
            case SecondType.Special:
                mat = SpecialMaterial;
                break;
            case SecondType.Skip:
                mat = SkipMaterial;
                break;
            default:
                mat = DefaultMaterial;
                break; 
        }
        Seconds[SecondIndex].material = mat;
        SecondIndex++;

        if (SecondIndex >= 5)
            Fight.EndTurn();
    }

}
public enum SecondType
{
    Movement,
    Attack,
    Special,
    Skip
}