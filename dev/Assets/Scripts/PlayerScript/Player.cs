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

    [Header("Player Parameters")]
    public float range = 2.5f;

    [Header("Animation")]
    public Animator animator;

    [Header("Dig Parameter")]
    public float digSize = 2;
    public float digInterval = 0.5f;
    public float DigCooldownSpeed = 1.0f;
    float digIntervalTimer = 0;
    bool canDig = true;
    private bool _isOverHeating = false;

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




    Vector2 moveInput;
    [HideInInspector]
    public FirstPersonCamera fps;

    MovementController mc;
    public Camera cam;
    Digger digger;


    private void Awake()
    {
        mc = GetComponent<MovementController>();
        cam = Camera.main;
        fps = cam.GetComponent<FirstPersonCamera>();
        digger = GetComponent<Digger>();

        digger.DigSize = digSize;
    }

    private void Start()
    {
        LoadPlayer();
    }

    private void OnApplicationQuit()
    {
        SavePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        digIntervalTimer -= Time.deltaTime;

        DigPercent -= DigCooldownSpeed * Time.deltaTime;
        if (DigPercent <= 30 && IsOverHeating) {
            IsOverHeating = false;
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range)) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Destructible")) {
                InRangeState = InRange.Destructible;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactible")) {
                InRangeState = InRange.Destructible;
            }
        }
        else {
            InRangeState = InRange.Nothing;
        }

        PlayerInput();

    }

    void PlayerInput()
    {
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


    void Dig()
    {
        digIntervalTimer = digInterval;
        DigPercent += 5;

        animator.Play("Armature|Dig");
        if (InRangeState == InRange.Destructible) {
            digger.Dig(hit.point);
            DigPercent += 10;
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
