using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float yOffset = 180;

    private void OnWillRenderObject()
    {
        Vector3 targetPos = Camera.main.transform.position;
        Vector3 lookDir = (targetPos - transform.position);
        Quaternion lookR = Quaternion.LookRotation(lookDir, Vector3.up);
        transform.rotation = lookR * Quaternion.Euler(90, yOffset, 0);
    }
}