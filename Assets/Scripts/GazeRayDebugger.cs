using UnityEngine;


public class GazeRayDebugger : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    public Color rayColor = Color.green;
    public float rayLength = 10f;

    void OnDrawGizmos()
    {
        if (rayInteractor != null && rayInteractor.enabled)
        {
            Vector3 origin = rayInteractor.rayOriginTransform.position;
            Vector3 direction = rayInteractor.rayOriginTransform.forward;
            Gizmos.color = rayColor;
            Gizmos.DrawLine(origin, origin + direction * rayLength);
        }
    }
}
