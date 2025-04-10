using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class KnockToEnter : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject knockPromptUI; // Assign your TMP UI (e.g., "Press E to Knock")

    [Header("Player")]
    public Transform player; // Assign the player Transform in the Inspector

    [Header("Settings")]
    public float triggerRadius = 2f; // Adjust based on how close they need to be
    public string sceneToLoad;

    private bool playerInside = false;
    private int knockCount = 0;
    private float lastKnockTime = 0f;
    private bool waitingForPause = false;
    private float knockResetDelay = 2f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool isInsideNow = distance <= triggerRadius;

        // Handle entering and exiting
        if (isInsideNow && !playerInside)
        {
            playerInside = true;
            if (knockPromptUI != null)
                knockPromptUI.SetActive(true);
        }
        else if (!isInsideNow && playerInside)
        {
            playerInside = false;
            if (knockPromptUI != null)
                knockPromptUI.SetActive(false);
            ResetKnocks();
        }

        // Knock input
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            RegisterKnock();
        }

        // Knock resolution check
        if (waitingForPause && Time.time - lastKnockTime >= knockResetDelay)
        {
            if (knockCount == 3)
            {
                Debug.Log("🚪 3 knocks registered. Opening...");
                LoadFinalScene();
            }
            else
            {
                Debug.Log("❌ Incorrect knock pattern. Resetting...");
                ResetKnocks();
            }
        }
    }

    void RegisterKnock()
    {
        knockCount++;
        lastKnockTime = Time.time;
        waitingForPause = true;

        Debug.Log($"🔊 Knock #{knockCount}");

        if (knockCount > 3)
        {
            Debug.Log("⚠️ Too many knocks! Resetting soon...");
        }
    }

    void ResetKnocks()
    {
        knockCount = 0;
        waitingForPause = false;
    }

    void LoadFinalScene()
    {
        ResetKnocks();
        SceneManager.LoadScene(sceneToLoad);
    }
}
