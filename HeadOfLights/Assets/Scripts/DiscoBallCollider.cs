using UnityEngine;

public class DiscoBallCollider : MonoBehaviour
{
    public AudioClip enterClip;
    public AudioClip exitClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            DiscoBallManager.SetIsInsideDiscoBall(true);
            PlaySound(enterClip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            DiscoBallManager.SetIsInsideDiscoBall(false);
            PlaySound(exitClip);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
