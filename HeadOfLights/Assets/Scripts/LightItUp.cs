using UnityEngine;

public class LightItUp : MonoBehaviour
{
    [SerializeField] private float maxIntensity = 5f;
    private Light dirLight;

    void Start()
    {
        dirLight = GetComponent<Light>();
        if (dirLight == null)
            Debug.LogWarning("Aucune Light trouvée sur ce GameObject !");
    }

    void Update()
    {
        if (StarManager.Instance == null)
            return;

        var starsField = typeof(StarManager).GetField("stars", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var starsList = starsField.GetValue(StarManager.Instance) as System.Collections.IEnumerable;

        int totalStars = 0;
        int litStars = 0;

        foreach (var starObj in starsList)
        {
            totalStars++;
            var star = starObj as Star;
            if (star != null && star.IsLit)
                litStars++;
        }

        float ratio = (totalStars > 0) ? (float)litStars / totalStars : 0f;
        float targetIntensity = ratio * maxIntensity;

        if (dirLight != null)
            dirLight.intensity = targetIntensity;
    }
}
