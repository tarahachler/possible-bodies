using UnityEngine;
using System.Collections;

public class MoveToCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform discoBallChild;

    public float delayBeforeForwardMove = 10f;
    public float delayBeforeReturnMove = 10f;

    public float moveDuration = 2f;
    public float returnDuration = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private Vector3 initialScale;
    private Vector3 enlargedScale;

    private bool forwardStarted = false;
    private bool returnStarted = false;

    private bool isForwardCoroutineRunning = false;
    private bool isReturnCoroutineRunning = false;

    private bool previousDiscoBallState = true;

    void Start()
    {
        initialPosition = transform.position;
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        targetPosition = cameraTransform.position;

        if (discoBallChild != null)
        {
            initialScale = discoBallChild.localScale;
            enlargedScale = initialScale + Vector3.one * 0.5f;
        }

        previousDiscoBallState = DiscoBallManager.makeDiscoBallTurn;
    }

    void Update()
    {
        // Détection arrêt de rotation de la boule
        if (previousDiscoBallState && !DiscoBallManager.makeDiscoBallTurn && !isForwardCoroutineRunning && !forwardStarted)
        {
            StartCoroutine(DelayedForward());
        }

        // Détection si retour déclenché par allStarsLit
        if (forwardStarted && !returnStarted && StarManager.Instance != null && StarManager.Instance.allStarsLit && !isReturnCoroutineRunning)
        {
            StartCoroutine(DelayedReturn());
        }

        previousDiscoBallState = DiscoBallManager.makeDiscoBallTurn;
    }

    IEnumerator DelayedForward()
    {
        isForwardCoroutineRunning = true;
        yield return new WaitForSeconds(delayBeforeForwardMove);
        forwardStarted = true;
        yield return MoveToTarget(targetPosition, enlargedScale, moveDuration);
        isForwardCoroutineRunning = false; // Libère le flag pour un éventuel prochain cycle
    }

    IEnumerator DelayedReturn()
    {
        isReturnCoroutineRunning = true;
        yield return new WaitForSeconds(delayBeforeReturnMove);
        returnStarted = true;
        yield return MoveToTarget(initialPosition, initialScale, returnDuration);

        // Ajout : signale la fin du jeu
        DiscoBallManager.gameEnded = true;

        isReturnCoroutineRunning = false; // Libère le flag pour un éventuel prochain cycle
    }

    IEnumerator MoveToTarget(Vector3 targetPos, Vector3 targetScale, float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 startScale = discoBallChild != null ? discoBallChild.localScale : Vector3.zero;
        float elapsed = 0f;

        float slalomAmplitude = 0.3f;
        float slalomFrequency = 2f;
        Vector3 slalomAxis = transform.right;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector3 basePos = Vector3.Lerp(startPos, targetPos, smoothT);
            float slalomOffset = Mathf.Sin(t * Mathf.PI * slalomFrequency) * slalomAmplitude * (1f - Mathf.Abs(0.5f - t) * 2f);
            Vector3 slalom = slalomAxis * slalomOffset;

            transform.position = basePos + slalom;

            if (discoBallChild != null)
            {
                discoBallChild.localScale = Vector3.Lerp(startScale, targetScale, smoothT);
            }

            yield return null;
        }

        transform.position = targetPos;
        if (discoBallChild != null)
            discoBallChild.localScale = targetScale;
    }
}
