using UnityEngine;

public class JournalToggle1 : MonoBehaviour
{
    public GameObject journalCanvas;
    public DialogueController dialogueController;
    public MonoBehaviour movementScript;

    private bool isOpen = false;
    private bool waitingToClose = false;

    void Update()
    {
        if (dialogueController != null && !dialogueController.IsReady())
        {
            if (isOpen && !waitingToClose)
            {
                waitingToClose = true;
                StartCoroutine(CloseJournalDelayed(0.1f));
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal(!isOpen);
        }
    }

    System.Collections.IEnumerator CloseJournalDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleJournal(false);
        waitingToClose = false;
        Debug.Log("📖 Journal closed due to dialogue.");
    }

    public void ForceCloseJournal()
    {
        if (isOpen)
        {
            ToggleJournal(false);
            Debug.Log("📖 Journal auto-closed by script.");
        }
    }


    void ToggleJournal(bool open)
    {
        isOpen = open;
        journalCanvas.SetActive(isOpen);

        if (movementScript != null)
        {
            movementScript.enabled = !isOpen;
        }

        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;

        Debug.Log("📖 Journal " + (isOpen ? "opened" : "closed"));
    }

    public bool IsJournalOpen => isOpen;
}
