using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShoeprintPhoto : MonoBehaviour
{
    public GameObject photoButton;       // UI Button (decorative only, not clickable)
    public GameObject whiteFlash;        // UI white screen overlay
    public GameObject photoImage;        // Photo UI image
    public Camera playerCamera;

    private bool playerIsNear = false;
    private bool hasTakenPhoto = false;
    private Collider myCollider;
    public DialogueController dialogueController;  // Reference to your dialogue system
    public TeleportWithFade teleportScript; // Drag in the teleport script
    [TextArea] public string followupDialogue;     // Text to show after photo


    void Start()
    {
        photoButton.SetActive(false);
        whiteFlash.SetActive(false);
        photoImage.SetActive(false);

        myCollider = GetComponent<Collider>();
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

        return cameraX > 30f; // Adjust this if needed
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTakenPhoto)
        {
            playerIsNear = true;
            Debug.Log("Player entered trigger zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            photoButton.SetActive(false);
            Debug.Log("Player exited trigger zone.");
        }
    }

    void OnTakePhoto()
    {
        if (hasTakenPhoto) return;

        hasTakenPhoto = true;
        photoButton.SetActive(false);
        Debug.Log("Photo taken with E key.");
        StartCoroutine(PhotoSequence());
    }

    IEnumerator PhotoSequence()
    {
        whiteFlash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        whiteFlash.SetActive(false);

        photoImage.SetActive(true);
        yield return new WaitForSeconds(3f);
        photoImage.SetActive(false);

        // Trigger follow-up dialogue
        if (dialogueController != null && !string.IsNullOrEmpty(followupDialogue))
        {
            dialogueController.ShowDialogue(followupDialogue);
        }

        if (teleportScript != null)
        {
            teleportScript.photoEventCompleted = true;
        }


        Cleanup();
    }



    void Cleanup()
    {
        if (myCollider != null)
            myCollider.enabled = false;

        Debug.Log("Interaction complete. Collider disabled.");
        this.enabled = false;
    }


}
