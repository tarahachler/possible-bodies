using UnityEngine;

public class AttachToCamera : MonoBehaviour
{
    public Transform centerEye; // Main Camera ou XR camera
    public Vector3 offset = new Vector3(0, 0, 1f); // Distance devant la tête

    void LateUpdate()
    {
        if (centerEye != null)
        {
            // Position avec offset local dans l'espace de la caméra
            transform.position = centerEye.position
                + centerEye.forward * offset.z
                + centerEye.right * offset.x
                + centerEye.up * offset.y;

            transform.rotation = Quaternion.LookRotation(centerEye.forward, Vector3.up);
        }
    }
}
