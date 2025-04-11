using UnityEngine;

public class TriggerFinalDialogueOnly : MonoBehaviour
{
    public DialogueController dialogueController;

    [Header("Final Dialogue Only")]
    [TextArea] public string finalDialogue;

    private bool triggered = false;

    void OnEnable()
    {
        if (!triggered)
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogueManually()
    {
        if (!triggered)
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        triggered = true;

        if (dialogueController != null && !string.IsNullOrEmpty(finalDialogue))
        {
            dialogueController.ShowDialogue(finalDialogue);
            Debug.Log("🗨️ Final dialogue triggered by TriggerFinalDialogueOnly.");
        }
        else
        {
            Debug.LogWarning("❗ DialogueController or finalDialogue missing.");
        }
    }
}
