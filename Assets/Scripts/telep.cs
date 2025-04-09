using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleportWithFade : MonoBehaviour
{
    [Header("Teleport Zones")]
    public Collider zoneACollider;
    public Collider zoneBCollider;

    [Header("Progress Control")]
    public bool teleportUnlocked = false; // 🟢 Set to true after dialogue
    public bool teleportUsedOnce = false; // 🟢 Prevent future use if needed
    public DialogueController outsideDialogue; // The dialogue to trigger when arriving
    [TextArea] public string outsideDialogueLine;
    public bool photoEventCompleted = false;      // ✅ Set true when photo is done
    private bool outsideDialogueTriggered = false; // ✅ Trigger once after going outside


    [Header("Zone Text Prompts")]
    public TextMeshProUGUI zoneAText;
    public TextMeshProUGUI zoneBText;


    [Header("Destinations")]
    public Vector3 destinationFromZoneA;
    public Vector3 destinationFromZoneB;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Options")]
    public float teleportCooldown = 0.5f;
    public bool rotateOnTeleport = true;
    public GameObject UIQuad;
    public bool showUIQuadWhenGoingToZoneA = true;
    public MonoBehaviour movementScript;

    [Header("Teleport UI")]
    public GameObject teleportButton; // Acts as an on-screen prompt
    private Vector3 targetDestination;
    private bool targetUIQuadState;
    private bool playerInZone = false; // ⭐ Track if player is inside a zone

    private bool canTeleport = true;
    private bool isFading = false;

    [Header("Activation Condition")]
    public GameObject requiredObjectToBeActive; // Assign in Inspector


    void Start()
    {
        if (teleportButton != null)
            teleportButton.SetActive(false);
    }

    void Update()
    {
        if (playerInZone && canTeleport && !isFading)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(FadeAndTeleport(targetDestination, targetUIQuadState));
                HideTeleportPrompt();
            }
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (!canTeleport || isFading) return;

        if (other == zoneACollider)
        {
            targetDestination = destinationFromZoneA;
            targetUIQuadState = false;
            ShowTeleportPrompt();
        }
        else if (other == zoneBCollider)
        {
            targetDestination = destinationFromZoneB;
            targetUIQuadState = true;
            ShowTeleportPrompt();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == zoneACollider || other == zoneBCollider)
        {
            HideTeleportPrompt();
        }
    }

    void ShowTeleportPrompt()
    {
        // ✅ Check if the required object is set to active, ignoring parent states
        if (requiredObjectToBeActive != null && !requiredObjectToBeActive.activeSelf)
        {
            Debug.Log("⛔ Teleport prompt blocked — required object is not set to active.");
            return;
        }

        playerInZone = true;

        if (teleportButton != null)
            teleportButton.SetActive(true);

        if (targetDestination == destinationFromZoneA)
        {
            if (zoneAText != null) zoneAText.gameObject.SetActive(true);
            if (zoneBText != null) zoneBText.gameObject.SetActive(false);
        }
        else if (targetDestination == destinationFromZoneB)
        {
            if (zoneAText != null) zoneAText.gameObject.SetActive(false);
            if (zoneBText != null) zoneBText.gameObject.SetActive(true);
        }

        Debug.Log("✅ Teleport prompt shown.");
    }



    void HideTeleportPrompt()
    {
        playerInZone = false;

        if (teleportButton != null)
            teleportButton.SetActive(false);

        if (zoneAText != null) zoneAText.gameObject.SetActive(false);
        if (zoneBText != null) zoneBText.gameObject.SetActive(false);
    }


    IEnumerator FadeAndTeleport(Vector3 destination, bool enableUIQuad)
    {
        isFading = true;
        canTeleport = false;

        if (movementScript != null)
            movementScript.enabled = false;

        // 🔻 HIDE minimap immediately if we're teleporting to Zone B
        if (UIQuad != null && !enableUIQuad)
            UIQuad.SetActive(false);

        yield return StartCoroutine(Fade(0f, 1f));

        // Temporarily disable CharacterController to avoid conflict during teleport
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        transform.position = destination;

        if (rotateOnTeleport)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 180f, 0f);
        }

        if (controller != null) controller.enabled = true;

        yield return new WaitForSeconds(teleportCooldown);

        yield return StartCoroutine(Fade(1f, 0f));

        if (movementScript != null)
            movementScript.enabled = true;

        // 🔺 SHOW minimap after fade-in if we're teleporting to Zone A
        if (UIQuad != null && enableUIQuad)
            UIQuad.SetActive(true);

        canTeleport = true;
        isFading = false;

        if (!outsideDialogueTriggered && photoEventCompleted && enableUIQuad && outsideDialogue != null)
        {
            Debug.Log("✅ Conditions met — starting outside dialogue coroutine");
            outsideDialogueTriggered = true;
            StartCoroutine(TriggerOutsideDialogueAfterDelay());
        }
        else
        {
            Debug.Log($"❌ Outside dialogue skipped. Triggered: {outsideDialogueTriggered}, photoDone: {photoEventCompleted}, outside: {enableUIQuad}, hasController: {outsideDialogue != null}");
        }




    }


    IEnumerator TriggerOutsideDialogueAfterDelay()
    {
        yield return new WaitForSeconds(0.25f); // give the scene a moment to settle

        if (outsideDialogue == null)
        {
            Debug.LogWarning("⚠️ No outsideDialogue assigned.");
            yield break;
        }

        if (string.IsNullOrEmpty(outsideDialogueLine))
        {
            Debug.LogWarning("⚠️ outsideDialogueLine is empty.");
            yield break;
        }

        Debug.Log("🗣 Showing outside dialogue: " + outsideDialogueLine);
        outsideDialogue.ShowDialogue(outsideDialogueLine);
    }


    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        Color imageColor = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, currentAlpha);
            yield return null;
        }

        fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, endAlpha);
    }
}
