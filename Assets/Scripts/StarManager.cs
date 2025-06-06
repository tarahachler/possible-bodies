using System.Collections;
using System.Collections.Generic;
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
        if (!star.IsLit) // Ajoute cette vérification pour éviter les doublons
        {
            starsLitCount++;
            star.IsLit = true;
            HandleAudio();
        }

        if (!allStarsLit && AllStarsLit())
        {
            allStarsLit = true;
        }
    }

    private void HandleAudio()
    {
        if (starsLitCount == 2)
        {
            // Démarre toutes les pistes, mute tout sauf la première
            for (int i = 0; i < 5; i++)
            {
                audioSources[i].Play();
                audioSources[i].mute = i != 0;
            }
        }
        else if (starsLitCount > 2)
        {
            // Calcule combien de pistes doivent être unmuted
            int totalStars = stars.Count;
            int toUnmute = 1 + Mathf.FloorToInt(((starsLitCount - 2) / (float)(totalStars - 2)) * 4);
            for (int i = 0; i < 5; i++)
            {
                audioSources[i].mute = i >= toUnmute ? true : false;
            }
        }
        else if (starsLitCount > 2 && stars.Count > 2)
        {
            int totalStars = stars.Count;
            int toUnmute = 1 + Mathf.FloorToInt(((starsLitCount - 2) / (float)(totalStars - 2)) * 4);
            for (int i = 0; i < 5; i++)
            {
                audioSources[i].mute = i >= toUnmute;
            }
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
