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
            // Wait until the current line finishes before continuing
            yield return new WaitUntil(() => dialogueController.IsReady());
        }

        if (nextPhotoTrigger != null)
            nextPhotoTrigger.SetActive(true); // Enable the photo object or its collider
    }
}
