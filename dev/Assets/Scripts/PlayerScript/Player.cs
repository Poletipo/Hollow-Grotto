using UnityEngine;

public class Player : MonoBehaviour {

    public enum PlayerState {
        Moving,
        Digging
    }

    public enum InRange {
        Nothing,
        Destructible,
        Interactible,
        Grapple
    }

    public delegate void PlayerEvent(Player player);

    public PlayerEvent OnDigging;
    public PlayerEvent OnDigPercentChange;
    public PlayerEvent OnDigOverheating;
    public PlayerEvent OnInRangeChange;
    public PlayerEvent OnDigSizeChanged;
    public PlayerEvent OnListenSonar;
    public PlayerEvent OnStopListenSonar;
    public PlayerEvent OnGrappleTimerChange;
    public PlayerEvent OnFixedRobotCountChange;

    [Header("Player Parameters")]
    public float range = 2.5f;

    [Header("Dig Parameter")]
    public float digInterval = 0.5f;
    public float DigCooldownSpeed = 1.0f;
    public bool InfiniteDigging = false;
    public GameObject Rocks;

    private float digIntervalTimer = 0;
    private float DigSizeClick;

    [Header("grappling Parameter")]
    public bool InfiniteGrapple;
    public float grappleInterval = 5f;
    public float grappleCooldownSpeed = 1.0f;


    [Header("Other Parameter")]
    public Health health;
    Vector2 moveInput;
    [HideInInspector]
    public FirstPersonCamera fps;
    [HideInInspector]
    public Camera cam;
    public AudioClip digSound;

    private MovementController mc;
    private AudioSource audioSource;
    private GrapplingHook grapplingHook;
    private Digger digger;


    private int _fixedRobotCount = 0;
    public int FixedRobotCount {
        get { return _fixedRobotCount; }
        set {
            _fixedRobotCount = value;
            OnFixedRobotCountChange?.Invoke(this);
        }
    }

    public RaycastHit hit;


    private float _grappleTimer = 999;
    public float GrappleTimer {
        get { return _grappleTimer; }
        set {
            _grappleTimer = value;
            OnGrappleTimerChange?.Invoke(this);
        }
    }

    private bool _isOverHeating = false;
    public bool IsOverHeating {
        get { return _isOverHeating; }
        set {
            _isOverHeating = value;
            OnDigOverheating?.Invoke(this);
        }
    }


    private float _digPercent = 0;
    public float DigPercent {
        get { return _digPercent; }
        set {
            _digPercent = Mathf.Clamp(value, 0.0f, 100.0f);
            OnDigPercentChange?.Invoke(this);
        }
    }


    private InRange _inRangeState = InRange.Nothing;
    public InRange InRangeState {
        get { return _inRangeState; }
        set {

            InRange temp = InRangeState;
            _inRangeState = value;

            if (temp != value) {
                OnInRangeChange?.Invoke(this);
            }
        }
    }


    private bool _playerEnabled;
    public bool PlayerEnabled {
        get { return _playerEnabled; }
        set { _playerEnabled = value; }
    }


    private float _digSize = 4;
    public float DigSize {
        get { return _digSize; }
        set {
            _digSize = Mathf.Clamp(value, 1.5f, 8.0f);
            digger.DigSize = _digSize;
            OnDigSizeChanged?.Invoke(this);
        }
    }

    private void Awake()
    {
        mc = GetComponent<MovementController>();
        mc.OnLanding += OnLanding;
        cam = Camera.main;
        health = GetComponent<Health>();
        health.OnDeath += OnDeath;
        fps = cam.GetComponent<FirstPersonCamera>();
        digger = GetComponent<Digger>();
        audioSource = GetComponent<AudioSource>();
        grapplingHook = GetComponent<GrapplingHook>();
        digger.DigSize = DigSize;
    }

    private void Start()
    {
        LoadPlayer();
        DigSizeClick = DigSize;
    }

