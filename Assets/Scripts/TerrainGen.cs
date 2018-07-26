using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGen : MonoBehaviour
{
    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 8;
    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.008f;
    float stoneMinHeight = -12;
    float dirtBaseHeight = 1;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 3;

    //Takes chunk and fills it
    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.chunkSize; x++)
        {
            for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.chunkSize; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight); //base height
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
        {
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);
        }

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));
        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);

        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
            Block newBlock = new Block();
            if (y <= stoneHeight - 5 && Random.Range(1, 10) >= 7) //Sets bluestone
            {
                newBlock.blockSort = 3;
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, newBlock);

            }
            else if (y <= stoneHeight - 4 && Random.Range(1, 10) >= 5) //Sets greenstone
            {
                newBlock.blockSort = 2;
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, newBlock);

            }
            else if (y <= stoneHeight - 1 && Random.Range(1, 10) >= 4) //Sets copper
            {
                newBlock.blockSort = 1;
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, newBlock);

            }
            else if (y <= stoneHeight) //Sets stone blocks
            {
                newBlock.blockSort = 0;
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, newBlock);
            }
            else if (y <= dirtHeight) //Sets dirt and grass
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockGrass());
            }
            else //Sets blocks to air
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockAir());
            }
        }
        return chunk;
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}