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
    public GameObject connectionPrefab;
    private static List<GameObject> _tilePrefabs;
    private static List<GameObject> _tokenPrefabs;
    private static GameObject _connectionPrefab;

    public static Dictionary<Player, Token> PlayerTokens = new Dictionary<Player, Token>();
    public static Dictionary<TileCoord,Tile> Tiles = new Dictionary<TileCoord, Tile>(new TileCoordComparer());

    void Start()
    {
        _tilePrefabs = tilePrefabs;
        _tokenPrefabs = tokenPrefabs;
        _connectionPrefab = connectionPrefab;
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
            RemoveTile(tile.Key);
        }
    }

    public static void AddTile(TileCoord coord)
    {
        var tile = Instantiate(_tilePrefabs[Random.Range(0, _tilePrefabs.Count)]).GetComponent<Tile>();
        Tiles[coord] = tile;
        NetworkServer.Spawn(tile.gameObject);
        tile.RpcMove(coord.R, coord.Q);

        Tuple<ConnectionDirection, TileCoord>[] directions = new[]
        {
            new Tuple<ConnectionDirection, TileCoord>(ConnectionDirection.East, new TileCoord(1,0)), 
            new Tuple<ConnectionDirection, TileCoord>(ConnectionDirection.SouthEast, new TileCoord(0,1)), 
            new Tuple<ConnectionDirection, TileCoord>(ConnectionDirection.SouthWest, new TileCoord(-1,1)), 
            new Tuple<ConnectionDirection, TileCoord>(ConnectionDirection.West, new TileCoord(-1,0)), 
            new Tuple<ConnectionDirection, TileCoord>(ConnectionDirection.NorthWest, new TileCoord(0,-1)), 
            new Tuple<ConnectionDirection, TileCoord>(ConnectionDirection.NorthEast, new TileCoord(1,-1)), 
        };

        foreach (var (dir, dirCoord) in directions)
        {
            var neighborCoord = new TileCoord(coord.R + dirCoord.R, coord.Q + dirCoord.Q);
            if (Tiles.ContainsKey(neighborCoord))
            {
                var neighborTile = Tiles[neighborCoord];
                var connection = Instantiate(_connectionPrefab).GetComponent<Connection>();
                NetworkServer.Spawn(connection.gameObject);
                tile.Connections[(int) dir] = connection;
                neighborTile.Connections[((int)dir + 3) % 6] = connection;
                connection.Left = tile;
                connection.Right = neighborTile;
            }
        }
    }
    public static void RemoveTile(TileCoord coord)
    {
        var tile = GetTile(coord);
        if (tile == null) return;
        foreach (var tileConnection in tile.Connections)
        {
            if(tileConnection != null) Destroy(tileConnection.gameObject);
        }
        Tiles.Remove(coord);
        Destroy(tile.gameObject);
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
        var tile = GetTile(coord);
        return tile.Connections.Select(conn => conn.From(tile)).ToList();
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