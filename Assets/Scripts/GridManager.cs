using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : BaseBehavior {


    /// <summary>
    /// A representation of the game world grid, for use in pathfinding.
    /// </summary>
    public IEnumerable<PathingTile> TilePathing {
        get; private set;
    }

    public static GridManager Instance {
        get; private set;
    }

    protected override void OnStart() {
        var tilePathing = new List<PathingTile>();
        var tilemaps = GetComponentsInChildren<Tilemap>();
        foreach (var tilemap in tilemaps) {
            var bnd = tilemap.cellBounds;
            var mapTiles = tilemap.GetTilesBlock(bnd);
            for (var x = 0; x < bnd.size.x; x++) {
                for (var y = 0; y < bnd.size.y; y++) {
                    var tile = mapTiles[x + y * bnd.size.x];
                    if (tile != null) {
                        tilePathing.Add(new PathingTile(new Vector2Int(bnd.x + x, bnd.y + y), tilemap.gameObject.layer != 6));
                    }
                }
            }
        }
        TilePathing = tilePathing;
        Instance = this;
    }

    public Vector2Int[] FindPath(Vector2Int start, Vector2Int end) {
        return Pathfinding.FindShortestPath(TilePathing, start, end);
    }

    public Vector2Int FindCoordinates(Vector3 position) {
        var grid = Instance.GetComponent<Grid>();
        return (Vector2Int)grid.WorldToCell(position);
    }

    public Vector3 FindWorldPosition(Vector2Int coordinates) {
        var grid = Instance.GetComponent<Grid>();
        return grid.CellToWorld((Vector3Int)coordinates);
    }
}
