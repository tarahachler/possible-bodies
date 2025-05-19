using UnityEngine;

public class AttachToCamera : MonoBehaviour
{
    public Transform cameraTransform;   // Assigne ici la caméra (par exemple Camera.main.transform)
    public Vector3 offset = new Vector3(0, 0, 2); // Position relative (ex: 2 unités devant la caméra)

    void Update()
    {
        if (cameraTransform != null)
        {
            // Positionne l'objet à l'offset relatif à la caméra
            transform.position = cameraTransform.position + cameraTransform.rotation * offset;

            // (optionnel) faire en sorte que l’objet regarde dans la même direction que la caméra
            transform.rotation = cameraTransform.rotation;
        }
    }
}
