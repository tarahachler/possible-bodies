using UnityEngine;

public class DiscoBallController : MonoBehaviour
{
    [Header("References")]
    public Transform discoBall;            // Enfant : visuel
    public Transform targetCamera;         // Caméra cible

    [Header("Positions")]
    public Vector3 originPosition = new Vector3(0, 2, 4);

    [Header("Rotation & Scale")]
    public float rotationSpeed = 90f;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    [Header("Approach Settings")]
    public float approachSpeed = 1.5f;
    public float stopDistance = 0.6f;

    [Header("Return Settings")]
    public float returnSpeed = 2f; // vitesse contrôlable depuis l'Inspector

    [Header("Timers")]
    public float lookStopDelay = 5f;

    [Header("Look Detection")]
    public float lookThreshold = 0.95f;

    [HideInInspector]
    public bool insideDiscoBall = false;

    private Vector3 velocity;
    private Vector3 originalScale;
    private bool isRotating = true;
    private bool isLookedAt = false;
    private bool approaching = false;
    private bool returning = false;
    private float lookTimer = 0f;

    void Start()
    {
        transform.position = originPosition;
        originalScale = Vector3.one;
        discoBall.localScale = originalScale;
        isRotating = true;
        isLookedAt = false;
        lookTimer = 0f;
        approaching = false;
        returning = false;
        insideDiscoBall = false;
    }

    void Update()
    {
        if (targetCamera == null || discoBall == null) return;

        if (returning)
        {
            ReturnToOrigin();
            return;
        }

        DetectLook();

        if (!isLookedAt && isRotating)
        {
            discoBall.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
        else if (isLookedAt && !approaching)
        {
            lookTimer += Time.deltaTime;
            if (lookTimer >= lookStopDelay)
            {
                isRotating = false;
                approaching = true;
            }
        }
        else if (approaching)
        {
            ApproachCamera();
        }

        if (!returning && insideDiscoBall && StarManager.Instance != null && StarManager.Instance.allStarsLit)
        {
            StartReturn();
        }
    }

    void DetectLook()
    {
        Vector3 toDisco = (transform.position - targetCamera.position).normalized;
        float dot = Vector3.Dot(targetCamera.forward, toDisco);

        if (dot > lookThreshold && !isLookedAt)
        {
            isLookedAt = true;
        }
    }

    void ApproachCamera()
    {
        Vector3 targetPos = targetCamera.position + targetCamera.forward * 0.5f;

        float totalDistance = Vector3.Distance(originPosition, targetPos);
        float currentDistance = Vector3.Distance(transform.position, originPosition);
        float t = Mathf.InverseLerp(0f, totalDistance, currentDistance);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, approachSpeed * Time.deltaTime);

        float scale = Mathf.Lerp(minScale, maxScale, t);
        discoBall.localScale = Vector3.one * scale;

        if (Vector3.Distance(transform.position, targetPos) < stopDistance)
        {
            approaching = false;
            insideDiscoBall = true;
        }
    }

    void StartReturn()
    {
        returning = true;
        insideDiscoBall = false;
    }

    void ReturnToOrigin()
    {
        transform.position = Vector3.SmoothDamp(transform.position, originPosition, ref velocity, 1f / returnSpeed);
        discoBall.localScale = Vector3.Lerp(discoBall.localScale, originalScale, Time.deltaTime * returnSpeed);

        if (Vector3.Distance(transform.position, originPosition) < 0.01f &&
            Vector3.Distance(discoBall.localScale, originalScale) < 0.01f)
        {
            transform.position = originPosition;
            discoBall.localScale = originalScale;

            returning = false;
            isRotating = true;
            isLookedAt = false;
            lookTimer = 0f;
            approaching = false;
        }
    }
}
