using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class Board : NetworkBehaviour
{
    public List<GameObject> tilePrefabs;
    private static Board _instance;

    public static TaskCompletionSource<Tile> SelectedTile = new TaskCompletionSource<Tile>();
    public static Dictionary<TileCoord,Tile> Tiles = new Dictionary<TileCoord, Tile>(new TileCoordComparer());
    public static List<GameObject> TilePrefabs;

    void Start()
    {
        TilePrefabs = tilePrefabs;
        _instance = this;
        var size = 10;
        for (int x = -size; x <= size; x++)
        {
            for (int y = -size; y <= size; y++)
            {
                for (int z = -size; z <= size; z++)
                {
                    if(x+y+z == 0)AddTile(new TileCoord(x, y,z));
                }
            }
        }
    }

    public static async Task<Tile> SelectTile(List<Tile> tiles)
    {
        if (tiles.Count == 0) return null;
        foreach(var tile in tiles)
        {
            tile.RpcSetSelectable(true);
        }

        await SelectedTile.Task;
        var rtn = SelectedTile.Task.Result;
        SelectedTile = new TaskCompletionSource<Tile>();

        foreach(var tile in tiles)
        {
            tile.RpcSetSelectable(false);
        }
        return rtn;
    }

    public static void AddTile(TileCoord coord)
    {
        var tile = Instantiate(TilePrefabs[Random.Range(0, TilePrefabs.Count)],_instance.transform).GetComponent<Tile>();
        NetworkServer.Spawn(tile.gameObject);

        tile.SetCoord(coord);
        tile.IsBuilt = true;
        Tiles[coord] = tile;
    }
    public static void RemoveTile(TileCoord coord)
    {
        var tile = GetTile(coord);
        if(tile != null) Destroy(tile.gameObject);
    }
    public static Tile GetTile(TileCoord coord)
    {
        if (coord == null) return null;
        Tile tile;
        Tiles.TryGetValue(coord, out tile);
        return tile;
    }
    public static Tile GetRandomTile()
    {
        return Tiles.RandomValue();
    }
    public static List<Tile> GetNeighbors(TileCoord coord)
    {
        var tiles = GetTilesWithinRadius(1, coord);
        var center = GetTile(coord);
        if (center != null) tiles.Remove(center);
        return tiles;
    }
    public static List<Tile> GetTilesWithinRadius(int radius, TileCoord coord)
    {
        var tiles = new List<Tile>();
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    if (x + y + z == 0)
                    {
                        var newTile = GetTile(new TileCoord(x + coord.X, y + coord.Y, z + coord.Z));
                        if (newTile != null) tiles.Add(newTile);
                    }
                }
            }
        }
        return tiles;
    }
    public static List<Tile> GetAllTiles()
    {
        return Enumerable.ToList(Tiles.Values);
    }

}

public class TileCoordComparer : IEqualityComparer<TileCoord>
{
    public bool Equals(TileCoord x, TileCoord y)
    {
        return x.Q == y.Q && x.R == y.R;
    }

    public int GetHashCode(TileCoord obj)
    {
        return obj.Q.GetHashCode() + obj.R.GetHashCode();
    }
}