using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShoeprintPhoto : MonoBehaviour
{
    public GameObject photoButton;
    public GameObject whiteFlash;
    public GameObject photoImage;
    public Camera playerCamera;
    public GameObject permanentObjectToActivate;
    public DialogueController dialogueController;
    public TeleportWithFade teleportScript;
    [TextArea] public string followupDialogue;
    public IntroDialogueManager introDialogueManager;

    [Header("Audio")]
    public AudioSource photoAudioSource;
    public AudioClip flashSound;
    public AudioClip followupVoiceClip; // 🎤 NEW: Optional voice clip for followup dialogue

    private bool playerIsNear = false;
    private bool hasTakenPhoto = false;
    private Collider myCollider;

    void Start()
    {
        photoButton.SetActive(false);
        whiteFlash.SetActive(false);
        photoImage.SetActive(false);
        myCollider = GetComponent<Collider>();

        // Ensure teleportation is initially disabled
        if (teleportScript != null)
        {
            teleportScript.enabled = false;
        }
    }

    void Update()
    {
        if (playerIsNear && !hasTakenPhoto && IsLookingDown())
        {
            photoButton.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                OnTakePhoto();
            }
        }
        else if (!hasTakenPhoto)
        {
            photoButton.SetActive(false);
        }
    }

    bool IsLookingDown()
    {
        float cameraX = playerCamera.transform.localEulerAngles.x;
        if (cameraX > 180) cameraX -= 360;
        return cameraX > 30f;
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
            photoButton.SetActive(false);
        }
    }

    void OnTakePhoto()
    {
        if (hasTakenPhoto) return;

        hasTakenPhoto = true;
        photoButton.SetActive(false);

        // Skip initial dialogues
        if (introDialogueManager != null)
        {
            introDialogueManager.SkipInitialDialogues();
        }

        StartCoroutine(PhotoSequence());
    }

    IEnumerator PhotoSequence()
    {
        if (whiteFlash != null)
        {
            if (photoAudioSource != null && flashSound != null)
            {
                photoAudioSource.PlayOneShot(flashSound);
            }

            Image flashImage = whiteFlash.GetComponent<Image>();
            if (flashImage != null)
            {
                whiteFlash.SetActive(true);
                flashImage.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        if (photoImage != null)
        {
            photoImage.SetActive(true);
        }

        float fadeDuration = 0.8f;
        float t = 0f;

        if (whiteFlash != null)
        {
            Image flashImage = whiteFlash.GetComponent<Image>();
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

        yield return new WaitForSeconds(3f);

        if (photoImage != null)
        {
            photoImage.SetActive(false);
        }

        if (permanentObjectToActivate != null)
        {
            permanentObjectToActivate.SetActive(true);
            Debug.Log("🧩 Permanent object activated after photo.");
        }

        if (dialogueController != null && !string.IsNullOrEmpty(followupDialogue))
        {
            bool complete = false;
            dialogueController.ShowDialogue(followupDialogue, followupVoiceClip, () => complete = true);
            yield return new WaitUntil(() => complete && dialogueController.IsReady());
        }


        if (teleportScript != null)
        {
            teleportScript.enabled = true;
            teleportScript.photoEventCompleted = true;
            Debug.Log("📸 Teleportation enabled after photo event.");
        }

        Cleanup();
    }

    void Cleanup()
    {
        if (myCollider != null)
            myCollider.enabled = false;

        this.enabled = false;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShoeprintPhoto : MonoBehaviour
{
    public GameObject photoButton;
    public GameObject whiteFlash;
    public GameObject photoImage;
    public Camera playerCamera;
    public GameObject permanentObjectToActivate;
    public DialogueController dialogueController;
    public TeleportWithFade teleportScript;
    [TextArea] public string followupDialogue;
    public IntroDialogueManager introDialogueManager;

    [Header("Audio")]
    public AudioSource photoAudioSource;
    public AudioClip flashSound;

    private bool playerIsNear = false;
    private bool hasTakenPhoto = false;
    private Collider myCollider;

    void Start()
    {
        photoButton.SetActive(false);
        whiteFlash.SetActive(false);
        photoImage.SetActive(false);
        myCollider = GetComponent<Collider>();

        // Ensure teleportation is initially disabled
        if (teleportScript != null)
        {
            teleportScript.enabled = false;
        }
    }

    void Update()
    {
        if (playerIsNear && !hasTakenPhoto && IsLookingDown())
        {
            photoButton.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                OnTakePhoto();
            }
        }
        else if (!hasTakenPhoto)
        {
            photoButton.SetActive(false);
        }
    }

    bool IsLookingDown()
    {
        float cameraX = playerCamera.transform.localEulerAngles.x;
        if (cameraX > 180) cameraX -= 360;
        return cameraX > 30f;
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
            photoButton.SetActive(false);
        }
    }

    void OnTakePhoto()
    {
        if (hasTakenPhoto) return;

        hasTakenPhoto = true;
        photoButton.SetActive(false);

        // Skip initial dialogues
        if (introDialogueManager != null)
        {
            introDialogueManager.SkipInitialDialogues();
        }

        StartCoroutine(PhotoSequence());
    }

    IEnumerator PhotoSequence()
    {
        if (whiteFlash != null)
        {
            // 🎵 Play flash sound
            if (photoAudioSource != null && flashSound != null)
            {
                photoAudioSource.PlayOneShot(flashSound);
            }

            Image flashImage = whiteFlash.GetComponent<Image>();
            if (flashImage != null)
            {
                whiteFlash.SetActive(true);
                flashImage.color = new Color(1f, 1f, 1f, 1f);
            }
        }


        if (photoImage != null)
        {
            photoImage.SetActive(true);
        }

        float fadeDuration = 0.8f;
        float t = 0f;

        if (whiteFlash != null)
        {
            Image flashImage = whiteFlash.GetComponent<Image>();
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

        yield return new WaitForSeconds(3f);

        if (photoImage != null)
        {
            photoImage.SetActive(false);
        }

        if (permanentObjectToActivate != null)
        {
            permanentObjectToActivate.SetActive(true);
            Debug.Log("🧩 Permanent object activated after photo.");
        }

        if (dialogueController != null && !string.IsNullOrEmpty(followupDialogue))
        {
            dialogueController.ShowDialogue(followupDialogue);
        }

        if (teleportScript != null)
        {
            teleportScript.enabled = true;
            teleportScript.photoEventCompleted = true;
            Debug.Log("📸 Teleportation enabled after photo event.");
        }

        Cleanup();
    }

    void Cleanup()
    {
        if (myCollider != null)
            myCollider.enabled = false;

        this.enabled = false;
    }
}
