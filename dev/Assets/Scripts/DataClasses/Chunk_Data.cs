using UnityEngine;

[System.Serializable]
public class Chunk_Data {

    public int[] Coordonates;
    public Objective_Data[] objectives;
    public float[] gridValues;

    public Chunk_Data(GameObject gameObject)
    {
        Coordonates = new int[3];
        Coordonates[0] = gameObject.GetComponent<Chunk>().Coordonnate.x;
        Coordonates[1] = gameObject.GetComponent<Chunk>().Coordonnate.y;
        Coordonates[2] = gameObject.GetComponent<Chunk>().Coordonnate.z;

        int gridCount = gameObject.GetComponent<Destructible>().GridPoints.Length;
        gridValues = new float[gridCount];
        for (int i = 0; i < gridCount; i++) {
            gridValues[i] = gameObject.GetComponent<Destructible>().GridPoints[i].val;
        }

        objectives = new Objective_Data[gameObject.GetComponent<Chunk>().objectives.Count];
        for (int i = 0; i < objectives.Length; i++) {
            objectives[i] = new Objective_Data(gameObject.GetComponent<Chunk>().objectives[i]);
        }
    }

}
