using System;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour {

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
    public float noiseMul = 10;

    public ChunkManager ChunkManager;

    private void Awake()
    {
        if (SaveManager.SaveExist()) {
            World_Data data = SaveManager.LoadWorld();
            Seed = data.Seed;
        }
        else {
            Seed = RandomSeed();
        }
        SetSeed();
    }


    public void SetSeed()
    {
        UnityEngine.Random.InitState(Seed);
        Scale = UnityEngine.Random.Range(0.04f, 0.1f);
        Persistence = UnityEngine.Random.Range(0.1f, 0.2f);
        axesScale.x = UnityEngine.Random.Range(0.8f, 2.0f);
        axesScale.z = axesScale.x;
        axesScale.y = UnityEngine.Random.Range(0.8f, 2.0f);
        noiseMul = UnityEngine.Random.Range(4, 10f);
    }

    // https://www.youtube.com/c/SebastianLague/featured
    public Vector3[] SeedValues()
    {
        var prng = new System.Random(Seed);
        var offsets = new Vector3[Octaves];
        float offsetRange = 1000;
        for (int i = 0; i < Octaves; i++) {
            offsets[i] = new Vector3((float)prng.NextDouble() * 2 - 1, (float)prng.NextDouble() * 2 - 1, (float)prng.NextDouble() * 2 - 1) * offsetRange;
        }
        return offsets;
    }


    public int RandomSeed()
    {
        return (int)DateTime.Now.Ticks;
    }


}
