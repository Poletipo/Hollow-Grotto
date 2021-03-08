using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    Vector3Int Coordonnate;
    bool IsModified = false;

    Utilities.Point[] gridPoints;

    public ChunkManager ChunkManager;

    MeshFilter MeshFilter;
    MeshRenderer MeshRenderer;
    MeshCollider MeshCollider;


    private void Start() {

        float start = Time.realtimeSinceStartup;
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();

        

        float end = Time.realtimeSinceStartup;
        Debug.Log("All: "+ (end - start));
    }


    private void Update() {
        CreateGridGPU();
        UpdateMesh();
    }

    void CreateGrid() {
        int nbPoint = (int)Mathf.Pow((ChunkManager.GridResolution+1), 3);
        gridPoints = new Utilities.Point[nbPoint];

        float gridSize = ((float)ChunkManager.ChunkSize) / (ChunkManager.GridResolution);

        int i = 0;
        for (int z = 0; z < (ChunkManager.GridResolution + 1); z++) {
            for (int y = 0; y < (ChunkManager.GridResolution + 1); y++) {
                for (int x = 0; x < (ChunkManager.GridResolution + 1); x++) {

                    Vector3 position = new Vector3(x * gridSize,
                        y * gridSize,
                        z * gridSize) + transform.position;

                    gridPoints[i] = new Utilities.Point { pos = position,
                        val = ChunkManager.NoiseGenerator.GetValue(position)
                    };
                    i++;
                }
            }
        }
        Debug.Log(gridPoints[0].pos);
    }

    void CreateGridGPU() {
        int nbPoint = (int)Mathf.Pow((ChunkManager.GridResolution + 1), 3);
        gridPoints = new Utilities.Point[nbPoint];

        float gridSize = ((float)ChunkManager.ChunkSize) / (ChunkManager.GridResolution);

        int i = 0;
        for (int z = 0; z < (ChunkManager.GridResolution + 1); z++) {
            for (int y = 0; y < (ChunkManager.GridResolution + 1); y++) {
                for (int x = 0; x < (ChunkManager.GridResolution + 1); x++) {

                    Vector3 position = new Vector3(x , y, z);

                    gridPoints[i] = new Utilities.Point {
                        pos = position,
                        val = ChunkManager.NoiseGenerator.GetValue(position)
                    };
                    i++;
                }
            }
        }
        Debug.Log(gridPoints[0].pos);
    }



    void UpdateMesh() {
        Mesh mesh = ChunkManager.MeshGenerator.GenerateMeshGPU(gridPoints);
        mesh.RecalculateBounds();
        MeshFilter.mesh = mesh;
        MeshCollider.sharedMesh = mesh;
    }

    private void OnDrawGizmos() {
        if (gridPoints == null) {
            return;
        }
        Gizmos.color = Color.white;
        for (int i = 0; i < gridPoints.Length; i++) {
            Gizmos.color = gridPoints[i].val < ChunkManager.MeshGenerator.Threshold ? Color.black : Color.white;
            //Gizmos.color = Color.Lerp(Color.white, Color.black, fullGrid.val[i]);
            //Handles.Label(gridPoints[i].pos, i.ToString());
            Gizmos.DrawSphere(gridPoints[i].pos, 0.1f);
        }
    }

}
