using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {
    [Header("FPS Parameters")]
    public Transform rotationAxis;
    public Transform YawAxis;
    public float rotationSpeed = 1;

    [HideInInspector]
    public Vector3 bodyRotation = new Vector3();
    [HideInInspector]
    public Vector3 viewRotation = new Vector3();

    private Vector2 mouseInput;
    private Player player;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.position = YawAxis.position;
        transform.rotation = YawAxis.rotation;
        player = GameManager.Instance.Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.PlayerEnabled) {
            mouseInput = new Vector2(
                -Input.GetAxis("Mouse Y"),
                Input.GetAxis("Mouse X")
            );

            bodyRotation += new Vector3(0, mouseInput.y, 0) * rotationSpeed;
            viewRotation += new Vector3(mouseInput.x, 0, 0) * rotationSpeed;
            viewRotation.x = Mathf.Clamp(viewRotation.x, -90, 90);

        }
    }

    private void LateUpdate()
    {
        rotationAxis.transform.rotation = Quaternion.Euler(bodyRotation);
        YawAxis.transform.localRotation = Quaternion.Euler(viewRotation);

        transform.rotation = YawAxis.rotation;
        transform.position = YawAxis.position;

    }
}
