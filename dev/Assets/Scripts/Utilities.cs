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
}
