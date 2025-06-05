using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    public ParticleSystem ps;
    public Transform target;
    public float attractionDistance = 0.2f;

    public AudioClip attractClip;  // Son quand particule est attirée
    public AudioClip attachClip;   // Son quand particule s'attache définitivement

    private AudioSource audioSource;

    private ParticleSystem.Particle[] particles;
    private Vector3[] attachedOffsets;
    private bool[] attached;
    private bool[] wasAttracted;  // Pour détecter la transition d’état
    private Collider targetCollider;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (target != null)
        {
            targetCollider = target.GetComponent<Collider>();
            if (targetCollider == null)
            {
                Debug.LogError("Le target doit avoir un Collider pour utiliser ClosestPoint.");
            }
        }

        int maxParticles = ps.main.maxParticles;
        particles = new ParticleSystem.Particle[maxParticles];
        attachedOffsets = new Vector3[maxParticles];
        attached = new bool[maxParticles];
        wasAttracted = new bool[maxParticles];
    }

    void LateUpdate()
    {
        if (ps == null || target == null || targetCollider == null) return;

        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            if (attached[i])
            {
                // Particule attachée → suit le corps
                particles[i].position = target.TransformPoint(attachedOffsets[i]);
                particles[i].velocity = Vector3.zero;
                particles[i].remainingLifetime = 99999f;
                continue;
            }

            Vector3 particleWorldPos = particles[i].position;
            float dist = Vector3.Distance(particleWorldPos, target.position);

            bool currentlyAttracted = dist < attractionDistance;

            // Jouer son quand la particule commence à être attirée
            if (currentlyAttracted && !wasAttracted[i])
            {
                PlaySound(attractClip);
                wasAttracted[i] = true;
            }
            else if (!currentlyAttracted)
            {
                wasAttracted[i] = false;
            }

            if (currentlyAttracted)
            {
                Vector3 closestPoint = targetCollider.ClosestPoint(particleWorldPos);
                particles[i].position = Vector3.Lerp(particleWorldPos, closestPoint, 0.1f);
                particles[i].velocity = Vector3.zero;

                if (Vector3.Distance(particles[i].position, closestPoint) < 0.01f)
                {
                    attached[i] = true;
                    attachedOffsets[i] = target.InverseTransformPoint(closestPoint);
                    particles[i].remainingLifetime = 99999f;

                    // Son quand la particule s’attache
                    PlaySound(attachClip);
                }
            }
        }

        ps.SetParticles(particles, count);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
