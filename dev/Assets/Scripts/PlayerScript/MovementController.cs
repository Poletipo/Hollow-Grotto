using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f, maxStairsAngle = 50f;

    [SerializeField, Range(0f, 100f)]
    float maxSnapSpeed = 100f;

    [SerializeField, Min(0f)]
    float probeDistance = 1f;

    [SerializeField]
    Transform playerInputSpace = default;

    public bool InputJump { get; set; }
    public Vector3 InputMove { get; set; }

    Vector3 velocity;
    Vector3 desiredVelocity;
    Vector3 contactNormal;


    float fallMultiplier = 2.5f;
    float lowJumpMultiplier = 2f;

    float dotMaxGroundAngle;

    bool OnGround => groundContactCount > 0;

    Rigidbody rb;
    private int groundContactCount;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dotMaxGroundAngle = Mathf.Cos(maxGroundAngle);
        Debug.Log(dotMaxGroundAngle);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
        UpdateJump();
        ClearState();
    }

    private void ClearState() {
        groundContactCount = 0;
    }

    void UpdateMove() {
        InputMove = Vector2.ClampMagnitude(InputMove, 1f);

        Vector3 forward = playerInputSpace.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = playerInputSpace.right;
        right.y = 0f;
        right.Normalize();
        desiredVelocity =
            (forward * InputMove.y + right * InputMove.x);
    }

    void Jump() {

        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        rb.velocity += Vector3.up * jumpSpeed;
    }


    void UpdateJump() {

        //Debug.Log(OnGround +" , " + groundContactCount + " , " + dotMaxGroundAngle);

        if (InputJump && OnGround) {
            Jump();
        }
        //Better Jump
        if (rb.velocity.y < 0) {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !InputJump) {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate() {
        UpdateMoveAcceleration();
        UpdateJump();
    }

    private void UpdateMoveAcceleration() {
        Vector3 velocity = rb.velocity;

        velocity.x += desiredVelocity.x * maxAcceleration * Time.deltaTime;
        velocity.z += desiredVelocity.z * maxAcceleration * Time.deltaTime;

        Vector2 moveVelocity = Vector2.ClampMagnitude(new Vector2(velocity.x, velocity.z), maxSpeed);

        velocity = new Vector3(moveVelocity.x, velocity.y, moveVelocity.y);
        //Debug.Log(velocity);
        rb.velocity = velocity;
    }


    bool IsGrounded() {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down*1.5f,Color.red);
        return Physics.Raycast(ray, 1.5f);
    }

    private void OnCollisionEnter(Collision collision) {
        EvaluateCollisions(collision);
    }
    private void OnCollisionStay(Collision collision) {
        EvaluateCollisions(collision);
    }

    private void EvaluateCollisions(Collision collision) {

        for (int i = 0; i < collision.contactCount; i++) {

            Vector3 normal = collision.GetContact(i).normal;

            float dotValue = Vector3.Dot(transform.up, normal);
                Debug.Log(dotValue + " , " + dotMaxGroundAngle);
            if(dotValue > dotMaxGroundAngle) {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }




    }
}
