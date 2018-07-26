using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;    

[Serializable]  //Lets class be saved as binary  

public class Block
{
    const float tileSize = 0.25f;

    public enum Direction { north, east, south, west, up, down };
    public struct Tile { public int x; public int y; }

    public bool changed = true;

    public int blockSort;

    //Base block constructor
    public Block()
    {
    }

    //Creates an array of vector2 and fills based on tile position
    public virtual Vector2[] FaceUVs (Direction direction)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);
        UVs[0] = new Vector2(tileSize * tilePos.x  + tileSize,  tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y);
        return UVs;
    }

    //Returns tile with texture coordinates, direction is used for override function
    public virtual Tile TexturePosition (Direction direction)
    {
        Tile tile = new Tile();

        switch (blockSort)
        {
            case 0: //Stone
                tile.x = 0;
                tile.y = 0;
                return tile;
            case 1: //Copper
                tile.x = 2;
                tile.y = 2;
                return tile;
            case 2: //Green stone
                tile.x = 2;
                tile.y = 3;
                return tile;
            case 3: //Blue Stone
                tile.x = 1;
                tile.y = 3;
                return tile;

        }
        return tile;
    }

    public virtual bool IsSolid (Direction direction)
    {
        switch (direction)
        {
            case Direction.north:
                return true;
            case Direction.east:
                return true;
            case Direction.south:
                return true;
            case Direction.west:
                return true;
            case Direction.up:
                return true;
            case Direction.down:
                return true;
        }
        return false;
    }

    //Checks if block is solid if false renders face data for that direction
    public virtual MeshData Blockdata (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;

        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up))
        {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south))
        {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north))
        {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west))
        {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east))
        {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }
        return meshData;
    }

    //Adds triangles to the array in the MeshData Class, takes last four added vertices and creates two triangles to create a quad
    protected virtual MeshData FaceDataUp (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));

        return meshData;
    }

    protected virtual MeshData FaceDataDown (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));

        return meshData;
    }

    protected virtual MeshData FaceDataNorth (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));

        return meshData;
    }

    protected virtual MeshData FaceDataEast (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));

        return meshData;
    }

    protected virtual MeshData FaceDataSouth (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));

        return meshData;
    }

    protected virtual MeshData FaceDataWest (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.west));

        return meshData;
    }
}