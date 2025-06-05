using UnityEngine;

public class DiscoLight : MonoBehaviour
{
    [SerializeField] private float changeInterval = 0.5f;
    [SerializeField] private float fadeDuration = 1f;
    private Light _light;
    private float initialIntensity;
    private float fadeTimer = 0f;
    private bool isFading = false;

    private void Start()
    {
        _light = GetComponent<Light>();
        initialIntensity = _light.intensity;
        InvokeRepeating(nameof(ChangeLightColor), 0f, changeInterval);
    }

    private void Update()
    {
        // Ajoute cette gestion en priorité
        if (DiscoBallManager.gameEnded)
        {
            if (!_light.enabled)
            {
                _light.enabled = true;
                _light.intensity = initialIntensity;
                isFading = false;
                fadeTimer = 0f;
            }
            return; // On sort, on ne fait rien d'autre
        }

        if (DiscoBallManager.makeDiscoBallTurn)
        {
            if (!_light.enabled)
            {
                _light.enabled = true;
                _light.intensity = initialIntensity;
                isFading = false;
                fadeTimer = 0f;
            }
        }
        else
        {
            if (_light.enabled && !isFading)
            {
                fadeTimer = 0f;
                isFading = true;
            }
        }

        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeTimer / fadeDuration);
            _light.intensity = Mathf.Lerp(initialIntensity, 0f, t);

            if (t >= 1f)
            {
                _light.enabled = false;
                isFading = false;
            }
        }
    }

    private void ChangeLightColor()
    {
        if (_light.enabled)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            _light.color = randomColor;
        }
    }
}
