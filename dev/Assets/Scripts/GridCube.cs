using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCube : MonoBehaviour
{
    public int xSize, ySize, zSize = 3;
    public float gridSize = 1;
    Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }
    
    void Generate() {
        vertices = new Vector3[(xSize + 1) * (ySize + 1) * (zSize + 1)];
        int i = 0;
        for (int z = 0; z < zSize; z++) {
            for (int y = 0; y < ySize; y++) {
                for (int x = 0; x < xSize; x++) {
                    vertices[i] = new Vector3(x, y, z);
                    i++;
                }
            }
        }
    }

    private void OnDrawGizmos() {
        if (vertices == null) {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    struct verticesValue {
        Vector3 vertice;
        float value;
    }
}
