using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChunkManager : MonoBehaviour
{
    [Range(1, 100)]
    public int GridResolution = 4;
    public int ChunkSize = 4;

    public MeshGenerator MeshGenerator;
    public NoiseGenerator NoiseGenerator;
     

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
