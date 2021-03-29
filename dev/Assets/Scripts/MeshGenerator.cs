using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshGenerator : MonoBehaviour {
    public ComputeShader marchingShader;

    public Mesh GenerateMesh(Utilities.Point[] gridPoints, float Threshold, int nbVoxelPerAxis) {
        Mesh mesh = new Mesh();

        ComputeBuffer pointsBuffer = new ComputeBuffer(gridPoints.Length, sizeof(float) * 4);
        pointsBuffer.SetData(gridPoints);

        int numThreadPerAxis = Mathf.CeilToInt(nbVoxelPerAxis / ((float)8));

        int maxNbTriangle = (int)Mathf.Pow(nbVoxelPerAxis, 3) * 5;

        ComputeBuffer trianglesBuffer = new ComputeBuffer(maxNbTriangle, sizeof(float) * 3 * 3, ComputeBufferType.Append);
        trianglesBuffer.SetCounterValue(0);
        marchingShader.SetBuffer(0, "points", pointsBuffer);
        marchingShader.SetBuffer(0, "triangles", trianglesBuffer);
        marchingShader.SetInt("numPointsPerAxis", (nbVoxelPerAxis + 1));
        marchingShader.SetFloat("Threshold", Threshold);
        marchingShader.Dispatch(0, numThreadPerAxis, numThreadPerAxis, numThreadPerAxis);

        ComputeBuffer triangleCount = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

        ComputeBuffer.CopyCount(trianglesBuffer, triangleCount, 0);

        int[] triCountArray = { 0 };
        triangleCount.GetData(triCountArray);

        Utilities.TriangleGPU[] triangles = new Utilities.TriangleGPU[triCountArray[0]];
        trianglesBuffer.GetData(triangles, 0, 0, triCountArray[0]);

        int vertexCount = triangles.Length * 3;

        Dictionary<Vector3, int> triCornerIndex = new Dictionary<Vector3, int>();

        //List triangle corner = nombre de corner
        //List index = nombre de coin

        int indexes = 0;
        for (int i = 0; i < triangles.Length; i++) {
            if (!triCornerIndex.ContainsKey(triangles[i].corner3)) {
                triCornerIndex.Add(triangles[i].corner3, indexes);
                indexes++;
            }
            if (!triCornerIndex.ContainsKey(triangles[i].corner2)) {
                triCornerIndex.Add(triangles[i].corner2, indexes);
                indexes++;
            }
            if (!triCornerIndex.ContainsKey(triangles[i].corner1)) {
                triCornerIndex.Add(triangles[i].corner1, indexes);
                indexes++;
            }
        }

        Vector3[] trianglesCorners = new Vector3[triCornerIndex.Count];
        indexes = 0;
        foreach (var item in triCornerIndex) {
            trianglesCorners[indexes] = item.Key;
            indexes++;
        }

        int[] triangleCornerIndex = new int[vertexCount];
        for (int i = 0, index = 0; i < triangles.Length; i++, index += 3) {
            triangleCornerIndex[index] = triCornerIndex[triangles[i].corner3];
            triangleCornerIndex[index + 1] = triCornerIndex[triangles[i].corner2];
            triangleCornerIndex[index + 2] = triCornerIndex[triangles[i].corner1];
        }

        mesh.name = "MarchingCube";
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = trianglesCorners;
        mesh.triangles = triangleCornerIndex;
        mesh.RecalculateNormals();

        pointsBuffer.Release();
        trianglesBuffer.Release();
        triangleCount.Release();
        return mesh;

    }
}
