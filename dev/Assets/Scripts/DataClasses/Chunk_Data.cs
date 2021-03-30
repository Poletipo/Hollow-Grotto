using UnityEngine;

[System.Serializable]
public class Chunk_Data {

    public int[] Coordonates;
    public float[] gridValues;

    public Chunk_Data(GameObject gameObject) {
        Coordonates = new int[3];
        Coordonates[0] = gameObject.GetComponent<Chunk>().Coordonnate.x;
        Coordonates[1] = gameObject.GetComponent<Chunk>().Coordonnate.y;
        Coordonates[2] = gameObject.GetComponent<Chunk>().Coordonnate.z;

        int gridCount = gameObject.GetComponent<Destructible>().GridPoints.Length;
        gridValues = new float[gridCount];
        for (int i = 0; i < gridCount; i++) {
            gridValues[i] = gameObject.GetComponent<Destructible>().GridPoints[i].val;
        }

    }

}
