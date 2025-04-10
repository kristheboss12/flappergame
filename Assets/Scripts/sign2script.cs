using UnityEngine;
using UnityEngine.UI; // ✅ Needed for Image
using System.Collections;

public class SimplePhotoCapture : MonoBehaviour
{
    public GameObject photoButton;       // Visual "Press E" button
    public GameObject whiteFlash;        // Full-screen white image
    public GameObject photoImage;        // The photo to display
    public Camera playerCamera;          // Reference to the player’s camera

    private bool playerIsNear = false;
    private bool hasTakenPhoto = false;
    private Collider myCollider;
    public GameObject permanentObjectToActivate; // Assign this in the Inspector

    [Header("Audio")]
    public AudioSource photoAudioSource;
    public AudioClip flashSound;



    void Start()
    {
        if (photoButton != null) photoButton.SetActive(false);
        if (whiteFlash != null) whiteFlash.SetActive(false);
        if (photoImage != null) photoImage.SetActive(false);

        myCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (playerIsNear && !hasTakenPhoto && IsLookingDown())
        {
            if (photoButton != null) photoButton.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                OnTakePhoto();
            }
        }
        else if (!hasTakenPhoto)
        {
            if (photoButton != null) photoButton.SetActive(false);
        }
    }

    bool IsLookingDown()
    {
        float cameraX = playerCamera.transform.localEulerAngles.x;
        if (cameraX > 180) cameraX -= 360;

        return cameraX > 30f; // Adjust if needed
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTakenPhoto)
        {
            playerIsNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            if (photoButton != null) photoButton.SetActive(false);
        }
    }

    void OnTakePhoto()
    {
        if (hasTakenPhoto) return;

        hasTakenPhoto = true;
        if (photoButton != null) photoButton.SetActive(false);
        StartCoroutine(PhotoSequence());
    }

    public void ForceTriggerCheck()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2f);
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                playerIsNear = true;
                Debug.Log("👣 Manually triggered playerIsNear = true (player already inside trigger)");
            }
        }
    }


    void Cleanup()
    {
        if (myCollider != null)
            myCollider.enabled = false;

        this.enabled = false;
    }

    IEnumerator PhotoSequence()
    {
        Image flashImage = null;

        if (whiteFlash != null)
        {
            // 🎵 Play flash sound
            if (photoAudioSource != null && flashSound != null)
            {
                photoAudioSource.PlayOneShot(flashSound);
            }

            flashImage = whiteFlash.GetComponent<Image>();
            if (flashImage != null)
            {
                whiteFlash.SetActive(true);
                flashImage.color = new Color(1f, 1f, 1f, 1f); // Full white instantly
            }
        }


        // Show the photo at the same time as the flash
        if (photoImage != null)
        {
            photoImage.SetActive(true);
        }

        // Fade out the flash
        float fadeDuration = 0.8f;
        float t = 0f;

        if (flashImage != null)
        {
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                flashImage.color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }

            flashImage.color = new Color(1f, 1f, 1f, 0f);
            whiteFlash.SetActive(false);
        }

        // Hold the photo for 3 seconds
        yield return new WaitForSeconds(3f);

        if (photoImage != null)
        {
            photoImage.SetActive(false);
        }

        if (permanentObjectToActivate != null)
        {
            permanentObjectToActivate.SetActive(true);
            Debug.Log("🧩 Permanent object activated after photo.");

            if (permanentObjectToActivate != null)
            {
                permanentObjectToActivate.SetActive(true);
                Debug.Log("🧩 Permanent object activated after photo.");

                // Try TriggerNextNPCSequence
                var nextSequence = permanentObjectToActivate.GetComponent<TriggerNextNPCSequence>();
                if (nextSequence != null)
                {
                    StartCoroutine(nextSequence.StartSequence());
                }

                var finalDialogueOnly = permanentObjectToActivate.GetComponent<TriggerFinalDialogueOnly>();
                if (finalDialogueOnly != null)
                {
                    finalDialogueOnly.TriggerDialogueManually();
                }


                if (nextSequence == null && finalDialogueOnly == null)
                {
                    Debug.LogWarning("⚠️ No recognized trigger component found on the activated object.");
                }
            }

        }

        // Cleanup and disable this script
        Cleanup();
    }




}
