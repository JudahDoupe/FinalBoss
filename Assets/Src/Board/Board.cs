using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour {

    public TileCoord Player1Spawn;
    public TileCoord Player2Spawn;
    public TileCoord BossSpwan;

    public TaskCompletionSource<Tile> SelectedTile = new TaskCompletionSource<Tile>();
    public Dictionary<TileCoord,Tile> Tiles;
    public List<GameObject> TilePrefabs;

    void Start()
    {
        if (Tiles == null)
        {
            Tiles = new Dictionary<TileCoord, Tile>(new TileCoordComparer());
        }

        var size = 10;
        for (int r = -size; r < size; r++)
        {
            for (int q = -size; q < size; q++)
            {
                AddTile(new TileCoord(r,q));
            }
        }

        MoveToken(Fight.Player1.Token, Player1Spawn);
    }

    public void AddTile(TileCoord coord)
    {
        var tile = Instantiate(TilePrefabs[Random.Range(0, TilePrefabs.Count)],transform).GetComponent<Tile>();

        var axis = new Vector3(0.5f, 0, 0.866f).normalized;
        tile.transform.position = axis * coord.R + Vector3.right * coord.Q + Vector3.up * Random.Range(-0.03f, 0.03f);

        tile.Coord = coord;
        tile.IsBuilt = true;
        Tiles[coord] = tile;
    }
    public void RemoveTile(TileCoord coord)
    {
        var tile = GetTile(coord);
        if(tile != null) Destroy(tile.gameObject);
    }
    public Tile GetTile(TileCoord coord)
    {
        if (coord == null) return null;
        Tile tile;
        Tiles.TryGetValue(coord, out tile);
        return tile;
    }
    public List<Tile> GetTilesWithinRadius(int radius, TileCoord coord)
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

    public async Task<Tile> SelectTile(List<Tile> tiles)
    {
        if (tiles.Count == 0) return null;
        foreach(var tile in tiles)
        {
            tile.IsSelectable = true;
        }

        await SelectedTile.Task;
        var rtn = SelectedTile.Task.Result;
        SelectedTile = new TaskCompletionSource<Tile>();

        foreach(var tile in tiles)
        {
            tile.IsSelectable = false;
        }
        return rtn;
    }

    private void MoveToken(Token token, TileCoord coord)
    {
        var tile = GetTile(coord) ?? Tiles.RandomValue();
        token.Tile = tile;
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