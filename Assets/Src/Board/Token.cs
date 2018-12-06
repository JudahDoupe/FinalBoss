using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (Coord == null)
        {
            _model.SetActive(false);
        }
        else
        {
            _model.SetActive(true);
            transform.rotation = Fight.Players.First(x => x.GetComponent<NetworkIdentity>().isLocalPlayer).Camera.transform.rotation;

            if (Vector3.Distance(transform.position, Coord.Position) > Speed * Time.deltaTime)
            {
                var dir = Vector3.Normalize(Coord.Position - transform.position);
                transform.Translate(dir * Speed * Time.deltaTime, Space.World);
            }
        }
    }

    public void SetCoord(TileCoord coord)
    {
        Coord = coord;
        transform.position = coord.Position;
    }
}
