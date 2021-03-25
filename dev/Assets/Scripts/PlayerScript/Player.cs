using UnityEngine;

public class Player : MonoBehaviour {

    Vector2 moveInput;
    public InputMaster controls;
    MovementController mc;

    private void Awake() {
        mc = GetComponent<MovementController>();

    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        PlayerInput();
    }

    void PlayerInput() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        mc.InputMove = moveInput;
        mc.InputJump |= Input.GetButton("Jump");
        mc.InputSprint = Input.GetButton("Sprint");
    }

}
