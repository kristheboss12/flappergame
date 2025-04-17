using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerNextNPCSequence : MonoBehaviour
{
    public DialogueController dialogueController;

    [Header("Dialogue After NPC Gift")]
    [TextArea] public string afterGiftDialogue;
    public AudioClip afterGiftVoice;

    [TextArea] public string finalDialogue;
    public AudioClip finalVoice;

    [Header("Button Choice")]
    public GameObject button1;
    public GameObject button2;
    public GameObject requiredButton;

    [Header("Optional Triggers")]
    public GameObject secondNPCTriggerZone;
    public GameObject nextPhotoTrigger;
    public GameObject finalCollider;

    public bool isFinalObject = false;

    private bool started = false;
    private bool correctButtonClicked = false;
    private bool waitingForDialogueTrigger = false;
    private bool dialogueTriggered = false;

    public JournalToggle1 journalToggle; // drag in Inspector

    void OnEnable()
    {
        StartCoroutine(StartSequence());
    }

    void Update()
    {
        if (correctButtonClicked && !waitingForDialogueTrigger && Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("⏩ J pressed after required button. Waiting for journal to close...");
            waitingForDialogueTrigger = true;
        }

        if (waitingForDialogueTrigger && !dialogueTriggered && journalToggle != null && !journalToggle.IsJournalOpen)
        {
            Debug.Log("✅ Journal is closed. Triggering final dialogue.");
            TriggerFinalDialogue();
        }
    }

    public IEnumerator StartSequence()
    {
        if (started) yield break;
        started = true;

        Debug.Log($"{gameObject.name}: StartSequence coroutine called.");
        yield return null;

        if (dialogueController != null && !string.IsNullOrEmpty(afterGiftDialogue))
        {
            dialogueController.ShowDialogue(afterGiftDialogue, afterGiftVoice, ShowButtons);
        }

        if (secondNPCTriggerZone != null)
            secondNPCTriggerZone.SetActive(true);

        if (nextPhotoTrigger != null)
            nextPhotoTrigger.SetActive(true);
    }

    void ShowButtons()
    {
        if (button1 != null) button1.SetActive(true);
        if (button2 != null) button2.SetActive(true);

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
            correctButtonClicked = true;
            Debug.Log("✅ Required button clicked. Closing journal after 3 seconds...");
            StartCoroutine(CloseJournalThenTriggerDialogue());
        }
    }

    IEnumerator CloseJournalThenTriggerDialogue()
    {
        yield return new WaitForSeconds(3f);

        if (journalToggle != null && journalToggle.IsJournalOpen)
        {
            journalToggle.ForceCloseJournal();
        }

        TriggerFinalDialogue();
    }

    void TriggerFinalDialogue()
    {
        dialogueTriggered = true;
        waitingForDialogueTrigger = false;

        if (dialogueController != null && !string.IsNullOrEmpty(finalDialogue))
        {
            dialogueController.ShowDialogue(finalDialogue, finalVoice, ActivateFinalCollider);
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
