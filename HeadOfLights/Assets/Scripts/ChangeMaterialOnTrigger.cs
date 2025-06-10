using UnityEngine;
using System.Collections;

public class ChangeMaterialOnTrigger : MonoBehaviour
{
    public Material defaultMaterial;        // Matériau normal
    public Material transparentMaterial;    // Matériau transparent
    public float enterDelay = 1f;             // Délai pour entrer dans la boule
    public float exitDelay = 1f;              // Délai pour sortir de la boule

    private Renderer rend;
    private bool isCoroutineRunning = false;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Applique le matériau par défaut au départ
        if (rend != null && defaultMaterial != null)
        {
            rend.material = defaultMaterial;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCoroutineRunning && other.CompareTag("MainCamera"))
        {
            StartCoroutine(SetInsideDiscoBallAfterDelay(true));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isCoroutineRunning && other.CompareTag("MainCamera"))
        {
            StartCoroutine(SetInsideDiscoBallAfterDelay(false));
        }
    }

    IEnumerator SetInsideDiscoBallAfterDelay(bool inside)
    {
        isCoroutineRunning = true;
        float currentDelay = inside ? enterDelay : exitDelay;
        yield return new WaitForSeconds(currentDelay);
        DiscoBallManager.SetIsInsideDiscoBall(inside);

        // Change le matériau ici, après le délai
        if (rend != null)
        {
            if (inside && transparentMaterial != null)
            {
                rend.material = transparentMaterial;
            }
            else if (!inside && defaultMaterial != null)
            {
                rend.material = defaultMaterial;
            }
        }

        isCoroutineRunning = false;
    }
}
