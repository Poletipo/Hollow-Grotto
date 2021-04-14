using UnityEngine;

public class TestSpawnMesh : MonoBehaviour {
    MeshFilter meshFilter;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        Vector3[] positions = MeshSpawner.GetSpawnPosition(meshFilter.mesh, transform, Vector3.up, 45, 100);

        for (int i = 0; i < positions.Length; i++) {
            Vector3 pos = transform.TransformPoint(positions[i]);
            GameObject objt = Instantiate(obj, pos, Random.rotation);
            objt.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
        }
    }

}
