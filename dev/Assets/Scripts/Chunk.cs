using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    public Vector3Int Coordonnate;
    bool IsModified = false;

    Utilities.Point[] gridPoints;

    public ChunkManager ChunkManager;

    MeshFilter MeshFilter;
    MeshRenderer MeshRenderer;
    MeshCollider MeshCollider;


    private void Start() {
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();

        

        //float startC = Time.realtimeSinceStartup;
        //CreateGrid();
        //UpdateMesh();
        //float endC = Time.realtimeSinceStartup;
        //Debug.Log("Mesh CPU: " + (endC - startC));
    }


    private void Update() {
        transform.position = Coordonnate * ChunkManager.ChunkSize;
        CreateGridGPU();

        float start = Time.realtimeSinceStartup;
        UpdateMeshGPU();
        float end = Time.realtimeSinceStartup;
        Debug.Log("Mesh GPU: " + (end - start));

        
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
    }

    void CreateGridGPU() {
        int nbPoint = (int)Mathf.Pow((ChunkManager.GridResolution + 1), 3);
        gridPoints = new Utilities.Point[nbPoint];

        float voxelSize = ((float)ChunkManager.ChunkSize) / (ChunkManager.GridResolution);
        float startC = Time.realtimeSinceStartup;



        ComputeBuffer pointsBuffer = new ComputeBuffer(nbPoint, sizeof(float) * 4);
        
        ComputeShader gridNoiseShader = ChunkManager.NoiseGenerator.gridNoiseShader;

        gridNoiseShader.SetBuffer(0,"points", pointsBuffer);
        gridNoiseShader.SetFloat("voxelSize", voxelSize);
        gridNoiseShader.SetInt("numPointsPerAxis", ChunkManager.GridResolution + 1);
        gridNoiseShader.SetVector("noiseOffset", ChunkManager.NoiseGenerator.Offset);
        gridNoiseShader.SetVector("chunkPosition", transform.position);

        int numThreadPerAxis = Mathf.CeilToInt(ChunkManager.GridResolution / ((float)8));

        gridNoiseShader.Dispatch(0, numThreadPerAxis, numThreadPerAxis, numThreadPerAxis);

        pointsBuffer.GetData(gridPoints);

        pointsBuffer.Release();
        float endC = Time.realtimeSinceStartup;
        Debug.Log("GRID CREATE GPU: " + (endC - startC));
    }



    void UpdateMeshGPU() {
        MeshFilter.mesh = null;
        MeshCollider.sharedMesh = null;

        Mesh mesh = ChunkManager.MeshGenerator.GenerateMeshGPU(gridPoints);
        //mesh.RecalculateBounds();
        MeshFilter.mesh = mesh;
        //MeshCollider.sharedMesh = mesh;
    }

    void UpdateMesh() {
        MeshFilter.mesh = null;
        MeshCollider.sharedMesh = null;
        Mesh mesh = ChunkManager.MeshGenerator.GenerateMesh(gridPoints);
        //mesh.RecalculateBounds();
        MeshFilter.mesh = mesh;
        //MeshCollider.sharedMesh = mesh;
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
