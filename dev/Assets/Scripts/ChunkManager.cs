using System;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ChunkManager : MonoBehaviour {
    [Range(1, 250)]
    public int GridResolution = 4;
    public int ChunkSize = 4;

    public MeshGenerator MeshGenerator;
    public NoiseGenerator NoiseGenerator;

    public float loadDistance = 50;

    Dictionary<String, GameObject> ChunkList;
    List<GameObject> unusedChunks;

    GameObject chunkHolder;
    GameObject player;
    public GameObject chunkObject;

    // Start is called before the first frame update
    void Start() {
        ChunkList = new Dictionary<string, GameObject>();
        unusedChunks = new List<GameObject>();

        player = GameManager.Instance.Player;
        chunkHolder = GameObject.Find("ChunkHolder");
        LoadChunks();
    }

    float timer = 0;
    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            LoadChunks();
            timer = 1f;
        }
    }

    int Counter = 0;
    void LoadChunks() {

        Vector3Int playerChunk = new Vector3Int();
        playerChunk.x = Mathf.FloorToInt(player.transform.position.x / ChunkSize);
        playerChunk.y = Mathf.FloorToInt(player.transform.position.y / ChunkSize);
        playerChunk.z = Mathf.FloorToInt(player.transform.position.z / ChunkSize);

        int nbChunksDistance = Mathf.Abs(Mathf.CeilToInt(loadDistance / ChunkSize));

        Dictionary<string, GameObject> oldchunks = new Dictionary<string, GameObject>(ChunkList);
        ChunkList.Clear();

        for (int x = -nbChunksDistance; x <= nbChunksDistance; x++) {
            for (int y = -nbChunksDistance; y <= nbChunksDistance; y++) {
                for (int z = -nbChunksDistance; z <= nbChunksDistance; z++) {
                    Vector3Int chunkPos = playerChunk + new Vector3Int(x, y, z);
                    String chunkPosKey = chunkPos.ToString();

                    //Debug.Log("Chunk POs " + chunkPosKey);

                    if (!oldchunks.ContainsKey(chunkPosKey)) {
                        GameObject chunk;
                        if (unusedChunks.Count == 0) {
                            Counter++;
                            Debug.Log(Counter);
                            chunk = Instantiate(chunkObject, Vector3.zero, Quaternion.identity);
                            chunk.GetComponent<Chunk>().Init(chunkPos);
                        }
                        else {
                            chunk = unusedChunks[0];
                            unusedChunks.RemoveAt(0);
                            chunk.GetComponent<Chunk>().Init(chunkPos);
                        }
                        ChunkList.Add(chunkPosKey, chunk);
                        chunk.transform.parent = chunkHolder.transform;

                    }
                    else {
                        //Debug.Log("Should be here if no moving");
                        ChunkList.Add(chunkPosKey, oldchunks[chunkPosKey]);
                        oldchunks.Remove(chunkPosKey);
                    }
                }
            }
        }

        if (oldchunks.Count != 0) {
            foreach (GameObject item in oldchunks.Values) {
                unusedChunks.Add(item);
            }
            oldchunks.Clear();
        }
    }

}
