using UnityEngine;

public class AttachBodyToCamera : MonoBehaviour
{
    public Transform centerEye; // Main Camera ou XR Camera
    public Vector3 offset = new Vector3(0, 0, 1f); // Position relative à la caméra (avant, haut, droite)

    void LateUpdate()
    {
        if (centerEye != null)
        {
            // Extraire la rotation horizontale uniquement (yaw)
            Vector3 forward = centerEye.forward;
            forward.y = 0; // Ignore la composante verticale
            forward.Normalize();

            // Créer une rotation qui regarde vers l'avant horizontal de la caméra
            Quaternion flatRotation = Quaternion.LookRotation(forward, Vector3.up);

            // Appliquer position avec rotation "plate"
            Vector3 worldOffset = flatRotation * offset;
            transform.position = centerEye.position + worldOffset;

            // Appliquer la rotation horizontale seulement
            transform.rotation = flatRotation;
        }
    }
}
