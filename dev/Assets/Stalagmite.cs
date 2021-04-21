using UnityEngine;

public class Stalagmite : MonoBehaviour {
    Destructible destructible;

    public float rockSize = 5;
    public int nbVoxelPerAxis = 10;

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

                    Vector3 middlePos = Vector3.one * (rockSize / 2);
                    middlePos.y = position.y;

                    float roundValue = Vector3.Distance(position, middlePos) - (rockSize / 4);
                    Vector3 bottomPos = position;
                    bottomPos.y = 0;

                    float heightValue = Vector3.Distance(position, bottomPos) / 4;

                    float value = roundValue + heightValue + Random.Range(0.1f, 0.3f);

                    if (value <= 0 && y == 0) {
                        value = 1f;
                    }

                    gridPoints[i].val = value;

                    i++;
                }
            }
        }
        destructible.GridPoints = gridPoints;
        destructible.UpdateMesh();
        destructible.UpdateBound();
    }

}
