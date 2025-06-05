using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StarManager : MonoBehaviour
{
    public static StarManager Instance;

    private List<Star> stars = new List<Star>();

    // ✅ Cette variable est accessible globalement
    public bool allStarsLit { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterStar(Star star)
    {
        stars.Add(star);
    }

    public void StarLit(Star star)
    {
        if (!allStarsLit && AllStarsLit())
        {
            allStarsLit = true; // ✅ C’est ici qu’on passe à true
            Debug.Log("🎉 Toutes les étoiles sont allumées !");        }
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
