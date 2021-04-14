using System.Collections.Generic;
using UnityEngine;

public static class MeshSpawner {

    public struct Triangle {
        public Vector3[] vertices;
        public Vector3[] verticesNormal;
        public Vector3 faceNormal;
    }

    public static Vector3[] GetSpawnPosition(Mesh mesh, Transform goTransform, Vector3 normalDirection, float angle, int number)
    {
        List<Triangle> validTriangles = GetValidTriangle(mesh, goTransform, normalDirection, angle);
        Vector3[] positions = new Vector3[0];
        if (validTriangles.Count > 0) {
            positions = GetPostions(validTriangles, number);
        }

        return positions;
    }

    public static Vector3[] GetSpawnPosition(Mesh mesh, Transform goTransform, Vector3 normalDirection, float angle, float percent)
    {
        List<Triangle> validTriangles = GetValidTriangle(mesh, goTransform, normalDirection, angle);
        Vector3[] positions = new Vector3[0];
        if (validTriangles.Count > 0) {
            int number = Mathf.CeilToInt(validTriangles.Count / 100.0f * percent);
            positions = GetPostions(validTriangles, number);
        }

        return positions;
    }


    private static List<Triangle> GetValidTriangle(Mesh mesh, Transform goTransform, Vector3 normalDirection, float angle)
    {
        List<Triangle> validTriangles = new List<Triangle>();
        float maxDotAngle = Mathf.Cos(angle * Mathf.Deg2Rad);

        for (int i = 0; i < (mesh.triangles.Length - 2); i += 3) {

            int i1 = mesh.triangles[i];
            int i2 = mesh.triangles[i + 1];
            int i3 = mesh.triangles[i + 2];

            Vector3 faceNormal = mesh.normals[i1] + mesh.normals[i2] + mesh.normals[i3];
            faceNormal.Normalize();
            faceNormal = goTransform.TransformDirection(faceNormal);
            float dotAngle = Vector3.Dot(normalDirection, faceNormal);

            if (dotAngle >= maxDotAngle) {

                Triangle triangle = new Triangle();
                triangle.vertices = new Vector3[] { mesh.vertices[i1], mesh.vertices[i2], mesh.vertices[i3] };
                triangle.verticesNormal = new Vector3[] { mesh.normals[i1], mesh.normals[i2], mesh.normals[i3] };
                triangle.faceNormal = faceNormal;

                validTriangles.Add(triangle);
            }
        }

        return validTriangles;
    }


    private static Vector3[] GetPostions(List<Triangle> validTriangles, int number)
    {
        Vector3[] positions = new Vector3[number];
        int index = 0;
        for (int i = 0; i < number; i++) {
            index = Random.Range(0, validTriangles.Count);

            float a = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.0f, 1.0f);

            Triangle tri = validTriangles[index];

            Vector3 pos = (1 - Mathf.Sqrt(a)) * tri.vertices[0] + (Mathf.Sqrt(a) * (1 - b)) * tri.vertices[1] + (b * Mathf.Sqrt(a)) * tri.vertices[2];
            positions[i] = pos;
        }

        return positions;
    }


}
