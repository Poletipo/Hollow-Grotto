using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager {

    public static void SaveChunk(GameObject gameObject) {

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

    public static Chunk_Data LoadChunk(string chunkName) {

        string fileName = "/" + chunkName + ".dat";
        string path = Application.persistentDataPath + fileName;

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

    public static void SavePlayer(GameObject gameObject) {
        string fileName = "/Player.dat";
        string path = Application.persistentDataPath + fileName;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);

        Player_Data data = new Player_Data(gameObject);

        bf.Serialize(fs, data);
        fs.Close();
    }

    public static Player_Data LoadPlayer() {

        string fileName = "/Player.dat";
        string path = Application.persistentDataPath + fileName;

        if (File.Exists(path)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            Player_Data data = (Player_Data)bf.Deserialize(fs);

            fs.Close();

            return data;
        }
        else {
            return null;
        }
    }


}
