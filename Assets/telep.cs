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
    public GameObject UIQuad; // Assign the UI quad
    public bool showUIQuadWhenGoingToZoneA = true; // toggle based on direction
    public MonoBehaviour movementScript; // Your movement script (optional)

    private bool canTeleport = true;
    private bool isFading = false;

    void OnTriggerEnter(Collider other)
    {
        if (!canTeleport || isFading) return;

        if (other == zoneACollider)
        {
            Debug.Log("Teleporting to Zone B...");
            StartCoroutine(FadeAndTeleport(destinationFromZoneA, false)); // Going TO Zone B
        }
        else if (other == zoneBCollider)
        {
            Debug.Log("Teleporting to Zone A...");
            StartCoroutine(FadeAndTeleport(destinationFromZoneB, true)); // Going TO Zone A
        }
    }

    IEnumerator FadeAndTeleport(Vector3 destination, bool enableUIQuad)
    {
        isFading = true;
        canTeleport = false;

        // Disable movement if needed
        if (movementScript != null)
            movementScript.enabled = false;

        // Fade out
        yield return StartCoroutine(Fade(0f, 1f));

        // Teleport and rotate
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        transform.position = destination;

        if (rotateOnTeleport)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 180f, 0f);
        }

        if (controller != null) controller.enabled = true;

        // Update UI Quad
        if (UIQuad != null)
            UIQuad.SetActive(enableUIQuad);

        // Wait a moment before fading back in
        yield return new WaitForSeconds(teleportCooldown);

        // Fade in
        yield return StartCoroutine(Fade(1f, 0f));

        // Re-enable movement
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
