using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
[RequireComponent(typeof(Destructible))]
public class Rock : MonoBehaviour {

    Destructible destructible;

    public float rockSize = 5;
    public int nbVoxelPerAxis = 10;
    public GameObject obj;
    private void Start()
    {
        destructible = GetComponent<Destructible>();
        destructible.Setup(0, nbVoxelPerAxis);
        CreateGrid();
    }

    void CreateGrid()
    {
        Utilities.Point[] gridPoints = new Utilities.Point[destructible.nbPoint];
        float gridSize = rockSize / nbVoxelPerAxis;

        int i = 0;
        for (int z = 0; z < (nbVoxelPerAxis + 1); z++) {
            for (int y = 0; y < (nbVoxelPerAxis + 1); y++) {
                for (int x = 0; x < (nbVoxelPerAxis + 1); x++) {

                    Vector3 position = new Vector3(x * gridSize,
                        y * gridSize,
                        z * gridSize);

                    gridPoints[i].pos = position;
                    gridPoints[i].val = -1;
                    if (x % (nbVoxelPerAxis) == 0 || y % (nbVoxelPerAxis) == 0 ||
                        z % (nbVoxelPerAxis) == 0) {
                        gridPoints[i].val = 1;
                    }
                    i++;
                }
            }
        }
        destructible.GridPoints = gridPoints;
        destructible.UpdateMesh();
        destructible.UpdateBound();
    }

}
