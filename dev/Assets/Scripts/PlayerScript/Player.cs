using UnityEngine;

public class Player : MonoBehaviour {

    Vector2 moveInput;
    public InputMaster controls;
    MovementController mc;
    Camera cam;
    Digger digger;
    public float digSize = 2;

    public float digInterval = 0.5f;
    float digIntervalTimer = 0;

    private void Awake() {
        mc = GetComponent<MovementController>();
        cam = Camera.main;
        digger = GetComponent<Digger>();
        digger.DigSize = digSize;
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        digIntervalTimer -= Time.deltaTime;

        PlayerInput();
    }

    void PlayerInput() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        mc.InputMove = moveInput;
        mc.InputJump |= Input.GetButton("Jump");
        mc.InputSprint = Input.GetButton("Sprint");
        if (Input.GetButton("Fire1")) {
            if (digIntervalTimer <= 0) {
                Dig();
                digIntervalTimer = digInterval;
            }
        }
    }


    void Dig() {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20, LayerMask.GetMask("Destructible"));
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 20, Color.red, 20);
        digger.Dig(hit.point);
    }

}
