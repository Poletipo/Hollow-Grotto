using UnityEngine;

public class Worm : MonoBehaviour {

    public Transform playerTransform;
    public float turnSpeed = 0.5f;
    public float speed = 20;
    public float closeDistance = 30;
    public float farDistance = 50;
    float stopTime;
    float stopTimer;
    bool isTurning = false;
    bool isMoving = true;


    // Start is called before the first frame update
    void Start()
    {
        //playerTransform = GameManager.Instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) {

            float dist = Vector3.Distance(transform.position, playerTransform.transform.position);

            if (dist <= closeDistance) {
                isTurning = false;
                turnSpeed = 0.1f;
            }
            else if (!isTurning && dist >= farDistance) {
                isMoving = false;
                stopTimer = 0;
                stopTime = Random.Range(5.0f, 10.0f);
                isTurning = true;
                turnSpeed = 2f;
            }

            Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            float str = Mathf.Min(turnSpeed * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopTime) {
                isMoving = true;
            }
        }




    }

}
