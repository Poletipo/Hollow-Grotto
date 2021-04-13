using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Destructible : MonoBehaviour {
    MeshFilter MeshFilter;
    MeshCollider MeshCollider;
    public bool isModified = false;

    public delegate void DestructibleEvent(Destructible destructible);

    public DestructibleEvent OnMeshUpdate;

    public Utilities.Point[] GridPoints;

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
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();
        nbPoint = (int)Mathf.Pow((nbVoxelPerAxis + 1), 3);
        GridPoints = new Utilities.Point[nbPoint];
    }

    public void UpdateMesh()
    {
        MeshFilter.mesh = null;
        MeshCollider.sharedMesh = null;
        Mesh mesh = GameManager.Instance.MeshGenerator.GenerateMesh(GridPoints, Threshold, nbVoxelPerAxis);

        mesh.RecalculateBounds();
        if (mesh.vertexCount <= 0) {
            float boundSize = GridPoints[(nbPoint - 1)].pos.x - GridPoints[0].pos.x;
            Bounds bound = new Bounds(Vector3.one * boundSize / 2 + gameObject.transform.position, Vector3.one * boundSize);
            mesh.bounds = bound;
        }
        //NormalSolver.RecalculateNormals(mesh, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        MeshFilter.mesh = mesh;
        MeshCollider.sharedMesh = mesh;
        OnMeshUpdate?.Invoke(this);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            if (MeshFilter.mesh.vertexCount <= 0) {

                float boundSize = GridPoints[(nbPoint - 1)].pos.x - GridPoints[0].pos.x;
                Gizmos.DrawCube(Vector3.one * boundSize / 2 + gameObject.transform.position, Vector3.one * boundSize);
            }
    }

}
