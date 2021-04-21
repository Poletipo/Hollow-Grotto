using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Destructible : MonoBehaviour {
    MeshFilter meshFilter;
    MeshCollider MeshCollider;
    public bool isModified = false;
    public Mesh mesh;

    public delegate void DestructibleEvent(Destructible destructible);

    public DestructibleEvent OnMeshUpdate;

    public Utilities.Point[] GridPoints;

    public Bounds Bound;

    public float Threshold = 0;
    public int nbVoxelPerAxis = 5;
    public int nbPoint { get; private set; }

    private void Awake()
    {
        Setup(Threshold, nbVoxelPerAxis);
    }

    public void Setup(float Threshold, int nbVoxelPerAxis)
    {
        this.Threshold = Threshold;
        this.nbVoxelPerAxis = nbVoxelPerAxis;
        Initialize();
    }

    void Initialize()
    {
        meshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();
        nbPoint = (int)Mathf.Pow((nbVoxelPerAxis + 1), 3);
        GridPoints = new Utilities.Point[nbPoint];
    }

    public void UpdateMesh()
    {
        meshFilter.mesh = null;
        MeshCollider.sharedMesh = null;
        mesh = GameManager.Instance.MeshGenerator.GenerateMesh(GridPoints, Threshold, nbVoxelPerAxis);
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        meshFilter.mesh = mesh;
        MeshCollider.sharedMesh = mesh;
        OnMeshUpdate?.Invoke(this);
    }

    public void UpdateBound()
    {
        float boundSize = GridPoints[(nbPoint - 1)].pos.x - GridPoints[0].pos.x;
        Bound = new Bounds(Vector3.one * boundSize / 2 + transform.position, Vector3.one * boundSize);

    }

}
