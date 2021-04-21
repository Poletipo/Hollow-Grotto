using UnityEngine;

public class RocksParticle : MonoBehaviour {

    ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, ps.main.duration);
    }

}
