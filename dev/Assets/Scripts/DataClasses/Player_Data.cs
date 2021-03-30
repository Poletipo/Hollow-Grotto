using UnityEngine;

[System.Serializable]
public class Player_Data {

    public float[] Position;
    public float BodyRotation;
    public float CamRotation;

    public Player_Data(GameObject gameObject) {
        Position = new float[3];
        Position[0] = gameObject.transform.position.x;
        Position[1] = gameObject.transform.position.y;
        Position[2] = gameObject.transform.position.z;


        BodyRotation = gameObject.transform.rotation.eulerAngles.y;

        FirstPersonCamera fps = gameObject.GetComponent<Player>().fps;
        CamRotation = fps.viewRotation.x;

    }

}