    void Update()
    {
        digIntervalTimer -= Time.deltaTime;

        DigPercent -= DigCooldownSpeed * Time.deltaTime;
        if (InfiniteDigging) {
            DigPercent = 0;
        }
        if (DigPercent <= 30 && IsOverHeating) {
            IsOverHeating = false;
        }


        if (InfiniteGrapple) {
            GrappleTimer = grappleInterval;
        }
        if (GrappleTimer < grappleInterval) {
            GrappleTimer += grappleCooldownSpeed * Time.deltaTime;
        }


        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, ~LayerMask.NameToLayer("Object"))) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Destructible")) {
                InRangeState = InRange.Destructible;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactible")) {
                if (hit.collider.GetComponent<Interactible>().Active) {
                    InRangeState = InRange.Interactible;
                }
            }
            else {
                InRangeState = InRange.Nothing;
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, grapplingHook.range) && GrappleTimer >= grappleInterval) {
            InRangeState = InRange.Grapple;
        }
        else {
            InRangeState = InRange.Nothing;
        }

        PlayerInput();
    }

    void PlayerInput()
    {

        if (PlayerEnabled) {

            float sw = Input.GetAxis("Mouse ScrollWheel");

            if (sw != 0) {
                DigSizeClick = Mathf.Clamp(DigSizeClick + 0.5f * sw, 1.5f, 8f);
                DigSize = Mathf.Floor(DigSizeClick * 10) / 10.0f;
            }

            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            mc.InputMove = moveInput;
            mc.InputJump |= Input.GetButton("Jump");

            mc.InputSprint = Input.GetButton("Sprint");
            if (Input.GetButton("Fire1")) {
                if (digIntervalTimer <= 0 && !IsOverHeating) {
                    Dig();
                }
            }
            if (Input.GetButtonDown("Fire2")) {
                if (InRangeState != InRange.Nothing && GrappleTimer >= grappleInterval) {
                    grapplingHook.Grapple(hit.point);
                }
            }
            else if (Input.GetButtonUp("Fire2") && grapplingHook.isGrappled) {
                GrappleTimer = 0;
                grapplingHook.UnGrapple();
            }



            if (Input.GetButtonDown("Interact") && InRangeState == InRange.Interactible) {
                hit.collider.GetComponent<Interactible>().Interact();
            }

            if (Input.GetButtonDown("Sonar")) {
                OnListenSonar?.Invoke(this);
            }
            else if (Input.GetButtonUp("Sonar")) {
                OnStopListenSonar?.Invoke(this);
            }
        }
    }

    void Dig()
    {
        digIntervalTimer = digInterval;
        DigPercent += 1.5F;

        OnDigging?.Invoke(this);
        if (InRangeState == InRange.Destructible) {
            digger.Dig(hit.point);
            Instantiate(Rocks, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
            DigPercent += DigSize * 2.5f;
            audioSource.PlayOneShot(digSound);
        }
        if (DigPercent >= 100) {
            IsOverHeating = true;
        }
    }

    public void LoadPlayer()
    {
        Player_Data data = SaveManager.LoadPlayer();
        if (data != null) {
            Vector3 pos = new Vector3();
            pos.x = data.Position[0];
            pos.y = data.Position[1];
            pos.z = data.Position[2];
            transform.position = pos;

            Vector3 camRot = new Vector3(0, 0, 0);
            camRot.x = data.CamRotation;

            fps.viewRotation = camRot;
            Vector3 rot = new Vector3(0, 0, 0);
            rot.y = data.BodyRotation;
            fps.bodyRotation = rot;

            DigSize = data.DigSize;
            health.SetHealth(data.Health);

            FixedRobotCount = data.FixedRobotCount;
        }
    }

    private void OnDeath(Health health)
    {
        PlayerEnabled = false;
    }

    private void OnLanding(MovementController movementController)
    {
        if (movementController.FallingSpeed <= -20) {
            health.Hurt((int)(-(movementController.FallingSpeed) - 10));
        }
    }
}
