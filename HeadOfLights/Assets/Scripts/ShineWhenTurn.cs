using UnityEngine;

public class ShineWhenTurn : MonoBehaviour
{
    public new ParticleSystem particleSystem;
    public float fadeDuration = 1f;

    private float fadeTimer = 0f;
    private bool isFading = false;

    private Gradient gradient;
    private ParticleSystem.ColorOverLifetimeModule colorModule;

    void Start()
    {
        if (particleSystem == null)
            particleSystem = GetComponentInChildren<ParticleSystem>();

        colorModule = particleSystem.colorOverLifetime;
        colorModule.enabled = true;

        // Crée un gradient d’opacité complet (par défaut à opaque)
        gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );

        colorModule.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    void Update()
    {
        // Si le jeu est terminé, on force l'affichage et la lecture des particules
        if (DiscoBallManager.gameEnded)
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.gameObject.SetActive(true);
                ResetGradient(1f);
                particleSystem.Play();
                isFading = false;
            }
            return; // On ne fait rien d'autre
        }

        if (DiscoBallManager.makeDiscoBallTurn)
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.gameObject.SetActive(true);
                ResetGradient(1f);
                particleSystem.Play();
                isFading = false;
            }
        }
        else
        {
            if (particleSystem.isPlaying && !isFading)
            {
                fadeTimer = 0f;
                isFading = true;
            }
        }

        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeTimer / fadeDuration);
            float alpha = Mathf.Lerp(1f, 0f, t);
            ResetGradient(alpha);

            if (t >= 1f)
            {
                particleSystem.Stop();
                particleSystem.gameObject.SetActive(false);
                isFading = false;
            }
        }
    }

    void ResetGradient(float alpha)
    {
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0f) },
            new GradientAlphaKey[] {
                new GradientAlphaKey(alpha, 0f),
                new GradientAlphaKey(alpha, 1f)
            }
        );
        colorModule.color = new ParticleSystem.MinMaxGradient(gradient);
    }
}
