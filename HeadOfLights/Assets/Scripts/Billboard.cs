using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float rotation = 90;

    private void OnWillRenderObject()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.Rotate(rotation, 0, 0);
    }
}