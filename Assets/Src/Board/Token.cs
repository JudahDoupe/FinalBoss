using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    [HideInInspector]
    public Tile Tile;
    public float Speed = 1;

    private GameObject _model;

    private void Start()
    {
        _model = transform.Find("Model").gameObject;
    }

    void Update () {
        if(Tile == null)
        {
            _model.SetActive(false);
        }
        else
        {
            _model.SetActive(true);
            transform.LookAt(Camera.main.transform);

            

	        if (Vector3.Distance(transform.position, Tile.transform.position) > Speed * Time.deltaTime)
	        {
	            var dir = Vector3.Normalize(Tile.transform.position - transform.position);
	            transform.Translate(dir * Speed * Time.deltaTime,Space.World);
	        }
        }
	}
}
