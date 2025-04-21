using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerDialogueOneCorrectButton : MonoBehaviour
{
    public DialogueController dialogueController;

    [Header("Dialogues")]
    [TextArea] public string firstDialogue;
    public AudioClip firstVoice;

    [TextArea] public string secondDialogue;
    public AudioClip secondVoice;

    [Header("Buttons")]
    public GameObject button1;
    public GameObject button2;
    [Tooltip("Assign the correct button that must be clicked to continue")]
    public GameObject requiredButton;

    [Header("Next Step")]
    public GameObject colliderToActivate;
    public float delayBeforeSecondDialogue = 5f;

    private bool firstDialogueStarted = false;
    private bool journalOpened = false;
    private bool correctButtonClicked = false;

    void Update()
    {
        if (!journalOpened && Input.GetKeyDown(KeyCode.J))
        {
            journalOpened = true;
            Debug.Log("📖 Journal opened with J.");
        }
    }

    // Called externally to begin the sequence
    public void StartFirstDialogue()
    {
        if (firstDialogueStarted) return;
        firstDialogueStarted = true;

        if (dialogueController != null && !string.IsNullOrEmpty(firstDialogue))
        {
            dialogueController.ShowDialogue(firstDialogue, firstVoice, ShowButtons);
        }
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
                Debug.Log("⛔ Please open your journal (press J) before continuing.");
                return;
            }

            correctButtonClicked = true;
            button1.SetActive(false);
            button2.SetActive(false);

            Debug.Log("✅ Correct button clicked after journal. Waiting before next dialogue...");
            StartCoroutine(DelayedSecondDialogue());
        }
        else
        {
            Debug.Log("❌ Wrong button. Try the other one.");
        }
    }

    IEnumerator DelayedSecondDialogue()
    {
        yield return new WaitForSeconds(delayBeforeSecondDialogue);

        if (dialogueController != null && !string.IsNullOrEmpty(secondDialogue))
        {
            dialogueController.ShowDialogue(secondDialogue, secondVoice, ActivateNextCollider);
        }
    }

    void ActivateNextCollider()
    {
        if (colliderToActivate != null)
        {
            colliderToActivate.SetActive(true);
            Debug.Log("🟩 Final collider activated after second dialogue.");
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerDialogueOneCorrectButton : MonoBehaviour
{
    public DialogueController dialogueController;

    [TextArea] public string firstDialogue;
    [TextArea] public string secondDialogue;

    public GameObject button1;
    public GameObject button2;

    [Tooltip("Assign the correct button that must be clicked to continue")]
    public GameObject requiredButton;

    public GameObject colliderToActivate;
    public float delayBeforeSecondDialogue = 5f;

    private bool firstDialogueStarted = false;
    private bool journalOpened = false;
    private bool correctButtonClicked = false;

    void Update()
    {
        // Listen for journal input
        if (!journalOpened && Input.GetKeyDown(KeyCode.J))
        {
            journalOpened = true;
            Debug.Log("📖 Journal opened with J.");
        }
    }

    // Call this externally from another script (like TriggerNextNPCSequence)
    public void StartFirstDialogue()
    {
        if (firstDialogueStarted) return;
        firstDialogueStarted = true;

        if (dialogueController != null && !string.IsNullOrEmpty(firstDialogue))
        {
            dialogueController.ShowDialogue(firstDialogue, ShowButtons);
        }
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
                Debug.Log("⛔ Please open your journal (press J) before continuing.");
                return;
            }

            correctButtonClicked = true;
            button1.SetActive(false);
            button2.SetActive(false);

            Debug.Log("✅ Correct button clicked after journal. Waiting before next dialogue...");
            StartCoroutine(DelayedSecondDialogue());
        }
        else
        {
            Debug.Log("❌ Wrong button. Try the other one.");
        }
    }

    IEnumerator DelayedSecondDialogue()
    {
        yield return new WaitForSeconds(delayBeforeSecondDialogue);

        if (dialogueController != null && !string.IsNullOrEmpty(secondDialogue))
        {
            dialogueController.ShowDialogue(secondDialogue, ActivateNextCollider);
        }
    }

    void ActivateNextCollider()
    {
        if (colliderToActivate != null)
        {
            colliderToActivate.SetActive(true);
            Debug.Log("🟩 Final collider activated after second dialogue.");
        }
    }
}
