using UnityEngine;

public class Sonar : MonoBehaviour {
    ParticleSystem sonar;
    Player player;

    public bool isActive = true;
    public bool useDistance = false;
    private bool isEmmiting = false;
    public float minDistance = 50;

    private Vector3 originalSize;
    float dist = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();
        player.OnListenSonar += OnSonarOn;
        player.OnStopListenSonar += OnSonarOff;

        sonar = GetComponent<ParticleSystem>();
        originalSize = transform.localScale;
    }

    private void Update()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);

        float SonarSize = Mathf.Clamp((dist / 50.0f), 1, 9999999999999);
        transform.localScale = originalSize * SonarSize;
        if (sonar.isPlaying) {
            if (useDistance && dist > minDistance) {
                sonar.Stop();
            }
        }
        else if (useDistance && dist <= minDistance && isEmmiting) {
            sonar.Play();
        }

    }

    private void OnSonarOn(Player player)
    {
        isEmmiting = true;
        if (isActive) {
            if (useDistance && dist <= minDistance) {
                sonar.Play();
            }
            else if (!useDistance) {
                sonar.Play();
            }
        }
    }

    private void OnSonarOff(Player player)
    {
        sonar.Stop();
        isEmmiting = false;
    }


}
