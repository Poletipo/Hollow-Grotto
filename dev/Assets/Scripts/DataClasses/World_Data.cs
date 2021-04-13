[System.Serializable]
public class World_Data {

    public int Seed;

    public World_Data(NoiseGenerator NoiseParams)
    {
        Seed = NoiseParams.Seed;

    }

}
