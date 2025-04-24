using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    [SerializeField]
    private TileBase[] floorTiles;
    [SerializeField]
    private TileBase[] wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft,
        wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTiles);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase[] tiles)
    {
        foreach (var position in positions)
        {
            TileBase tile = GetRandomTile(tiles);
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    private TileBase GetRandomTile(TileBase[] tiles)
    {
        if (tiles == null || tiles.Length == 0) return null;
        return tiles[UnityEngine.Random.Range(0, tiles.Length)];
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallTop);
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallSideRight);
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallSideLeft);
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallBottom);
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallFull);
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallInnerCornerDownLeft);
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallInnerCornerDownRight);
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallDiagonalCornerDownLeft);
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallDiagonalCornerDownRight);
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallDiagonalCornerUpRight);
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallDiagonalCornerUpLeft);
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallFull);
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = GetRandomTile(wallBottom);
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }
}
