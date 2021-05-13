using UnityEngine;

[System.Serializable]
public class Player_Data {

    public float[] Position;
    public float BodyRotation;
    public float CamRotation;
    public float DigSize;
    public int Health;
    public int FixedRobotCount;

    public Player_Data(GameObject gameObject)
    {
        Position = new float[3];
        Position[0] = gameObject.transform.position.x;
        Position[1] = gameObject.transform.position.y;
        Position[2] = gameObject.transform.position.z;


        BodyRotation = gameObject.transform.rotation.eulerAngles.y;

        Player player = gameObject.GetComponent<Player>();
        FirstPersonCamera fps = player.fps;
        CamRotation = fps.viewRotation.x;

        FixedRobotCount = player.FixedRobotCount;
        DigSize = player.DigSize;
        Health = player.health.hp;
    }

}
