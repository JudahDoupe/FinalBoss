using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnTimer : NetworkBehaviour {

    public Material MovementMaterial;
    public Material AttackMaterial;
    public Material SpecialMaterial;
    public Material NeutralMaterial;

    public List<Renderer> Seconds;

    public void Update()
    {
        for (var i = 0; i < Fight.TurnActions.Length; i++)
        {
            Material mat;
            switch (Fight.TurnActions[i])
            {
                case ActionType.Movement:
                    mat = MovementMaterial;
                    break;
                case ActionType.Attack:
                    mat = AttackMaterial;
                    break;
                case ActionType.Special:
                    mat = SpecialMaterial;
                    break;
                case ActionType.Neutral:
                    mat = NeutralMaterial;
                    break;
                default:
                    mat = NeutralMaterial;
                    break; 
            }
            Seconds[i].material = mat;
        }
    }

}