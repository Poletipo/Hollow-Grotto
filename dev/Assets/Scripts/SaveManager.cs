using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager {


    static string chunkDirectory = Application.persistentDataPath + "/Chunks";
    static string worldPath = Application.persistentDataPath + "/World.dat";
    static string playerPath = Application.persistentDataPath + "/Player.dat";

    public static bool SaveExist()
    {
        string fileName = "/World.dat";
        string path = Application.persistentDataPath + fileName;

        if (File.Exists(worldPath)) {
            return true;
        }
        return false;
    }

    public static void SaveWorld()
    {
        // Create the Binary Formatter.
        BinaryFormatter bf = new BinaryFormatter();
        // Stream the file with a File Stream. (Note that File.Create() 'Creates' or 'Overwrites' a file.)
        FileStream file = new FileStream(worldPath, FileMode.Create);

        World_Data data = new World_Data(GameManager.Instance.ChunkManager.NoiseGenerator);

        bf.Serialize(file, data);
        file.Close();
    }

    public static World_Data LoadWorld()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(worldPath, FileMode.Open);

        World_Data data = (World_Data)bf.Deserialize(fs);

        fs.Close();

        return data;
    }

    public static void DeleteWorld()
    {

        string[] chunks = Directory.GetFiles(chunkDirectory);

        for (int i = 0; i < chunks.Length; i++) {
            File.Delete(chunks[i]);
        }
        File.Delete(worldPath);
        File.Delete(playerPath);
    }


    public static void SaveChunk(GameObject gameObject)
    {

        string fileName = "/" + gameObject.name + ".dat";
        string path = Application.persistentDataPath + fileName;
        // Create the Binary Formatter.
        BinaryFormatter bf = new BinaryFormatter();
        // Stream the file with a File Stream. (Note that File.Create() 'Creates' or 'Overwrites' a file.)
        FileStream file = new FileStream(path, FileMode.Create);

        Chunk_Data data = new Chunk_Data(gameObject);

        bf.Serialize(file, data);
        file.Close();
    }

    public static void SaveChunk(Chunk_Data data)
    {

        if (!Directory.Exists(chunkDirectory)) {
            Directory.CreateDirectory(chunkDirectory);
        }

        Vector3Int coord = new Vector3Int();
        coord.x = data.Coordonates[0];
        coord.y = data.Coordonates[1];
        coord.z = data.Coordonates[2];


        string fileName = "/Chunk" + coord + ".dat";
        string path = chunkDirectory + fileName;
        // Create the Binary Formatter.
        BinaryFormatter bf = new BinaryFormatter();
        // Stream the file with a File Stream. (Note that File.Create() 'Creates' or 'Overwrites' a file.)
        FileStream file = new FileStream(path, FileMode.Create);

        bf.Serialize(file, data);
        file.Close();
    }

    public static Chunk_Data LoadChunk(string chunkName)
    {

        string fileName = "/" + chunkName + ".dat";
        string path = chunkDirectory + fileName;

        if (File.Exists(path)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Chunk_Data data = (Chunk_Data)bf.Deserialize(stream);

            stream.Close();

            return data;
        }
        else {
            return null;
        }
    }

    public static void SavePlayer(GameObject gameObject)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(playerPath, FileMode.Create);

        Player_Data data = new Player_Data(gameObject);

        bf.Serialize(fs, data);
        fs.Close();
    }

    public static Player_Data LoadPlayer()
    {
        if (File.Exists(playerPath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(playerPath, FileMode.Open);

            Player_Data data = (Player_Data)bf.Deserialize(fs);

            fs.Close();

            return data;
        }
        else {
            return null;
        }
    }


}
