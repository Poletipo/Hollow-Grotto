using UnityEngine;

public class Objective : MonoBehaviour {
    public GameObject Sonar;

    bool Active = true;

    private void Update()
    {

        float dist = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
        float SonarSize = Mathf.Clamp((dist / 50.0f), 1, 9999999999999999);


        Sonar.transform.localScale = Vector3.one * SonarSize;
    }


}
