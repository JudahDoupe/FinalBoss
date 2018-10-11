using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static float Size = 1;

    public TileCoord Coord;
    public bool IsSelectable;
    public bool IsBuilt;

    private Board _board;
    private GameObject _model;
    private GameObject _selector;

    void Start()
    {
        _board = transform.parent.GetComponent<Board>();
        _model = transform.Find("Model").gameObject;
        _selector = transform.Find("Selector").gameObject;
    }
    void Update()
    {
        _selector.SetActive(IsSelectable);
        _model.SetActive(IsBuilt);
    }

}

[Serializable]
public class TileCoord
{
    //Cubic Coordinates
    public int X { get { return Q; } }
    public int Y { get { return - Q - R; } }
    public int Z { get { return R; } }

    //Axial Coordinates
    public int R;
    public int Q;

    public TileCoord(int x, int y, int z)
    {
        Q = x;
        R = z;
    }
    public TileCoord(int r, int q)
    {
        R = r;
        Q = q;
    }


}
