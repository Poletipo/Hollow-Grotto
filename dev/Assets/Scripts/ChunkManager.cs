using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ChunkManager : MonoBehaviour {
    [Range(1, 250)]
    public int GridResolution = 4;
    public int ChunkSize = 4;
    public float Threshold = 0;

    public MeshGenerator MeshGenerator;
    public NoiseGenerator NoiseGenerator;

    public float loadDistance = 50;

    public Dictionary<string, GameObject> ChunkList;
    public Dictionary<string, Chunk_Data> ModifiedChunkList;
    List<GameObject> unusedChunks;

    GameObject chunkHolder;
    GameObject player;
    public GameObject chunkObject;

    // Start is called before the first frame update
    void Start() {
        ChunkList = new Dictionary<string, GameObject>();
        ModifiedChunkList = new Dictionary<string, Chunk_Data>();
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
            timer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            SaveModifiedChunks();
        }

    }

    Vector3Int lastPlayerChunk = new Vector3Int();
    void LoadChunks() {
        Vector3Int playerChunk = new Vector3Int();
        playerChunk.x = Mathf.FloorToInt(player.transform.position.x / ChunkSize);
        playerChunk.y = Mathf.FloorToInt(player.transform.position.y / ChunkSize);
        playerChunk.z = Mathf.FloorToInt(player.transform.position.z / ChunkSize);

        if (lastPlayerChunk != playerChunk) {
            lastPlayerChunk = playerChunk;
            int nbChunksDistance = Mathf.Abs(Mathf.CeilToInt(loadDistance / ChunkSize));

            Dictionary<string, GameObject> oldchunks = new Dictionary<string, GameObject>(ChunkList);
            ChunkList.Clear();

            for (int x = -nbChunksDistance; x <= nbChunksDistance; x++) {
                for (int y = -nbChunksDistance; y <= nbChunksDistance; y++) {
                    for (int z = -nbChunksDistance; z <= nbChunksDistance; z++) {
                        Vector3Int chunkPos = playerChunk + new Vector3Int(x, y, z);
                        string chunkPosKey = chunkPos.ToString();

                        if (!oldchunks.ContainsKey(chunkPosKey)) {
                            GameObject chunk;
                            if (unusedChunks.Count == 0) {
                                chunk = Instantiate(chunkObject, Vector3.zero, Quaternion.identity);
                                if (ModifiedChunkList.ContainsKey(chunkPosKey)) {
                                    chunk.GetComponent<Chunk>().LoadChunk(ModifiedChunkList[chunkPosKey]);
                                }
                                else {
                                    chunk.GetComponent<Chunk>().LoadChunk(chunkPos);
                                }
                            }
                            else {
                                chunk = unusedChunks[0];
                                unusedChunks.RemoveAt(0);
                                if (ModifiedChunkList.ContainsKey(chunkPosKey)) {
                                    chunk.GetComponent<Chunk>().LoadChunk(ModifiedChunkList[chunkPosKey]);
                                }
                                else {
                                    chunk.GetComponent<Chunk>().LoadChunk(chunkPos);
                                }
                            }
                            ChunkList.Add(chunkPosKey, chunk);
                            chunk.transform.parent = chunkHolder.transform;

                        }
                        else {
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


    void SaveModifiedChunks() {
        foreach (Chunk_Data data in ModifiedChunkList.Values) {
            SaveManager.SaveChunk(data);
        }
        ModifiedChunkList.Clear();
    }


}
