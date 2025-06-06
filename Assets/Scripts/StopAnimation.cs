using UnityEngine;

public class StopAnimation : MonoBehaviour
{
    public float stopAfterSeconds = 3f; // Durée avant arrêt (modifiable dans l’inspecteur)
    private float timer = 0f;
    private Animator animator;
    private Renderer rend;
    private Color originalColor;
    private bool isFading = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stopAfterSeconds && animator != null && !isFading)
        {
            isFading = true;
            animator.enabled = false; // Arrête l’animation
        }

        // Fade out progressif après arrêt de l’animation
        if (isFading && rend != null)
        {
            float fadeDuration = 1f; // Durée du fade out en secondes
            float fadeAmount = Mathf.Clamp01(1 - (timer - stopAfterSeconds) / fadeDuration);
            Color fadedColor = originalColor;
            fadedColor.a = fadeAmount;
            rend.material.color = fadedColor;

            if (fadeAmount <= 0f)
            {
                // Optionnel : désactive l’objet après le fade out
                gameObject.SetActive(false);
            }
        }
    }
}
