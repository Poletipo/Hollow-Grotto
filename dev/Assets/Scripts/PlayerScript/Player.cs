using UnityEngine;

public class Player : MonoBehaviour {

    public enum PlayerState {
        Moving,
        Digging
    }

    public enum InRange {
        Nothing,
        Destructible,
        Interactible
    }

    public delegate void PlayerEvent(Player player);

    public PlayerEvent OnDigPercentChange;
    public PlayerEvent OnDigOverheating;
    public PlayerEvent OnInRangeChange;
    public PlayerEvent OnDigSizeChanged;

    [Header("Player Parameters")]
    public float range = 2.5f;

    [Header("Animation")]
    public Animator animator;



    [Header("Dig Parameter")]
    public float digInterval = 0.5f;
    public float DigCooldownSpeed = 1.0f;
    public bool InfiniteDigging = false;
    public GameObject Rocks;
    float digIntervalTimer = 0;
    bool canDig = true;
    private bool _isOverHeating = false;

    [Header("Other Parameter")]
    public Health health;
    Vector2 moveInput;
    [HideInInspector]
    public FirstPersonCamera fps;
    [HideInInspector]
    public Camera cam;

    MovementController mc;
    Digger digger;


    RaycastHit hit;

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
            OnInRangeChange?.Invoke(this);
            _inRangeState = value;
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
        cam = Camera.main;
        health = GetComponent<Health>();
        fps = cam.GetComponent<FirstPersonCamera>();
        digger = GetComponent<Digger>();

        digger.DigSize = DigSize;
    }

    private void Start()
    {
        LoadPlayer();
    }

    // Update is called once per frame
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

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range)) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Destructible")) {
                InRangeState = InRange.Destructible;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactible")) {
                InRangeState = InRange.Interactible;
            }
        }
        else {
            InRangeState = InRange.Nothing;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            SavePlayer();
        }

        PlayerInput();

    }

    void PlayerInput()
    {
        if (PlayerEnabled) {

            float sw = Input.GetAxis("Mouse ScrollWheel");

            if (sw != 0) {
                DigSize += 5f * sw;
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
        }
    }


    void Dig()
    {
        digIntervalTimer = digInterval;
        DigPercent += 2;

        animator.Play("Armature|Dig");
        if (InRangeState == InRange.Destructible) {
            digger.Dig(hit.point);
            Instantiate(Rocks, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
            DigPercent += DigSize * 2.5f;
        }
        if (DigPercent >= 100) {
            IsOverHeating = true;
        }
    }

    void SavePlayer()
    {
        SaveManager.SavePlayer(gameObject);
    }
    void LoadPlayer()
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

        }
    }


}
