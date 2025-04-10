using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerNextNPCSequence : MonoBehaviour
{
    public DialogueController dialogueController;

    [Header("Dialogue After NPC Gift")]
    [TextArea] public string afterGiftDialogue;
    [TextArea] public string finalDialogue;

    [Header("Button Choice")]
    public GameObject button1;
    public GameObject button2;

    [Tooltip("Only this button is required to continue.")]
    public GameObject requiredButton;

    [Header("Optional Triggers")]
    public GameObject secondNPCTriggerZone;
    public GameObject nextPhotoTrigger;
    public GameObject finalCollider;

    public bool isFinalObject = false;

    private bool started = false;
    private bool journalOpened = false;
    private bool correctButtonClicked = false;

    void OnEnable()
    {
        StartCoroutine(StartSequence());
    }

    void Update()
    {
        if (!journalOpened && Input.GetKeyDown(KeyCode.J))
        {
            journalOpened = true;
            Debug.Log("📓 Journal opened.");
        }
    }

    public IEnumerator StartSequence()
    {
        if (started) yield break;
        started = true;

        Debug.Log($"{gameObject.name}: StartSequence coroutine called.");

        // Wait one frame to make sure all UI is active and stable
        yield return null;

        if (dialogueController != null && !string.IsNullOrEmpty(afterGiftDialogue))
        {
            dialogueController.ShowDialogue(afterGiftDialogue, ShowButtons);
        }

        if (secondNPCTriggerZone != null)
            secondNPCTriggerZone.SetActive(true);

        if (nextPhotoTrigger != null)
            nextPhotoTrigger.SetActive(true);
    }

    void ShowButtons()
    {
        button1.SetActive(true);
        button2.SetActive(true);

        Button btn1 = button1.GetComponent<Button>();
        Button btn2 = button2.GetComponent<Button>();

        if (btn1 != null)
        {
            btn1.onClick.RemoveAllListeners();
            btn1.onClick.AddListener(() => OnButtonClicked(button1));
        }

        if (btn2 != null)
        {
            btn2.onClick.RemoveAllListeners();
            btn2.onClick.AddListener(() => OnButtonClicked(button2));
        }
    }

    void OnButtonClicked(GameObject clickedButton)
    {
        if (correctButtonClicked) return;

        if (clickedButton == requiredButton)
        {
            if (!journalOpened)
            {
                Debug.Log("⛔ Please press J to open your journal first.");
                return;
            }

            correctButtonClicked = true;

            button1.SetActive(false);
            button2.SetActive(false);

            Debug.Log("✅ Correct button clicked. Waiting 5 seconds...");
            StartCoroutine(DelayedFinalDialogue());
        }
        else
        {
            Debug.Log("❌ Incorrect button clicked.");
        }
    }

    IEnumerator DelayedFinalDialogue()
    {
        yield return new WaitForSeconds(5f);

        if (dialogueController != null && !string.IsNullOrEmpty(finalDialogue))
        {
            dialogueController.ShowDialogue(finalDialogue, ActivateFinalCollider);
        }
    }

    void ActivateFinalCollider()
    {
        if (finalCollider != null)
        {
            finalCollider.SetActive(true);
            Debug.Log("🟩 Final collider activated after final dialogue.");
        }
    }
}
