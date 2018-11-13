using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour {

    private Tile _tile;

    void Start()
    {
        _tile = transform.parent.GetComponent<Tile>();
    }

	public void Click()
    {
        _tile.CmdSelectTile();
    }
}
