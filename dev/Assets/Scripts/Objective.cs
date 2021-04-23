using TMPro;
using UnityEngine;

public class Objective : MonoBehaviour {
    public ParticleSystem Sonar;
    public GameObject RobotHead;
    public ParticleSystem Smoke;
    public TextMeshProUGUI SaveTxt;

    bool Fixed = false;
    private GameObject player;

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.GetComponent<Player>().OnListenSonar += OnSonarOn;
        player.GetComponent<Player>().OnStopListenSonar += OnSonarOff;
    }


    private void Update()
    {
        float dist = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
        float SonarSize = Mathf.Clamp((dist / 50.0f), 1, 9999999999999999);
        Sonar.transform.localScale = Vector3.one * SonarSize;

        Vector3 toLook = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        RobotHead.transform.rotation = Quaternion.LookRotation(transform.position - toLook, Vector3.up);
    }

    private void OnSonarOn(Player player)
    {
        if (!Fixed) {
            Sonar.Play();
        }
    }

    private void OnSonarOff(Player player)
    {
        Sonar.Stop();
    }

}
