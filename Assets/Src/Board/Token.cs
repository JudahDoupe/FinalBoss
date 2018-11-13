using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

public class Token : NetworkBehaviour
{
    [CanBeNull] [HideInInspector]
    public TileCoord Coord;
    public float Speed = 1;

    private GameObject _model;

    private void Start()
    {
        _model = transform.Find("Model").gameObject;
    }

    void Update () {
        if(Coord == null)
        {
            _model.SetActive(false);
        }
        else
        {
            _model.SetActive(true);
            transform.rotation = Camera.main.transform.rotation;

            if (Vector3.Distance(transform.position, Coord.Position) > Speed * Time.deltaTime)
	        {
	            var dir = Vector3.Normalize(Coord.Position - transform.position);
	            transform.Translate(dir * Speed * Time.deltaTime,Space.World); 
	        } 
        }
	}

    public void SetCoord(TileCoord coord)
    {
        Coord = coord;
        transform.position = Coord.Position;
    }
}
