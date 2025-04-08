using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    [TextArea] public string dialogueLine;
    public DialogueController dialogueController;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            dialogueController.ShowDialogue(dialogueLine);
        }
    }
}
