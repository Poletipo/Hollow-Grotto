using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour {

    public Sonar Sonar;
    public GameObject RobotHead;
    public ParticleSystem Smoke;
    public TextMeshProUGUI SaveTxt;
    public Image HealthSlider;
    public Light Light;

    public Interactible repairInteract;
    public Interactible healInteract;

    private bool _fixed = false;
    private int _healthRefill = 3;


    public int HealthRefill {
        get { return _healthRefill; }
        set {
            _healthRefill = value;
            if (value <= 0) {
                healInteract.Active = false;
            }
            HealthSlider.fillAmount = _healthRefill / 3.0f;
        }
    }

    public bool Fixed {
        get { return _fixed; }
        set {
            _fixed = value;
            if (value) {
                FixObjective();
            }
        }
    }

    private void FixObjective()
    {
        Smoke.Stop();
        Light.color = new Color(0.3f, 0.8f, 1);
        Sonar.isActive = false;
        GameManager.Instance.ChunkManager.GenerateObjectif();
    }

    private void SetFixObjective()
    {
        Smoke.Stop();
        Light.color = new Color(0.3f, 0.8f, 1);
        Sonar.isActive = false;
    }

    private GameObject player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        Vector3 toLook = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        RobotHead.transform.rotation = Quaternion.LookRotation(transform.position - toLook, Vector3.up);
    }

    public void LoadObjective(Objective_Data data)
    {
        Vector3 pos = new Vector3();
        pos.x = data.Position[0];
        pos.y = data.Position[1];
        pos.z = data.Position[2];
        transform.position = pos;

        _fixed = data.isFixed;
        if (data.isFixed) {
            SetFixObjective();
            repairInteract.Active = false;
        }
        HealthRefill = data.healthRefill;
    }



}
