using UnityEngine;

public class Worm : MonoBehaviour {

    public Transform playerTransform;
    public float turnSpeed = 0.5f;
    public float speed = 20;

    // Start is called before the first frame update
    void Start()
    {
        //playerTransform = GameManager.Instance.Player.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
        float str = Mathf.Min(turnSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

        transform.position += transform.forward * speed * Time.deltaTime;

    }
}
