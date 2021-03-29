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

    public DigShape DiggingShape = DigShape.Sphere;

    private void Awake() {
        Init();
    }
    public void Init() {
        boxBound = new Bounds();
        boxBound.size = Vector3.one * DigSize;
    }


    public void Dig(Vector3 digPostion) {

        if (DiggingShape == DigShape.Box) {
            foreach (GameObject item in GameManager.Instance.ListDestructible) {
                boxBound.center = digPostion;
                boxBound.size = Vector3.one * DigSize;
                if (boxBound.Intersects(item.GetComponent<Collider>().bounds)) {

                    Utilities.Point[] points = item.GetComponent<Destructible>().GridPoints;

                    Vector3 hitPos = item.transform.InverseTransformPoint(digPostion);
                    boxBound.center = hitPos;
                    Vector3 size = new Vector3(1 / item.transform.localScale.x, 1 / item.transform.localScale.y,
                        1 / item.transform.localScale.z);
                    boxBound.size = Vector3.Scale(boxBound.size, size);

                    for (int i = 0; i < points.Length; i++) {
                        if (boxBound.Contains(points[i].pos)) {
                            points[i].val = item.GetComponent<Destructible>().Threshold;
                        }
                    }
                    item.GetComponent<Destructible>().GridPoints = points;
                    item.GetComponent<Destructible>().UpdateMesh();
                }
            }
        }
        else if (DiggingShape == DigShape.Sphere) {
            foreach (GameObject item in GameManager.Instance.ListDestructible) {
                boxBound.center = digPostion;
                boxBound.size = Vector3.one * DigSize;
                if (boxBound.Intersects(item.GetComponent<Collider>().bounds)) {

                    Utilities.Point[] points = item.GetComponent<Destructible>().GridPoints;
                    Vector3 hitPos = item.transform.InverseTransformPoint(digPostion);
                    boxBound.center = hitPos;
                    Vector3 size = new Vector3(1 / item.transform.localScale.x, 1 / item.transform.localScale.y,
                        1 / item.transform.localScale.z);
                    boxBound.size = Vector3.Scale(boxBound.size, size);
                    boxBound.center = hitPos;
                    for (int i = 0; i < points.Length; i++) {
                        if (Vector3.Distance((points[i].pos), hitPos) <= (DigSize * size.x / 2)) {
                            //points[i].val = item.GetComponent<Destructible>().Threshold;
                            points[i].val += (DigSize * size.x / 2) - Vector3.Distance((points[i].pos), hitPos);
                            //points[i].val -= points[i].val;
                        }
                    }
                    item.GetComponent<Destructible>().UpdateMesh();
                }
            }
        }
    }


}
