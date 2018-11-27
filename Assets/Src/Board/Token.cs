using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

public class Token : NetworkBehaviour
{
    [CanBeNull] [HideInInspector]
    public TileCoord Coord;
    [HideInInspector]
    public Player Player;
    public float Speed = 1;

    private GameObject _model;

    private void Start()
    {
        _model = transform.Find("Model").gameObject;
    }

    private void Update ()
    {
        RpcUpdateTokenPosition();
    }

    public void SetCoord(TileCoord coord)
    {
        Coord = coord;
        transform.position = coord.Position;
    }

    /* MESSAGES FROM SERVER */

    [ClientRpc]
    public void RpcUpdateTokenPosition()
    {
        if (Coord == null)
        {
            _model.SetActive(false);
        }
        else
        {
            _model.SetActive(true);
            transform.rotation = Player.Camera.transform.rotation;

            if (Vector3.Distance(transform.position, Coord.Position) > Speed * Time.deltaTime)
            {
                var dir = Vector3.Normalize(Coord.Position - transform.position);
                transform.Translate(dir * Speed * Time.deltaTime, Space.World);
            }
        }
    }
}
