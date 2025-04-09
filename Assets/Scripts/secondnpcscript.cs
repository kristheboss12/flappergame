using UnityEngine;

public class TriggerNextNPCSequence : MonoBehaviour
{
    public DialogueController dialogueController;
    [TextArea] public string initialDialogue;

    public GameObject secondNPCTriggerZone; // The second NPC’s box collider GameObject
    public GameObject nextPhotoTrigger;     // Optional: a second photo trigger if needed

    private bool started = false;

    void OnEnable()
    {
        StartSequence();
    }

    public void StartSequence()
    {
        if (started) return;
        started = true;

        Debug.Log("StartSequence method called."); // Debug line

        if (dialogueController != null && !string.IsNullOrEmpty(initialDialogue))
        {
            dialogueController.ShowDialogue(initialDialogue);
        }

        if (secondNPCTriggerZone != null)
        {
            secondNPCTriggerZone.SetActive(true);
        }

        if (nextPhotoTrigger != null)
        {
            nextPhotoTrigger.SetActive(true);
        }

        Debug.Log("🎬 Second NPC sequence started.");
    }

}
