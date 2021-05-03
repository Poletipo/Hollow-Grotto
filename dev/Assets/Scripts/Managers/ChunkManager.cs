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

    GameObject objectifChunk;
    GameObject chunkHolder;
    GameObject player;
    public GameObject chunkObject;
    public GameObject Objectif;

    // Start is called before the first frame update
    void Start()
    {
        ChunkList = new Dictionary<string, GameObject>();
        ModifiedChunkList = new Dictionary<string, Chunk_Data>();
        unusedChunks = new List<GameObject>();

        player = GameManager.Instance.Player;
        chunkHolder = GameObject.Find("ChunkHolder");
        if (!SaveManager.SaveExist()) {
            GeneratePlayerStart();
            GenerateObjectif();
        }
    }

    float timer = 0;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            LoadChunks();
            timer = 0f;
        }
    }

    Vector3Int lastPlayerChunk = new Vector3Int();
    void LoadChunks()
    {
        Vector3Int playerChunk = new Vector3Int();
        playerChunk.x = Mathf.FloorToInt(player.transform.position.x / ChunkSize);
        playerChunk.y = Mathf.FloorToInt(player.transform.position.y / ChunkSize);
        playerChunk.z = Mathf.FloorToInt(player.transform.position.z / ChunkSize);

        if (lastPlayerChunk != playerChunk) {
            lastPlayerChunk = playerChunk;
            int nbChunksDistance = Mathf.Abs(Mathf.CeilToInt(loadDistance / ChunkSize));

            foreach (GameObject item in ChunkList.Values) {
                if (item != objectifChunk) {
                    item.GetComponent<Chunk>().Unused = true;
                }
            }
            for (int x = -nbChunksDistance; x <= nbChunksDistance; x++) {
                for (int y = -nbChunksDistance; y <= nbChunksDistance; y++) {
                    for (int z = -nbChunksDistance; z <= nbChunksDistance; z++) {
                        Vector3Int chunkPos = playerChunk + new Vector3Int(x, y, z);
                        LoadChunk(chunkPos);
                    }
                }
            }
            MarkedUnusedChunks();
        }
    }


    public GameObject LoadChunk(Vector3Int chunkCoord)
    {
        Vector3Int chunkPos = chunkCoord;
        string chunkPosKey = chunkPos.ToString();
        GameObject chunk;
        //Si le chunk existe pas deja
        if (!ChunkList.ContainsKey(chunkPosKey)) {
            //Si il n'y a pas de chunk inutiliser
            if (unusedChunks.Count == 0) {
                chunk = Instantiate(chunkObject, Vector3.zero, Quaternion.identity);
                //Si le chunk a été modifier avant
                if (ModifiedChunkList.ContainsKey(chunkPosKey)) {
                    chunk.GetComponent<Chunk>().LoadChunk(ModifiedChunkList[chunkPosKey]);
                }
                //Si le chunk n'a pas été modifier avant
                else {
                    chunk.GetComponent<Chunk>().LoadChunk(chunkPos);
                }
            }
            //Si la liste de chunk inutiliser existe
            else {
                chunk = unusedChunks[0];
                chunk.GetComponent<Chunk>().ResetChunk();
                unusedChunks.RemoveAt(0);
                //Si le chunk a été modifier avant
                if (ModifiedChunkList.ContainsKey(chunkPosKey)) {
                    chunk.GetComponent<Chunk>().LoadChunk(ModifiedChunkList[chunkPosKey]);
                }
                //Si le chunk n'a pas été modifier avant
                else {
                    chunk.GetComponent<Chunk>().LoadChunk(chunkPos);
                }
            }
            ChunkList.Add(chunkPosKey, chunk);
            chunk.transform.parent = chunkHolder.transform;
        }
        //Si le chunk existe deja
        else {
            chunk = ChunkList[chunkPosKey];
        }
        chunk.GetComponent<Chunk>().Unused = false;
        return chunk;
    }


    private void MarkedUnusedChunks()
    {
        Dictionary<string, GameObject> oldchunks = new Dictionary<string, GameObject>(ChunkList);
        ChunkList.Clear();

        foreach (GameObject item in oldchunks.Values) {
            if (!item.GetComponent<Chunk>().Unused) {
                ChunkList.Add(item.GetComponent<Chunk>().Coordonnate.ToString(), item);
            }
            else {
                unusedChunks.Add(item);
            }
        }
    }


    public void GenerateObjectif()
    {
        bool validPos = false;
        GameObject chunk;
        while (!validPos) {
            Vector3Int objChunk = Vector3Int.CeilToInt((player.transform.position + Random.onUnitSphere * 50) / ChunkSize);
            chunk = LoadChunk(objChunk);
            chunk.GetComponent<Chunk>().Unused = true;
            Vector3[] listPos = MeshSpawner.GetSpawnPosition(chunk.GetComponent<MeshFilter>().mesh, chunk.transform, Vector3.up, 20, 1);

            if (listPos.Length > 0) {
                validPos = true;
                GameObject tempObj = Instantiate(Objectif, listPos[0] + chunk.transform.position, Quaternion.identity);
                chunk.GetComponent<Chunk>().objectives.Add(tempObj);
                objectifChunk = chunk;
                chunk.GetComponent<Chunk>().Unused = false;
                GameManager.Instance.ChunkManager.ModifiedChunkList[chunk.GetComponent<Chunk>().Coordonnate.ToString()] = new Chunk_Data(chunk);
            }
        }
    }

    public void GeneratePlayerStart()
    {
        bool validPos = false;
        GameObject chunk;
        while (!validPos) {
            Vector3Int objChunk = Vector3Int.CeilToInt((Random.insideUnitSphere * 500) / ChunkSize);
            chunk = LoadChunk(objChunk);
            Vector3[] listPos = MeshSpawner.GetSpawnPosition(chunk.GetComponent<MeshFilter>().mesh, chunk.transform, Vector3.up, 20, 50.0f);

            if (listPos.Length > 10) {
                validPos = true;
                player.transform.position = listPos[0] + chunk.transform.position + Vector3.up;
            }
        }
    }

    public void SaveModifiedChunks()
    {
        foreach (Chunk_Data data in ModifiedChunkList.Values) {
            if (data.objectives.Length > 0) {
                Debug.Log(data.objectives[0].isFixed);
            }
            SaveManager.SaveChunk(data);
        }
        ModifiedChunkList.Clear();
    }


}
