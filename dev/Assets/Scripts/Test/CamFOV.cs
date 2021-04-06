using UnityEngine;

public class CamFOV : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        SetHFov(30);
    }



    public static void SetHFov(float hFov) // hFov: Horizontal FOV from Blender
    {
        var vFov = 2 * Mathf.Atan(16 / 1.77f * hFov);
        vFov *= Mathf.Rad2Deg;
        Debug.Log(vFov);

    }

}
