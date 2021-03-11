using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{

    public ComputeShader gridNoiseShader;

    public int Seed = 0;
    [Range(0.0f, 1.0f)]
    public float Scale = 1.0f;
    [Range(1, 10)]
    public int Octaves = 1;
    [Range(0.0f, 1.0f)]
    public float Persistence = 1;
    public Vector3 Offset;
    public Vector3 axesScale;


    public ChunkManager ChunkManager;

    public float GetValue(Vector3 pos) {
        //float value = 0.0f;
        //return Random.Range(0.0f, 1.0f);

        float noiseY = Mathf.PerlinNoise((pos.x + pos.z)  * Scale, (pos.y + pos.x)  * Scale) / 2;
        float noiseZ = Mathf.PerlinNoise((pos.x + pos.y)  * Scale, (pos.z + pos.y)  * Scale) / 2;

        return noiseY + noiseZ;
    }

    // https://www.youtube.com/c/SebastianLague/featured
    public Vector3[] SeedValues() {
        var prng = new System.Random(Seed);
        var offsets = new Vector3[Octaves];
        float offsetRange = 1000;
        for (int i = 0; i < Octaves; i++) {
            offsets[i] = new Vector3((float)prng.NextDouble() * 2 - 1, (float)prng.NextDouble() * 2 - 1, (float)prng.NextDouble() * 2 - 1) * offsetRange;
        }
        return offsets;
    }

}
