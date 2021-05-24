// Code based on Tutorial:
// https://github.com/DaniDevy/FPS_Movement_Rigidbody/blob/master/GrapplingGun.cs

using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour {

    public float range = 15f;
    public Camera cam;
    public Transform lineBase;

    private SpringJoint joint;
    private Vector3 currentGrapplePosition;
    public bool isGrappled = false;
    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (isGrappled) {
            DrawRope();
        }
    }

    public void Grapple(Vector3 grabPoint)
    {
        isGrappled = true;

        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grabPoint;

        float distanceFromPoint = Vector3.Distance(transform.position, grabPoint);

        joint.maxDistance = 1;
        joint.minDistance = 1;

        joint.spring = 10f;
        joint.damper = 4f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = grabPoint;
    }

    public void UnGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        isGrappled = false;
    }

    void DrawRope()
    {
        lr.SetPosition(0, lineBase.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

}
