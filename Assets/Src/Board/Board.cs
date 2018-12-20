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

    public static Dictionary<Player, TaskCompletionSource<Tile>> SelectedTiles = new Dictionary<Player, TaskCompletionSource<Tile>>();
    public static Dictionary<Player, Token> Tokens = new Dictionary<Player, Token>();
    public static Dictionary<TileCoord,Tile> Tiles = new Dictionary<TileCoord, Tile>(new TileCoordComparer());
    public static List<GameObject> TilePrefabs;
    public static List<GameObject> TokenPrefabs;

    void Start()
    {
        TilePrefabs = tilePrefabs;
        TokenPrefabs = tokenPrefabs;
    }

    public static async Task<Tile> SelectTile(Player player, List<Tile> tiles)
    {
        if (tiles.Count == 0) return null;
        foreach(var tile in tiles)
        {
            tile.TargetSetSelectable(player.connectionToClient, true);
        }

        await SelectedTiles[player].Task;
        var rtn = SelectedTiles[player].Task.Result;
        SelectedTiles[player] = new TaskCompletionSource<Tile>();

        foreach(var tile in tiles)
        {
            tile.TargetSetSelectable(player.connectionToClient, false);
        }
        return rtn;
    }

    public static void AddTile(TileCoord coord)
    {
        var tile = Instantiate(TilePrefabs[Random.Range(0, TilePrefabs.Count)]).GetComponent<Tile>();
        Tiles[coord] = tile;
        NetworkServer.Spawn(tile.gameObject);
        tile.RpcSetCoord(coord.R, coord.Q);
        tile.IsBuilt = true;
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

        var token = Instantiate(TokenPrefabs[TokenId]).GetComponent<Token>(); ;
        NetworkServer.Spawn(token.gameObject);
        token.SetCoord(null);
        Tokens.Add(player, token);
    }
    public static void MoveToken(Player player, TileCoord coord, bool snapToTile = false)
    {
        if(snapToTile)
            GetToken(player).SetCoord(coord);
        else
            GetToken(player).MoveToCoord(coord);
    }
    public static Token GetToken(Player player)
    {
        if (player == null)
        {
            Debug.Log("cannot get token of null player");
            return null;
        }
        if (!Tokens.ContainsKey(player))
        {
            Debug.Log("player has not registered a token");
            return null;
        }
        return Tokens[player];
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