using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public int Seed = 0;
    public float Scale = 1.0f;
    public int Octaves = 1;
    public int Persistence = 1;
    public Vector3 Offset;


    public ChunkManager ChunkManager;

    public float GetValue(Vector3 pos) {
        //float value = 0.0f;

        float noiseY = Mathf.PerlinNoise((pos.x + pos.z) / (ChunkManager.GridResolution ) * Scale, (pos.y + pos.x) / (ChunkManager.GridResolution ) * Scale) / 2;
        float noiseZ = Mathf.PerlinNoise((pos.x + pos.y) / (ChunkManager.GridResolution ) * Scale, (pos.z + pos.y) / (ChunkManager.GridResolution ) * Scale) / 2;


        return noiseY + noiseZ;
    }

}
