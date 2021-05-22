using UnityEngine;

public class Worm : MonoBehaviour {

    [Header("Positions")]
    public Transform playerTransform;
    public Transform digPosition;

    private Vector3 target;
    [Header("Worm Parameters")]
    public float turnSpeed = 0.5f;
    public float closeTurnSpeed = 0.5f;
    public float farTurnSpeed = 0.5f;
    public float speed = 20;
    public float closeDistance = 30;
    public float farDistance = 50;
    public float unfocusTargetDistance = 150;
    public Vector2 unfocusTime;

    private float stopTime;
    private float stopTimer;
    private bool isTurning = false;
    private bool isMoving = true;

    private Digger digger;
    private AudioSource audioSource;
    private bool roaring = false;

    void Start()
    {
        target = playerTransform.position;
        digger = GetComponent<Digger>();
        audioSource = GetComponent<AudioSource>();
        digger.DigSize = 6;
        LoadWorm();
    }

    void Update()
    {
        if (isMoving) {
            target = playerTransform.position;
        }
        else {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopTime) {
                isMoving = true;
            }
        }
        float dist = Vector3.Distance(transform.position, playerTransform.transform.position);

        if (dist <= closeDistance) {
            isTurning = false;
            turnSpeed = closeTurnSpeed;
        }
        else if (!isTurning && dist >= farDistance) {
            isMoving = false;
            stopTimer = 0;
            target = playerTransform.position + Random.onUnitSphere * unfocusTargetDistance;
            stopTime = Random.Range(unfocusTime.x, unfocusTime.y);
            isTurning = true;
            turnSpeed = farTurnSpeed;
            roaring = false;
        }


        if (!roaring && dist < 100 + 10 && target == playerTransform.position) {
            audioSource.Play(0);
            roaring = true;
        }



        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        float str = Mathf.Min(turnSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

        transform.position += transform.forward * speed * Time.deltaTime;

        if (dist <= (GameManager.Instance.ChunkManager.ChunkSize * 2 - 5)) {
            digger.Dig(digPosition.position);
        }
    }

    public void LoadWorm()
    {
        Worm_Data data = SaveManager.LoadWorm();
        if (data != null) {
            Vector3 pos = new Vector3();
            pos.x = data.Position[0];
            pos.y = data.Position[1];
            pos.z = data.Position[2];
            transform.position = pos;

            Quaternion rot = new Quaternion();
            rot.x = data.Rotation[0];
            rot.y = data.Rotation[1];
            rot.z = data.Rotation[2];
            rot.w = data.Rotation[3];
            transform.rotation = rot;
        }
    }
}
