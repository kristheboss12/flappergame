using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeleportWithFade : MonoBehaviour
{
    [Header("Teleport Zones")]
    public Collider zoneACollider;
    public Collider zoneBCollider;

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

    void Start()
    {
        if (teleportButton != null)
            teleportButton.SetActive(false);
    }

    void Update()
    {
        if (playerInZone && canTeleport && !isFading)
        {
            if (Input.GetKeyDown(KeyCode.E)) // ⭐ Press E to teleport
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
        playerInZone = true; // ⭐ enable input
        if (teleportButton != null)
            teleportButton.SetActive(true);
    }

    void HideTeleportPrompt()
    {
        playerInZone = false; // ⭐ disable input
        if (teleportButton != null)
            teleportButton.SetActive(false);
    }

    IEnumerator FadeAndTeleport(Vector3 destination, bool enableUIQuad)
    {
        isFading = true;
        canTeleport = false;

        if (movementScript != null)
            movementScript.enabled = false;

        yield return StartCoroutine(Fade(0f, 1f));

        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        transform.position = destination;

        if (rotateOnTeleport)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 180f, 0f);
        }

        if (controller != null) controller.enabled = true;

        if (UIQuad != null)
            UIQuad.SetActive(enableUIQuad);

        yield return new WaitForSeconds(teleportCooldown);

        yield return StartCoroutine(Fade(1f, 0f));

        if (movementScript != null)
            movementScript.enabled = true;

        canTeleport = true;
        isFading = false;
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
