using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogueSequence : MonoBehaviour
{
    public DialogueController dialogueController;
    [TextArea] public string[] dialogueLines;
    public GameObject nextPhotoTrigger; // Assigned photo trigger that activates after dialogue

    private bool triggered = false;
    private BoxCollider photoTriggerCollider;

    void Start()
    {
        if (nextPhotoTrigger != null)
        {
            photoTriggerCollider = nextPhotoTrigger.GetComponent<BoxCollider>();
            if (photoTriggerCollider != null)
            {
                photoTriggerCollider.enabled = false; // 🔒 Disable the collider at the start
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(StartDialogueSequence());
    }

    IEnumerator StartDialogueSequence()
    {
        foreach (string line in dialogueLines)
        {
            dialogueController.ShowDialogue(line);
            yield return new WaitUntil(() => dialogueController.IsReady());
        }

        if (photoTriggerCollider != null)
        {
            photoTriggerCollider.enabled = true; // ✅ Enable collider after dialogue
            Debug.Log("✅ BoxCollider on nextPhotoTrigger is now enabled.");
        }
        else
        {
            Debug.LogWarning("⚠️ BoxCollider not found on nextPhotoTrigger.");
        }

        // Optionally enable script too if needed
        MonoBehaviour photoScript = nextPhotoTrigger.GetComponent<SimplePhotoCapture>();
        if (photoScript != null)
        {
            photoScript.enabled = true;
        }
    }
}
