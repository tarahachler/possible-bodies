using UnityEngine;

public class ToggleVisibilityOnCollision : MonoBehaviour
{
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.enabled = false; // commence invisible
        }
    }

    void LateUpdate()
    {
        if (DiscoBallManager.isInsideDiscoBall == true && rend != null)
        {
            rend.enabled = true; // visible si à l'intérieur de la disco ball
        }
        else
        {
            rend.enabled = false; // invisible sinon
        }
    }
}
