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

        Vector3[] trianglesCorners = new Vector3[vertexCount];
        for (int i = 0, index = 0; i < triangles.Length; i++, index += 3) {
            trianglesCorners[index] = triangles[i].corner3;
            trianglesCorners[index + 1] = triangles[i].corner2;
            trianglesCorners[index + 2] = triangles[i].corner1;
        }

        int[] triangleCornerIndex = new int[vertexCount];

        for (int i = 0; i < vertexCount; i++) {
            triangleCornerIndex[i] = i;
        }

        mesh.name = "MarchingCube";
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = trianglesCorners;
        mesh.triangles = triangleCornerIndex;

        pointsBuffer.Release();
        trianglesBuffer.Release();
        triangleCount.Release();
        return mesh;

    }
}
