using UnityEngine;
using System.Collections;

public class DialogueTriggerZone : MonoBehaviour
{
    [TextArea] public string dialogueLine;
    public AudioClip voiceLine;
    public IntroDialogueManager manager;

    [Header("Optional Delay Settings")]
    public bool useDelay = false;
    public float delaySeconds = 3f;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            if (useDelay)
            {
                StartCoroutine(TriggerDialogueWithDelay());
            }
            else
            {
                manager.TriggerDialogue(dialogueLine, voiceLine);
            }
        }
    }

    IEnumerator TriggerDialogueWithDelay()
    {
        yield return new WaitForSeconds(delaySeconds);
        manager.TriggerDialogue(dialogueLine, voiceLine);
    }
}
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
