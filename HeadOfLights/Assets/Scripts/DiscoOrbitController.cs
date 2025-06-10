using UnityEngine;
using System.Collections.Generic;

public class DiscoOrbitController : MonoBehaviour
{
    public string targetTag = "DiscoOrbit";
    public Transform discoBallCenter;
    public float orbitSpeed = 20f; // degrés/seconde
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private List<Transform> orbitingObjects = new List<Transform>();
    private List<Vector3> localOffsets = new List<Vector3>();

    void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(targetTag))
            {
                orbitingObjects.Add(child);
                Vector3 relative = child.position - discoBallCenter.position;
                localOffsets.Add(relative);
            }
        }
    }

    void Update()
    {
        float time = Time.time;

        for (int i = 0; i < orbitingObjects.Count; i++)
        {
            Vector3 offset = localOffsets[i];

            // Calcule la rotation orbitale autour de Y
            Quaternion rotation = Quaternion.Euler(0f, orbitSpeed * time, 0f);
            Vector3 rotatedOffset = rotation * offset;

            // Ajoute le flottement vertical
            float floatY = Mathf.Sin(time * floatFrequency + i) * floatAmplitude;
            Vector3 finalPosition = discoBallCenter.position + rotatedOffset + Vector3.up * floatY;

            orbitingObjects[i].position = finalPosition;
        }
    }
}
