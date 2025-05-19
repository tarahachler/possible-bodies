using UnityEngine;

public class LookAtChangeColor : MonoBehaviour
{
    public Transform vrCamera;
    public float maxDistance = 10f;
    private Renderer sphereRenderer;
    private bool isLookedAt = false;
    public ParticleSystem particles;  // Référence au système de particules

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
        if (particles != null)
        {
            particles.Stop(); // Assure-toi qu’il est désactivé au départ
        }
    }

    void Update()
    {
        Ray ray = new Ray(vrCamera.position, vrCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.transform == transform && !isLookedAt)
            {
                ChangeColor();
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
    }

    void ChangeColor()
    {
        Color lightedColor = new Color(1.0f, 0.8f, 0.6f);
        sphereRenderer.material.color = lightedColor;

        if (particles != null)
        {
            particles.Play(); // Lance les particules
        }
    }
}
