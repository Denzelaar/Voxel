using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization {
    public static string saveFolderName = "WorldSaves";

    //Save location for the save files
    public static string SaveLocation(string worldName)
    { 
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    //Filename based upon world position
    public static string FileName(WorldPos chunkLocation)
    {
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }

    //Checks if chunk is modified and if so saves it to an binary file
    public static void SaveChunk(Chunk chunk)
    {
        Save save = new Save(chunk);    

        if (save.blocks.Count == 0)
        {
            return;
        }

        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save);
        stream.Close();
    }

    //Loads chunks by using the file location and chunks world position
    public static bool Load(Chunk chunk)
    {
        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        //Deserialize the changed block and set their position in the chunk to their value
        Save save = (Save)formatter.Deserialize(stream);
        foreach (var block in save.blocks)
        {
            chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
        }

        stream.Close();
        return true;
    }
}
