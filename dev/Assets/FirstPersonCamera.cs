using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform rotationAxis;
    public Transform YawAxis;

    public float rotationSpeed = 1;

    Camera camera;
    Vector2 mouseInput;
    Vector3 bodyRotation = new Vector3();
    Vector3 viewRotation = new Vector3();

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = GetComponent<Camera>();
        transform.position = YawAxis.position;
        transform.parent = YawAxis;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput = new Vector2(
            -Input.GetAxis("Mouse Y"),
            Input.GetAxis("Mouse X")
        );

        bodyRotation += new Vector3(0, mouseInput.y, 0) * rotationSpeed;
        viewRotation += new Vector3(mouseInput.x,0, 0) * rotationSpeed;

    }

    private void LateUpdate() {
        rotationAxis.transform.rotation = Quaternion.Euler(bodyRotation);
        YawAxis.transform.localRotation = Quaternion.Euler(viewRotation);
    }
}
