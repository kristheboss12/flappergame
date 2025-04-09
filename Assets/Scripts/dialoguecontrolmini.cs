using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    [TextArea] public string dialogueLine;
    public IntroDialogueManager manager;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            manager.TriggerDialogue(dialogueLine);
        }
    }
}
