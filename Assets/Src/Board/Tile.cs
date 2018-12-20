using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

public class Tile : NetworkBehaviour
{
    public static float Size = 1;
    public TileCoord Coord;
    [SyncVar]
    public bool IsBuilt;
    public bool IsSelectable;

    private GameObject _model;
    private GameObject _selector;

    void Start()
    {
        _model = transform.Find("Model").gameObject;
        _selector = transform.Find("Selector").gameObject;
    }
    void Update()
    {
        _selector.SetActive(IsSelectable);
        _model.SetActive(IsBuilt);
    }

    public void SetCoord(TileCoord coord)
    { 
        Coord = coord;
        transform.position = Coord.Position;
    }
    
    public void SelectTile()
    {
        Player.LocalPlayer.CmdSelectTile(Coord.R, Coord.Q);
    }

    /* MESSAGES FROM SERVER */

    [TargetRpc]
    public void TargetSetSelectable(NetworkConnection connectionToClient, bool isSelectable)
    {
        IsSelectable = isSelectable;
    }

}

[Serializable]
public class TileCoord
{
    //Cubic Coordinates
    public int X => Q;
    public int Y => - Q - R;
    public int Z => R;

    //Axial Coordinates
    public int R { get; }
    public int Q { get; }

    //World Coordinates
    private float VerticalOffset { get; }
    private Vector3 Axis => new Vector3(0.5f, 0, 0.866f).normalized;
    public Vector3 Position => Axis * R + Vector3.right * Q + Vector3.up * VerticalOffset;

    public TileCoord(int x, int y, int z)
    {
        Q = x;
        R = z;
        var r = new Random();
        VerticalOffset = r.Next(-300, 300) * 0.0001f;
    }
    public TileCoord(int r, int q)
    {
        R = r;
        Q = q;
        var random = new Random();
        VerticalOffset = random.Next(-300, 300) * 0.0001f;
    }

}
