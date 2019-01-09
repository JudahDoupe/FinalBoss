using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public List<GameObject> tilePrefabs;
    public List<GameObject> tokenPrefabs;
    private static List<GameObject> _tilePrefabs;
    private static List<GameObject> _tokenPrefabs;

    public static Dictionary<Player, Token> PlayerTokens = new Dictionary<Player, Token>();
    public static Dictionary<TileCoord,Tile> Tiles = new Dictionary<TileCoord, Tile>(new TileCoordComparer());

    void Start()
    {
        _tilePrefabs = tilePrefabs;
        _tokenPrefabs = tokenPrefabs;
    }

    public static void Build()
    {
        var size = 10;
        for (int x = -size; x <= size; x++)
        {
            for (int y = -size; y <= size; y++)
            {
                for (int z = -size; z <= size; z++)
                {
                    if (x + y + z == 0) AddTile(new TileCoord(x, y, z));
                }
            }
        }
    }
    public static void Destroy()
    {
        foreach (var tile in Tiles)
        {
            Destroy(tile.Value);
        }
    }

    public static async Task<Tile> SelectTile(Player player, List<Tile> tiles)
    {
        if (tiles.Count == 0) return null;
        foreach(var tile in tiles)
        {
            tile.TargetIsSelectable(player.connectionToClient, true);
        }

        await player.SelectedTile.Task;
        var rtn = player.SelectedTile.Task.Result;
        player.SelectedTile = new TaskCompletionSource<Tile>();

        foreach(var tile in tiles)
        {
            tile.TargetIsSelectable(player.connectionToClient, false);
        }
        return rtn;
    }

    public static void AddTile(TileCoord coord)
    {
        var tile = Instantiate(_tilePrefabs[Random.Range(0, _tilePrefabs.Count)]).GetComponent<Tile>();
        Tiles[coord] = tile;
        NetworkServer.Spawn(tile.gameObject);
        tile.RpcMove(coord.R, coord.Q);
        tile.IsSolid = true;
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

    public static void AddToken(Player player, int TokenId)
    {
        if (GetToken(player) != null) return;

        var token = Instantiate(_tokenPrefabs[TokenId]).GetComponent<Token>(); ;
        NetworkServer.Spawn(token.gameObject);
        token.Coord = null;
        token.RpcClearCoord();
        PlayerTokens.Add(player, token);
    }
    public static void MoveToken(Player player, TileCoord coord, bool snapToTile = false)
    {
        var token = GetToken(player);
        token.Coord = coord;
        if (snapToTile)
            token.RpcSetCoord(coord.R, coord.Q);
        else
            token.RpcMoveToCoord(coord.R,coord.Q);
    }
    public static Token GetToken(Player player)
    {
        if (player == null)
        {
            Debug.Log("cannot get token of null player");
            return null;
        }
        if (!PlayerTokens.ContainsKey(player))
        {
            Debug.Log("player has not registered a token");
            return null;
        }
        return PlayerTokens[player];
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