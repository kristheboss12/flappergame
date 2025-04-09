using UnityEngine;

public class JournalToggle : MonoBehaviour
{
    public GameObject journalCanvas;                  // Drag your journal UI here
    public DialogueController dialogueController;     // Reference to your dialogue system

    private bool isOpen = false;

    void Update()
    {
        // If dialogue is active, close journal and block toggling
        if (dialogueController != null && !dialogueController.IsReady())
        {
            if (isOpen)
            {
                journalCanvas.SetActive(false);
                isOpen = false;
                Debug.Log("📖 Journal closed because dialogue started.");
            }
            return; // Block input during dialogue
        }

        // Toggle journal with J key
        if (Input.GetKeyDown(KeyCode.J))
        {
            isOpen = !isOpen;
            journalCanvas.SetActive(isOpen);
            Debug.Log("📖 Journal " + (isOpen ? "opened" : "closed"));
        }
    }
}
