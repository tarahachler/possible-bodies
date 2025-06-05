using UnityEngine;
using System.Collections;

public class RotateForSeconds : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrés/seconde
    public float duration = 1f;       // durée de rotation après que l’utilisateur ait vu la boule

    public AudioClip startRotationClip; // Son au démarrage
    public AudioClip stopRotationClip;  // Son à l'arrêt

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.01f;

    public Material stoppedMaterial; // <-- Ajoute cette ligne
    public float materialTransitionDuration = 1f; // Durée de la transition

    private AudioSource audioSource;
    private float stopTimer = 0f;
    private bool isStopping = false;
    private bool wasRotating = false; // Pour détecter les changements d'état
    private Vector3 originalPosition;

    private Material originalMaterial; // Pour restaurer si besoin
    private MeshRenderer meshRenderer; // Pour accéder au renderer

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        originalPosition = transform.localPosition;

        // Récupère le MeshRenderer et le material d'origine
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
            originalMaterial = meshRenderer.material;
    }

    void Update()
    {
        // Si le jeu est terminé, la boule tourne sans interruption
        if (DiscoBallManager.gameEnded)
        {
            if (!wasRotating)
            {
                PlayClip(startRotationClip);
                wasRotating = true;
            }
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            return; // On sort pour ne pas exécuter le reste du code
        }

        if (DiscoBallManager.makeDiscoBallTurn)
        {
            if (!wasRotating)
            {
                PlayClip(startRotationClip);
                wasRotating = true;
            }

            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

            if (DiscoBallManager.userSawDiscoBall)
            {
                if (!isStopping)
                {
                    stopTimer = 0f;
                    isStopping = true;
                }

                stopTimer += Time.deltaTime;

                if (stopTimer >= duration)
                {
                    DiscoBallManager.makeDiscoBallTurn = false;
                    isStopping = false;
                    Debug.Log("Rotation arrêtée après " + duration + "s.");
                }
            }
        }
        else
        {
            if (wasRotating)
            {
                PlayClip(stopRotationClip); // jouer à l'arrêt
                StartCoroutine(Shake());   // lance l'effet de shake

                // Change le material si assigné
                if (meshRenderer != null && stoppedMaterial != null)
                    meshRenderer.material = stoppedMaterial;

                wasRotating = false;
            }

            isStopping = false;
            stopTimer = 0f;
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            float z = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    IEnumerator TransitionMaterial(Material fromMat, Material toMat, float duration)
    {
        if (meshRenderer == null || fromMat == null || toMat == null)
            yield break;

        float elapsed = 0f;
        Color fromColor = fromMat.HasProperty("_Color") ? fromMat.color : Color.white;
        Color toColor = toMat.HasProperty("_Color") ? toMat.color : Color.white;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Color lerped = Color.Lerp(fromColor, toColor, t);
            meshRenderer.material.color = lerped;
            elapsed += Time.deltaTime;
            yield return null;
        }
        meshRenderer.material.color = toColor;
        meshRenderer.material = toMat; // Assure que le matériau final est bien assigné
    }
}
