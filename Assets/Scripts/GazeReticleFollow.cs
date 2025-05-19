using UnityEngine;

public class GazeReticleFollow : MonoBehaviour
{
    public Transform cameraTransform;
    public float distance = 2.0f;

    void Update()
    {
        transform.position = cameraTransform.position + cameraTransform.forward * distance;
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
