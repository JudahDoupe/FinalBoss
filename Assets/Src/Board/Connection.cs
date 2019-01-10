using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : NetworkBehaviour
{
    public Tile Left;
    public Tile Right;

    void Update()
    {
        transform.position = Vector3.Lerp(Left.transform.position, Right.transform.position, 0.5f);
    }

    public Tile To(Tile from)
    {
        if (from == Left) return Right;
        else if (from == Right) return Left;
        else return null;
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