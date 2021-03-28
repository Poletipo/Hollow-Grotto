using UnityEngine;

public class Destructible : MonoBehaviour {
    MeshFilter MeshFilter;
    MeshCollider MeshCollider;

    public delegate void DestructibleEvent(Destructible destructible);

    public DestructibleEvent OnNotColliding;

    private Utilities.Point[] _gridPoints;

    public Utilities.Point[] GridPoints {
        get { return _gridPoints; }
        set {
            _gridPoints = value;
        }
    }

    public float Threshold = 0;
    public int nbVoxelPerAxis = 5;
    public int nbPoint { get; private set; }
    bool colliding = true;

    private void Awake() {
        Setup(Threshold, nbVoxelPerAxis);
    }

    public void Setup(float Threshold, int nbVoxelPerAxis) {
        this.Threshold = Threshold;
        this.nbVoxelPerAxis = nbVoxelPerAxis;
        Initialize();
    }

    void Initialize() {
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();
        nbPoint = (int)Mathf.Pow((nbVoxelPerAxis + 1), 3);
        GridPoints = new Utilities.Point[nbPoint];
    }

    public void UpdateMesh() {
        MeshFilter.mesh = null;
        MeshCollider.sharedMesh = null;
        Mesh mesh = GameManager.Instance.MeshGenerator.GenerateMesh(GridPoints, Threshold, nbVoxelPerAxis);
        mesh.RecalculateBounds();
        MeshFilter.mesh = mesh;
        MeshCollider.sharedMesh = mesh;
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.contactCount == 0 && colliding) {
            OnNotColliding?.Invoke(this);
            colliding = false;
        }
    }



}
