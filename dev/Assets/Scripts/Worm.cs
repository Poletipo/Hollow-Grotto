using UnityEngine;

public class Worm : MonoBehaviour {

    public Transform playerTransform;
    public Transform digPosition;

    private Vector3 target;

    public float turnSpeed = 0.5f;
    public float closeTurnSpeed = 0.5f;
    public float farTurnSpeed = 0.5f;
    public float speed = 20;
    public float closeDistance = 30;
    public float farDistance = 50;
    float stopTime;
    float stopTimer;
    bool isTurning = false;
    bool isMoving = true;

    private Digger digger;

    // Start is called before the first frame update
    void Start()
    {
        target = playerTransform.position;
        digger = GetComponent<Digger>();
        digger.DigSize = 6;
    }

    // Update is called once per frame
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
            target = playerTransform.position + Random.onUnitSphere * 100;
            stopTime = Random.Range(5.0f, 10.0f);
            isTurning = true;
            turnSpeed = farTurnSpeed;
        }

        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        float str = Mathf.Min(turnSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

        transform.position += transform.forward * speed * Time.deltaTime;

        if (dist <= GameManager.Instance.ChunkManager.ChunkSize) {
            digger.Dig(digPosition.position);
        }


    }
}
