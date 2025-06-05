using UnityEngine;

public class LoopThenStop : MonoBehaviour
{
    public Animator animator;
    public string animationStateName = "NomDeTonClip"; // nom du state dans Animator
    public float loopDuration = 5f;

    private float timer = 0f;
    private bool stopTriggered = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        animator.Play(animationStateName);
    }

    void Update()
    {
        if (!stopTriggered)
        {
            timer += Time.deltaTime;
            if (timer >= loopDuration)
            {
                animator.speed = 0f; // stoppe l'animation à son état actuel
                stopTriggered = true;
            }
        }
    }
}
