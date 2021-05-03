using UnityEngine;

[System.Serializable]
public class Objective_Data {

    public float[] Position;
    public bool isFixed;
    public int healthRefill;

    public Objective_Data(GameObject gameObject)
    {
        Position = new float[3];
        Position[0] = gameObject.transform.position.x;
        Position[1] = gameObject.transform.position.y;
        Position[2] = gameObject.transform.position.z;

        isFixed = gameObject.GetComponent<Objective>().Fixed;
        healthRefill = gameObject.GetComponent<Objective>().HealthRefill;
    }

}
