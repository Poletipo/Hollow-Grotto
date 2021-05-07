using UnityEngine;

[System.Serializable]
public class Worm_Data {

    public float[] Position;
    public float[] Rotation;

    public Worm_Data(GameObject gameObject)
    {
        Position = new float[3];
        Position[0] = gameObject.transform.position.x;
        Position[1] = gameObject.transform.position.y;
        Position[2] = gameObject.transform.position.z;


        Rotation = new float[4];

        Rotation[0] = gameObject.transform.rotation.x;
        Rotation[1] = gameObject.transform.rotation.y;
        Rotation[2] = gameObject.transform.rotation.z;
        Rotation[3] = gameObject.transform.rotation.w;
    }

}
