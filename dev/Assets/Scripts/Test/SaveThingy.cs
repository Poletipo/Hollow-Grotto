using UnityEngine;

public class SaveThingy : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            SaveManager.SaveChunk(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            Chunk_Data data = SaveManager.LoadChunk("Cube_Data");
        }
    }
}
