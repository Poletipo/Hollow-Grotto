using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour {

    public enum MovementState {
        Idle,
        Moving,
        Sliping,
        Jumping,
        Falling
    }

    public delegate void MovementEvent(MovementController movementController);
    public MovementEvent OnIdle;
    public MovementEvent OnMoving;
    public MovementEvent OnSliping;
    public MovementEvent OnFalling;
    public MovementEvent OnLanding;
    public MovementEvent OnJumping;

    private MovementState _moveState = MovementState.Idle;

    public MovementState MoveState {
        get { return _moveState; }
        set {
            _moveState = value;
        }
    }

    public Vector3 Velocity { get; set; }

    float changeStateTimer = 0;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    float maxSprintSpeed = 15f;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxDecceleration = 20f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f;

    [SerializeField]
    Transform playerInputSpace = default;

    public bool InputJump { get; set; }
    public bool InputSprint { get; set; }
    public Vector3 InputMove { get; set; }

    Vector3 velocity;
    Vector3 desiredVelocity;
    Vector3 contactNormal;
    Vector3 steepNormal;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    float dotMaxGroundAngle;

    bool onGround;
    bool isJumping = false;

    Rigidbody rb;
    private int groundContactCount;
    private int steepContactCount;
    private Vector3 prevPosition;

    float fallingTimer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        prevPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        dotMaxGroundAngle = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    void ClearState()
    {
        onGround = false;
        groundContactCount = 0;
        steepContactCount = 0;
        steepNormal = contactNormal = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateMove();
        UpdateJump();

        changeStateTimer += Time.deltaTime;
    }


    private void UpdateState()
    {

        Velocity = (transform.position - prevPosition) / Time.fixedDeltaTime;
        prevPosition = transform.position;
        if (onGround) {
            if (MoveState == MovementState.Jumping || MoveState == MovementState.Falling) {
                OnLanding?.Invoke(this);
            }

            if (Velocity.magnitude > 0.1f && MoveState != MovementState.Moving) {
                MoveState = MovementState.Moving;
                OnMoving?.Invoke(this);
            }
            else if (Velocity.magnitude <= 0.1f && MoveState != MovementState.Idle) {
                MoveState = MovementState.Idle;
                OnIdle?.Invoke(this);
            }
            changeStateTimer = 0;
        }
        else if (!onGround && steepContactCount > 0) {
            if (MoveState != MovementState.Sliping) {
                MoveState = MovementState.Sliping;
                OnSliping?.Invoke(this);
            }
            changeStateTimer = 0;
        }
        else if (!onGround && steepContactCount <= 0) {
            if (Velocity.y < 0.5f && MoveState != MovementState.Falling) {
                changeStateTimer += Time.fixedDeltaTime;
                if (changeStateTimer >= 0.5f) {
                    MoveState = MovementState.Falling;
                    OnFalling?.Invoke(this);
                    changeStateTimer = 0;
                }
            }
            else {
                changeStateTimer = 0;
            }
            if (Velocity.y >= 0.5f && MoveState != MovementState.Jumping && isJumping) {
                MoveState = MovementState.Jumping;
                OnJumping?.Invoke(this);
            }
        }
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;

        if (InputJump) {
            InputJump = false;
            Jump();
        }

        Move();

        rb.velocity = velocity;
        UpdateState();
        ClearState();
    }

    void UpdateMove()
    {
        InputMove = Vector2.ClampMagnitude(InputMove, 1f);

        Vector3 forward = playerInputSpace.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = playerInputSpace.right;
        right.y = 0f;
        right.Normalize();
        float speed = InputSprint ? maxSprintSpeed : maxSpeed;
        desiredVelocity =
            (forward * InputMove.y + right * InputMove.x) * speed;
    }

    void Jump()
    {
        if (onGround) {
            isJumping = true;

            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            velocity.y += Mathf.Max(jumpSpeed - velocity.y, 0f);
        }
    }

    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    void Move()
    {
        if (onGround) {
            rb.AddForce(-contactNormal.normalized * 9.8f);
        }
        else {
            rb.AddForce(Physics.gravity);
        }

        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        if (onGround) {
            acceleration = InputMove.magnitude > 0 ? maxAcceleration : maxDecceleration;
        }
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        Debug.DrawRay(transform.position, (xAxis * (newX - currentX) + zAxis * (newZ - currentZ)));

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    void UpdateJump()
    {
        if (!onGround) {
            //Better Jump
            if (rb.velocity.y < 0) {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !InputJump) {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            fallingTimer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollisions(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollisions(collision);
    }

    private void EvaluateCollisions(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++) {

            Vector3 normal = collision.GetContact(i).normal;

            float dotValue = Vector3.Dot(transform.up, normal);
            if (dotValue >= dotMaxGroundAngle) {
                groundContactCount += 1;
                contactNormal += normal;
            }
            else if (dotValue > -0.01f) {
                steepContactCount += 1;
                steepNormal += normal;
            }
        }

        if (groundContactCount <= 0 && steepContactCount > 0) {
            steepNormal.Normalize();
            float dotValue = Vector3.Dot(transform.up, steepNormal);
            if (dotValue >= dotMaxGroundAngle) {
                steepContactCount = 0;
                groundContactCount = 1;
                contactNormal = steepNormal;
            }
        }

        onGround = groundContactCount > 0;
        if (onGround) {
            isJumping = false;
        }

        if (onGround && rb.velocity.y <= -1 && fallingTimer > 0.5f) {
            rb.velocity /= Mathf.Abs(rb.velocity.y);
        }

        fallingTimer = onGround ? 0 : fallingTimer;

    }
}
