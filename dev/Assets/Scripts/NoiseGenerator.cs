using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public int Seed = 0;
    [Range(0.0f, 1.0f)]
    public float Scale = 1.0f;
    public int Octaves = 1;
    public int Persistence = 1;
    public Vector3 Offset;


    public ChunkManager ChunkManager;

    public float GetValue(Vector3 pos) {
        //float value = 0.0f;

        float noiseY = Mathf.PerlinNoise((pos.x + pos.z)  * Scale, (pos.y + pos.x)  * Scale) / 2;
        float noiseZ = Mathf.PerlinNoise((pos.x + pos.y)  * Scale, (pos.z + pos.y)  * Scale) / 2;


        return noiseY + noiseZ;
    }

}
