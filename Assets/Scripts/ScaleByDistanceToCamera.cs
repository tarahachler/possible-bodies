using UnityEngine;

public class ScaleByDistanceToCamera : MonoBehaviour
{
    public Transform targetCamera;     // Référence vers la caméra (MainCamera)
    public float minDistance = 0.2f;   // Distance à laquelle la sphère est à sa taille max
    public float maxDistance = 2f;     // Distance à laquelle la sphère est à sa taille min
    public float minScale = 0.5f;      // Taille minimale de la sphère
    public float maxScale = 1f;        // Taille maximale de la sphère

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        if (targetCamera == null) return;

        float distance = Vector3.Distance(transform.position, targetCamera.position);

        // Clamp pour éviter les valeurs en dehors des bornes
        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
        float scaleValue = Mathf.Lerp(minScale, maxScale, t);

        transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }

}
