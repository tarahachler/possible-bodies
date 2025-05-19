using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    public ParticleSystem ps;
    public Transform target;
    public float attractionDistance = 1f;

    private ParticleSystem.Particle[] particles;
    private Collider targetCollider;

    void Start()
    {
        if (target != null)
        {
            targetCollider = target.GetComponent<Collider>();

            if (targetCollider == null)
            {
                Debug.LogError("Le target doit avoir un Collider pour utiliser ClosestPoint.");
            }
        }
    }

    void LateUpdate()
    {
        if (ps == null || target == null || targetCollider == null) return;

        if (particles == null || particles.Length < ps.main.maxParticles)
            particles = new ParticleSystem.Particle[ps.main.maxParticles];

        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 particleWorldPos = particles[i].position;

            float dist = Vector3.Distance(particleWorldPos, target.position);

            if (dist < attractionDistance)
            {
                // Trouver le point le plus proche sur le contour de l'objet (Collider)
                Vector3 closestPoint = targetCollider.ClosestPoint(particleWorldPos);

                // Approche progressive
                particles[i].position = Vector3.Lerp(particleWorldPos, closestPoint, 0.1f);
                particles[i].velocity = Vector3.zero;
                particles[i].remainingLifetime = 99999f; // elles ne meurent pas
            }
        }

        ps.SetParticles(particles, count);
    }
}
