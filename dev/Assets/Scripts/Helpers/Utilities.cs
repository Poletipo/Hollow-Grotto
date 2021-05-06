using UnityEngine;

public static class Utilities {
    public struct Point {
        public Vector3 pos;
        public float val;
    }

    public struct GridCell {
        public Point[] point;
    }

    public struct TriangleGPU {
        public Vector3 corner1;
        public Vector3 corner2;
        public Vector3 corner3;
    }

    public struct Triangle {
        public Vector3[] corner;
    }

    public static Vector3Int GetChunkCoordonate(Vector3 pos)
    {
        Vector3Int chunkCoordonate = new Vector3Int();
        chunkCoordonate.x = Mathf.FloorToInt(pos.x / GameManager.Instance.ChunkManager.ChunkSize);
        chunkCoordonate.y = Mathf.FloorToInt(pos.y / GameManager.Instance.ChunkManager.ChunkSize);
        chunkCoordonate.z = Mathf.FloorToInt(pos.z / GameManager.Instance.ChunkManager.ChunkSize);

        return chunkCoordonate;
    }
}
