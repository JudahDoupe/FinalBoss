using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    [HideInInspector]
    public Tile Tile;
    public float Speed = 1;

	void Update () {
	    transform.LookAt(Camera.main.transform);


        transform.position = Tile.transform.position;
        return;
	    if (Vector3.Distance(transform.position, Tile.transform.position) > Speed)
	    {
	        var dir = Vector3.Normalize(transform.position - Tile.transform.position);
	        transform.Translate(dir * Speed,Space.World);
	    }
	}
}
