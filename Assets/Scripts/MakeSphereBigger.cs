using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MakeSphereBigger : MonoBehaviour
{
    public float gazeDuration = 1f;
    public Vector3 targetScale = new Vector3(4f, 4f, 4f);

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private float gazeTimer = 0f;
    private bool isGazing = false;
    private bool hasGrown = false;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        interactable.hoverEntered.AddListener(OnGazeEnter);
        interactable.hoverExited.AddListener(OnGazeExit);
    }

    void OnDestroy()
    {
        interactable.hoverEntered.RemoveListener(OnGazeEnter);
        interactable.hoverExited.RemoveListener(OnGazeExit);
    }

    void OnGazeEnter(HoverEnterEventArgs args)
    {
        isGazing = true;
        gazeTimer = 0f;
    }

    void OnGazeExit(HoverExitEventArgs args)
    {
        isGazing = false;
        gazeTimer = 0f;
        hasGrown = false; // Permet de refaire grossir la sphère à chaque nouveau regard
    }

    void Update()
    {
        if (isGazing){
            ChangeToRandomColor();
        }
        
        if (isGazing && !hasGrown)
        {
            gazeTimer += Time.deltaTime;
            if (gazeTimer >= gazeDuration)
            {
                GrowSphere();
                hasGrown = true;
            }
        }
    }

    void GrowSphere()
    {
        transform.localScale = targetScale;
        Debug.Log("Gaze triggered: sphere grew.");
    }

    void ChangeToRandomColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            renderer.material.color = randomColor;
        }
    }
}
