using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : NetworkBehaviour
{
    public Tile Left;
    public Tile Right;
    public bool Traversable => !_wall.activeSelf;

    private GameObject _wall;

    void Start()
    {
        _wall = transform.Find("Wall").gameObject;
        _wall.SetActive(false);
    }

    public Tile From(Tile from)
    {
        if (from == Left) return Right;
        else if (from == Right) return Left;
        else return null;
    }

    [ClientRpc]
    public void RpcSetPosition(int leftR, int leftQ, int rightR, int rightQ)
    {
        var left = new TileCoord(leftR,leftQ);
        var right = new TileCoord(rightR,rightQ);
        transform.position = Vector3.Lerp(left.Position, right.Position, 0.5f);
        transform.LookAt(left.Position);
    }
    [ClientRpc]
    public void RpcBuildWall()
    {
        _wall.SetActive(true);
    } 
}

public enum ConnectionDirection
{
    East = 0,
    SouthEast = 1,
    SouthWest = 2,
    West = 3,
    NorthWest = 4,
    NorthEast = 5,
}