using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour {
    public Vector3Int Coordonnate;
    bool IsModified = false;

    public Utilities.Point[] gridPoints;

    ChunkManager ChunkManager;

    MeshFilter MeshFilter;
    MeshRenderer MeshRenderer;
    MeshCollider MeshCollider;


    private void Awake() {
        ChunkManager = FindObjectOfType<ChunkManager>();
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();

        float start = Time.realtimeSinceStartup;
        Init(Coordonnate);
        float end = Time.realtimeSinceStartup;
        //Debug.Log("Mesh GPU: " + (end - start));
    }


    public void Init(Vector3Int pos) {
        Coordonnate = pos;
        gameObject.name = "Chunk" + Coordonnate;

        transform.position = Coordonnate * ChunkManager.ChunkSize;
        CreateGridGPU();
        UpdateMeshGPU();
    }

    void CreateGrid() {
        int nbPoint = (int)Mathf.Pow((ChunkManager.GridResolution + 1), 3);
        gridPoints = new Utilities.Point[nbPoint];

        float gridSize = ((float)ChunkManager.ChunkSize) / (ChunkManager.GridResolution);

        int i = 0;
        for (int z = 0; z < (ChunkManager.GridResolution + 1); z++) {
            for (int y = 0; y < (ChunkManager.GridResolution + 1); y++) {
                for (int x = 0; x < (ChunkManager.GridResolution + 1); x++) {

                    Vector3 position = new Vector3(x * gridSize,
                        y * gridSize,
                        z * gridSize) + transform.position;

                    gridPoints[i] = new Utilities.Point {
                        pos = position,
                        val = ChunkManager.NoiseGenerator.GetValue(position)
                    };
                    i++;
                }
            }
        }
    }

    void CreateGridGPU() {
        int nbPoint = (int)Mathf.Pow((ChunkManager.GridResolution + 1), 3);
        gridPoints = new Utilities.Point[nbPoint];

        float voxelSize = ((float)ChunkManager.ChunkSize) / (ChunkManager.GridResolution);
        float startC = Time.realtimeSinceStartup;

        ComputeBuffer pointsBuffer = new ComputeBuffer(nbPoint, sizeof(float) * 4);

        ComputeShader gridNoiseShader = ChunkManager.NoiseGenerator.gridNoiseShader;

        Vector3[] randSeedOffsets = ChunkManager.NoiseGenerator.SeedValues();
        ComputeBuffer offsetsBuffer = new ComputeBuffer(randSeedOffsets.Length, sizeof(float) * 3);
        offsetsBuffer.SetData(randSeedOffsets);

        gridNoiseShader.SetBuffer(0, "randSeedOffsets", offsetsBuffer);

        gridNoiseShader.SetBuffer(0, "points", pointsBuffer);
        gridNoiseShader.SetFloat("voxelSize", voxelSize);
        gridNoiseShader.SetFloat("chunkSize", ChunkManager.ChunkSize);
        gridNoiseShader.SetFloat("noiseScale", ChunkManager.NoiseGenerator.Scale);
        gridNoiseShader.SetFloat("octaves", ChunkManager.NoiseGenerator.Octaves);
        gridNoiseShader.SetFloat("persistence", ChunkManager.NoiseGenerator.Persistence);
        gridNoiseShader.SetInt("numPointsPerAxis", ChunkManager.GridResolution + 1);
        gridNoiseShader.SetVector("noiseOffset", ChunkManager.NoiseGenerator.Offset);
        gridNoiseShader.SetVector("axesSize", ChunkManager.NoiseGenerator.axesScale);
        gridNoiseShader.SetVector("chunkPosition", transform.position);

        int numThreadPerAxis = Mathf.CeilToInt(ChunkManager.GridResolution + 1 / ((float)8));

        gridNoiseShader.Dispatch(0, numThreadPerAxis, numThreadPerAxis, numThreadPerAxis);

        pointsBuffer.GetData(gridPoints);

        pointsBuffer.Release();
        offsetsBuffer.Release();
        float endC = Time.realtimeSinceStartup;
        //Debug.Log("GRID CREATE GPU: " + (endC - startC));
    }



    public void UpdateMeshGPU() {
        MeshFilter.mesh = null;
        MeshCollider.sharedMesh = null;

        Mesh mesh = ChunkManager.MeshGenerator.GenerateMeshGPU(gridPoints);
        mesh.RecalculateBounds();
        MeshFilter.mesh = mesh;
        MeshCollider.sharedMesh = mesh;
    }

    //private void OnDrawGizmos() {
    //    if (gridPoints == null) {
    //        return;
    //    }
    //    Gizmos.color = Color.white;
    //    for (int i = 0; i < gridPoints.Length; i++) {
    //        Gizmos.color = gridPoints[i].val < ChunkManager.MeshGenerator.Threshold ? Color.black : Color.white;
    //        //Gizmos.color = Color.Lerp(Color.white, Color.black, fullGrid.val[i]);
    //        //Handles.Label(gridPoints[i].pos, i.ToString());
    //        Gizmos.DrawSphere(gridPoints[i].pos, 0.1f);
    //    }
    //}

}
