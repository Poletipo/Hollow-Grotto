using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
[RequireComponent(typeof(Destructible))]
public class Chunk : MonoBehaviour {
    public Vector3Int Coordonnate;

    bool particuleSpawned = false;

    MeshFilter MeshFilter;
    ChunkManager ChunkManager;
    Destructible destructible;
    ParticleSystem particle;
    ParticleSystem.ShapeModule sh;

    public bool SpawnParticles = true;

    private void Awake()
    {
        ChunkManager = GameManager.Instance.ChunkManager;
        destructible = GetComponent<Destructible>();
        particle = GetComponent<ParticleSystem>();
        MeshFilter = GetComponent<MeshFilter>();


        destructible.OnMeshUpdate += OnMeshUpdate;

        destructible.Setup(ChunkManager.Threshold, ChunkManager.GridResolution);
        sh = particle.shape;
        sh.enabled = true;
        sh.shapeType = ParticleSystemShapeType.Mesh;
        Init(Coordonnate);
    }

    private void Update()
    {

        if (!particuleSpawned) {
            UpdateParticle();
        }

    }

    private void OnMeshUpdate(Destructible destructible)
    {
        UpdateParticle();
    }

    public void Init(Vector3Int pos)
    {
        Coordonnate = pos;
        gameObject.name = "Chunk" + Coordonnate;

        transform.position = Coordonnate * ChunkManager.ChunkSize;
        CreateChunkGrid();

        ResetChunk();
        UpdateParticle();
    }
    public void Init(Vector3Int pos, float[] gridPoints)
    {
        Coordonnate = pos;
        gameObject.name = "Chunk" + Coordonnate;

        transform.position = Coordonnate * ChunkManager.ChunkSize;
        CreateChunkGrid();
        for (int i = 0; i < destructible.nbPoint; i++) {
            destructible.GridPoints[i].val = gridPoints[i];
        }
        destructible.UpdateMesh();

        ResetChunk();
        UpdateParticle();
    }

    public void ResetChunk()
    {
        if (SpawnParticles && MeshFilter.mesh.vertexCount > 4) {
            sh.mesh = MeshFilter.mesh;
            particle.Pause();
            particuleSpawned = false;
        }
        particle.Simulate(0, false, true);
    }

    void CreateChunkGrid()
    {
        int nbPoint = destructible.nbPoint;
        float voxelSize = ((float)ChunkManager.ChunkSize) / (ChunkManager.GridResolution);

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
        gridNoiseShader.SetFloat("noiseMulValue", ChunkManager.NoiseGenerator.noiseMul);
        gridNoiseShader.SetInt("numPointsPerAxis", ChunkManager.GridResolution + 1);
        gridNoiseShader.SetVector("noiseOffset", ChunkManager.NoiseGenerator.Offset);
        gridNoiseShader.SetVector("axesSize", ChunkManager.NoiseGenerator.axesScale);
        gridNoiseShader.SetVector("chunkPosition", transform.position);

        int numThreadPerAxis = Mathf.CeilToInt(ChunkManager.GridResolution + 1 / ((float)8));

        gridNoiseShader.Dispatch(0, numThreadPerAxis, numThreadPerAxis, numThreadPerAxis);

        pointsBuffer.GetData(destructible.GridPoints);
        destructible.UpdateMesh();
        destructible.UpdateBound();
        pointsBuffer.Release();
        offsetsBuffer.Release();
    }


    void UpdateParticle()
    {
        if (SpawnParticles) {

            ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[particle.main.maxParticles];
            int partAlive = particle.GetParticles(m_Particles);
            RaycastHit hit;

            for (int i = 0; i < partAlive; i++) {
                Vector3 partMin = transform.TransformPoint(m_Particles[i].position);
                partMin.y = transform.position.y;

                ParticleSystem.Particle particle = m_Particles[i];
                if (Physics.Linecast(transform.TransformPoint(m_Particles[i].position), partMin, out hit)) {
                    particle.position = transform.InverseTransformPoint(hit.point);
                    m_Particles[i] = particle;
                    particuleSpawned = true;
                }
                else {
                    particle.remainingLifetime = 0;
                    m_Particles[i] = particle;
                }
            }
            particle.SetParticles(m_Particles);
            particle.Play();
        }
    }


    public void SaveChunk()
    {
        if (destructible.isModified) {
            SaveManager.SaveChunk(gameObject);
            destructible.isModified = false;
        }
    }

    public void LoadChunk(Vector3Int coordonate)
    {
        string Chunkname = "Chunk" + coordonate;
        Chunk_Data data = SaveManager.LoadChunk(Chunkname);
        if (data != null) {
            Vector3Int coord = new Vector3Int();
            coord.x = data.Coordonates[0];
            coord.y = data.Coordonates[1];
            coord.z = data.Coordonates[2];

            Init(coord, data.gridValues);
        }
        else {
            Init(coordonate);
        }
    }

    public void LoadChunk(Chunk_Data data)
    {
        Vector3Int coord = new Vector3Int();
        coord.x = data.Coordonates[0];
        coord.y = data.Coordonates[1];
        coord.z = data.Coordonates[2];

        Init(coord, data.gridValues);
    }

}
