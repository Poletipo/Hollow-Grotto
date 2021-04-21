using UnityEngine;

public class ParticuleDownTest : MonoBehaviour {
    ParticleSystem particuleSystem;
    // Start is called before the first frame update
    void Start()
    {

        particuleSystem = GetComponent<ParticleSystem>();
        particuleSystem.Play();


    }


    private void Update()
    {
        ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[particuleSystem.main.maxParticles];
        int partAlive = particuleSystem.GetParticles(m_Particles);
        RaycastHit hit;

        for (int i = 0; i < partAlive; i++) {
            if (Physics.Linecast(transform.TransformPoint(m_Particles[i].position + Vector3.up), Vector3.down, out hit, 5)) {
                ParticleSystem.Particle particle = m_Particles[i];
                particle.position = transform.InverseTransformPoint(hit.point);
                m_Particles[i] = particle;
            }
        }
        particuleSystem.SetParticles(m_Particles);
    }

}
