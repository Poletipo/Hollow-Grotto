using UnityEngine;

public class Digger : MonoBehaviour {

    public enum DigShape {
        Box,
        Sphere
    };

    Bounds boxBound;


    private float _digSize = 5;

    public float DigSize {
        get { return _digSize; }
        set {
            _digSize = value;
            boxBound.size = Vector3.one * DigSize;
        }
    }

    private DigShape _diggingShape = DigShape.Sphere;

    public DigShape DiggingShape {
        get { return _diggingShape; }
        set { _diggingShape = value; }
    }

    private void Awake() {
        Init();
    }
    public void Init() {
        boxBound = new Bounds();
        boxBound.size = Vector3.one * DigSize;
    }


    public void Dig(Vector3 digPostion) {

        boxBound.center = digPostion;
        if (DiggingShape == DigShape.Box) {
            foreach (GameObject item in GameManager.Instance.ChunkManager.GetComponent<ChunkManager>().ChunkList.Values) {

                if (boxBound.Intersects(item.GetComponent<Collider>().bounds)) {

                    Utilities.Point[] points = item.GetComponent<Chunk>().gridPoints;

                    for (int i = 0; i < points.Length; i++) {
                        if (boxBound.Contains(points[i].pos)) {
                            points[i].val += 1000;
                        }
                    }
                    item.GetComponent<Chunk>().UpdateMeshGPU();
                }
            }
        }
        else if (DiggingShape == DigShape.Sphere) {
            foreach (GameObject item in GameManager.Instance.ChunkManager.GetComponent<ChunkManager>().ChunkList.Values) {

                if (boxBound.Intersects(item.GetComponent<Collider>().bounds)) {

                    Utilities.Point[] points = item.GetComponent<Chunk>().gridPoints;

                    for (int i = 0; i < points.Length; i++) {
                        if (Vector3.Distance((points[i].pos), digPostion) <= (DigSize / 2)) {
                            points[i].val += (DigSize / 2) - Vector3.Distance((points[i].pos), digPostion);
                            //points[i].val -= points[i].val;
                        }
                    }
                    item.GetComponent<Chunk>().UpdateMeshGPU();
                }
            }
        }
    }


}
