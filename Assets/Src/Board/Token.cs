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
        Coord = null;
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
            var targetRotation = FindObjectOfType<Camera>()?.transform.eulerAngles ?? transform.eulerAngles;
            targetRotation.Scale(new Vector3(0, 1, 0));
            transform.eulerAngles = targetRotation;

            if (Vector3.Distance(transform.position, Coord.Position) > Speed * Time.deltaTime)
            {
                var dir = Vector3.Normalize(Coord.Position - transform.position);
                transform.Translate(dir * Speed * Time.deltaTime, Space.World);
            }
        }
    }

    [ClientRpc]
    public void RpcSetCoord(int r, int q)
    {
        Coord = new TileCoord(r,q);
        transform.position = Coord.Position;
    }
    [ClientRpc]
    public void RpcMoveToCoord(int r, int q)
    {
        Coord = new TileCoord(r, q);
    }
    [ClientRpc]
    public void RpcMoveAlongPath(int[] rs, int[] qs)
    {
        var coords = new List<TileCoord>();
        for (int i = 0; i < rs.Length; i++)
        {
            coords.Add(new TileCoord(rs[i], qs[i]));
        }
        StartCoroutine(MoveAlongPath(coords));
    }
    [ClientRpc]
    public void RpcClearCoord()
    {
        Coord = null;
    }

    private IEnumerator MoveAlongPath(List<TileCoord> coords)
    {
        foreach (var tileCoord in coords)
        {
            Coord = tileCoord;
            while (Vector3.Distance(transform.position, Coord.Position) > 0.05f)
            {
                yield return new WaitForUpdate();
            }
        }
    }
}
