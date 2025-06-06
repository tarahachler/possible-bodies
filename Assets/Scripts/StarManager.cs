using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class StarManager : MonoBehaviour
{
    public static StarManager Instance;

    private List<Star> stars = new List<Star>();

    // ✅ Cette variable est accessible globalement
    public bool allStarsLit { get; private set; } = false;

    public AudioClip[] audioClips = new AudioClip[5];
    private AudioSource[] audioSources = new AudioSource[5];
    private int starsLitCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Crée 5 AudioSources et assigne les clips
        for (int i = 0; i < 5; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = audioClips[i];
            source.loop = true;
            source.playOnAwake = false;
            source.mute = true;
            audioSources[i] = source;
        }
    }

    public void RegisterStar(Star star)
    {
        stars.Add(star);
    }

    public void StarLit(Star star)
    {
        starsLitCount++;

        HandleAudio();

        if (!allStarsLit && AllStarsLit())
        {
            allStarsLit = true;
        }
    }

    private void HandleAudio()
    {
        float fPlaying = (starsLitCount - 2) * (audioClips.Count() - 1.0f) / (stars.Count - 2) + 1;
        int nPlaying = Mathf.FloorToInt(fPlaying);
        Debug.Log($"fPlaying: {fPlaying} , nPlaying: {nPlaying}, Stars Lit: {starsLitCount}, Total Stars: {stars.Count}");
        
        for (int i = 0; i < audioClips.Count(); i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].Play();
            }
            audioSources[i].mute = i >= nPlaying;
        }       
    }

    private bool AllStarsLit()
    {
        foreach (var star in stars)
        {
            if (!star.IsLit)
                return false;
        }
        return true;
    }
}
