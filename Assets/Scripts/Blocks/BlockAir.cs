using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]  //Lets class be saved as binary  
public class BlockAir : Block {

    public BlockAir()
         : base()
    {
    }
    public override MeshData Blockdata
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }
    public override bool IsSolid(Block.Direction direction)
    {
        return false;
    }
}
