using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
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

}
