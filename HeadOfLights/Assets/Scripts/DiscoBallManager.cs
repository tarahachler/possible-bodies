using UnityEngine;

public class DiscoBallManager : MonoBehaviour
{
    // --- Variables globales ---
    public static bool userSawDiscoBall = false;
    public static bool isInsideDiscoBall = false;
    public static bool makeDiscoBallTurn = false;
    public static bool gameEnded = false;

    // --- Configuration ---
    public Transform discoBall;
    public Transform userCamera;
    public float requiredLookTime = 1f;
    public Vector3 initialDiscoBallPosition = new Vector3(0f, 2f, 4f);
    public float lookAngleThreshold = 15f; // angle max pour considérer que l'utilisateur regarde la boule

    private float lookTimer = 0f;
    private bool hasReturned = false;

    void Update()
    {
        HandleUserLookingAtDiscoBall();
        HandleReturnToInitialPosition();
    }

    // --- 1. Regarde la boule disco pendant une seconde ---
    void HandleUserLookingAtDiscoBall()
    {
        if (userSawDiscoBall) return; // ne pas recalculer si déjà vu

        Vector3 toDiscoBall = (discoBall.position - userCamera.position).normalized;
        float angle = Vector3.Angle(userCamera.forward, toDiscoBall);

        if (angle < lookAngleThreshold)
        {
            lookTimer += Time.deltaTime;
            if (lookTimer >= requiredLookTime)
            {
                userSawDiscoBall = true;
            }
        }
        else
        {
            lookTimer = 0f; // reset si on regarde ailleurs
        }
    }

    // --- 2. Une seule fois quand la boule est revenue ---
    void HandleReturnToInitialPosition()
    {
        if (!makeDiscoBallTurn && !hasReturned &&
            Vector3.Distance(discoBall.position, initialDiscoBallPosition) < 0.01f)
        {
            makeDiscoBallTurn = true;
            hasReturned = true;
            Debug.Log("Disco ball est revenue à sa position d'origine. Rotation relancée.");
        }
    }

    // --- 3. À appeler pour forcer un nouvel état ---
    public static void SetIsInsideDiscoBall(bool state)
    {
        isInsideDiscoBall = state;
    }

    // --- 4. Réinitialisation de l’état (à appeler depuis RotateForSeconds par exemple) ---
    public static void ResetDiscoBallState()
    {
        userSawDiscoBall = false;
        makeDiscoBallTurn = false;
    }

    // --- 5. Pour permettre à d'autres scripts de réinitialiser 'hasReturned' si besoin ---
    public void ResetHasReturned()
    {
        hasReturned = false;
    }
}
