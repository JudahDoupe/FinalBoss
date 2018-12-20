using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

public class Token : NetworkBehaviour
{
    [CanBeNull]
    public TileCoord Coord;
    [HideInInspector]
    public float Speed = 1;

    private GameObject _model;

    private void Start()
    {
        _model = transform.Find("Model").gameObject;
    }
    private void Update ()
    {
        if (Coord == null)
        {
            _model.SetActive(false);
        }
        else
        {
            _model.SetActive(true);
            var player = Fight.Players.FirstOrDefault(x => x.GetComponent<NetworkIdentity>().isLocalPlayer);
            if (player == null) return;
            transform.rotation = player.Camera.transform.rotation;

            if (Vector3.Distance(transform.position, Coord.Position) > Speed * Time.deltaTime)
            {
                var dir = Vector3.Normalize(Coord.Position - transform.position);
                transform.Translate(dir * Speed * Time.deltaTime, Space.World);
            }
        }
    }

    public void MoveToCoord(TileCoord coord)
    {
        if (isServer)
        {
            if(coord == null) RpcClearCoord();
            RpcMoveToCoord(coord.R,coord.Q);
        }
        else
        {
            Coord = coord;
            if(coord == null) CmdClearCoord();
            CmdMoveToCoord(coord.R, coord.Q);
        }
    }
    public void SetCoord(TileCoord coord)
    {
        if (isServer)
        {
            if(coord == null) RpcClearCoord();
            else RpcSetCoord(coord.R, coord.Q);
        }
        else
        {
            Coord = coord;
            if(coord == null) CmdClearCoord();
            else CmdMoveToCoord(coord.R, coord.Q);
        }
    }

    [ClientRpc]
    private void RpcSetCoord(int r, int q)
    {
        Coord = new TileCoord(r,q);
        transform.position = Coord.Position;
    }
    [ClientRpc]
    private void RpcMoveToCoord(int r, int q)
    {
        Coord = new TileCoord(r, q);
    }
    [ClientRpc]
    private void RpcClearCoord()
    {
        Coord = null;
    }

    [Command]
    private void CmdSetCoord(int r, int q)
    {
        Coord = new TileCoord(r, q);
        transform.position = Coord.Position;
        RpcSetCoord(r, q);
    }
    [Command]
    private void CmdMoveToCoord(int r, int q)
    {
        Coord = new TileCoord(r, q);
        RpcMoveToCoord(r, q);
    }
    [Command]
    private void CmdClearCoord()
    {
        Coord = null;
        RpcClearCoord();
    }
}
