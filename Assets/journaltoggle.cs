using UnityEngine;

public class JournalToggle1 : MonoBehaviour
{
    public GameObject journalCanvas;                    // Drag your journal UI canvas here
    public DialogueController dialogueController;       // Drag your dialogue controller here
    public MonoBehaviour movementScript;                // Drag your movement script here (ClickToLookWalkerWithMinimap)

    private bool isOpen = false;

    void Update()
    {
        // Automatically close journal during dialogue
        if (dialogueController != null && !dialogueController.IsReady())
        {
            if (isOpen)
            {
                ToggleJournal(false);
                Debug.Log("📖 Journal closed due to dialogue.");
            }
            return;
        }

        // Toggle with J
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal(!isOpen);
        }
    }

    void ToggleJournal(bool open)
    {
        isOpen = open;
        journalCanvas.SetActive(isOpen);

        if (movementScript != null)
        {
            movementScript.enabled = !isOpen; // Fully disable movement script
        }

        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;

        Debug.Log("📖 Journal " + (isOpen ? "opened" : "closed"));
    }
}
