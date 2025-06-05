using UnityEngine;

public class ParticleRotator : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSpeed = 65f;

    void Update()
    {
        transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

