using UnityEngine;

public class Player : MonoBehaviour {

    Vector2 moveInput;
    public InputMaster controls;
    MovementController mc;
    public FirstPersonCamera fps;
    Camera cam;
    Digger digger;
    public float digSize = 2;

    public float digInterval = 0.5f;
    float digIntervalTimer = 0;

    private void Awake() {
        mc = GetComponent<MovementController>();
        cam = Camera.main;
        fps = cam.GetComponent<FirstPersonCamera>();
        digger = GetComponent<Digger>();
        digger.DigSize = digSize;
    }

    private void Start() {
        LoadPlayer();
    }

    private void OnApplicationQuit() {
        SavePlayer();
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
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 2, Color.red, 20);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3, LayerMask.GetMask("Destructible"))) {
            digger.Dig(hit.point);
        }
    }

    void SavePlayer() {
        SaveManager.SavePlayer(gameObject);
    }
    void LoadPlayer() {
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
