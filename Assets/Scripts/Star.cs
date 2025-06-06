using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour
{
    public Transform vrCamera;
    public float maxDistance = 10f;
    private Renderer sphereRenderer;
    private bool isLookedAt = false;
    public ParticleSystem particles;

    public bool IsLit { get; set; }

    public AudioClip lightUpClip;
    private AudioSource audioSource;

    public Material unlitMaterial;
    public Material litMaterial;

    public GameObject planeObject;

    private float rotationSpeed; // 🌟 vitesse de rotation individuelle


    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
        StarManager.Instance.RegisterStar(this);

        if (particles != null)
            particles.Stop();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (unlitMaterial != null)
            sphereRenderer.material = unlitMaterial;

        if (planeObject != null)
            planeObject.SetActive(false);

        rotationSpeed = Random.Range(3f, 8f);

    }

    void Update()
    {
        if (IsLit) return;

        Ray ray = new Ray(vrCamera.position, vrCamera.forward);
        RaycastHit hit;
        int layerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            if (hit.transform == transform && !isLookedAt)
            {
                LightUp();
                isLookedAt = true;
            }
            else if (hit.transform != transform)
            {
                isLookedAt = false;
            }
        }
        else
        {
            isLookedAt = false;
        }

        if (IsLit)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }

    void LightUp()
    {
        if (!IsLit) {
            Debug.Log("Star lit: " + gameObject.name);

            if (litMaterial != null)
                sphereRenderer.material = litMaterial;

            if (particles != null)
                particles.Play();

            if (planeObject != null)
                StartCoroutine(GlitchPlane());

            PlayLightUpSound();

            StarManager.Instance.StarLit(this);

            IsLit = true;
        }
    }

    void PlayLightUpSound()
    {
        if (lightUpClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(lightUpClip);
        }
    }

    // ✅ Coroutine de glitch
    IEnumerator GlitchPlane()
    {
        for (int i = 0; i < 4; i++)
        {
            planeObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.12f));
            planeObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.03f, 0.1f));
        }

        planeObject.SetActive(true); // affichage final
    }
}
