using UnityEngine;

public class Snake : MonoBehaviour {

    public float speed = 0.5f;
    public int gridSize = 1;
    float yPos;
    float gridPos;
    // Start is called before the first frame update
    void Start() {
        yPos = transform.position.y;
        gridPos = yPos;
    }

    // Update is called once per frame
    void Update() {

        yPos += speed * Time.deltaTime;
        if (yPos >= gridPos * gridSize) {
            gridPos += gridSize;
        }


        transform.position = new Vector3(0, gridPos, 0);



    }
}
