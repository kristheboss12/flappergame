using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroDialogueManager : MonoBehaviour
{
    public DialogueController dialogueController;
    public GameObject shoeprintObject; // Assign your Shoeprint GameObject here

    private int completedDialogues = 0;
    private bool skipDialogues = false;

    // 🔧 UPDATED: Now accepts optional AudioClip
    public void TriggerDialogue(string line, AudioClip voiceClip = null)
    {
        if (skipDialogues)
        {
            completedDialogues++;
            StartCoroutine(CheckAfterDialogue());
        }
        else
        {
            completedDialogues++;
            dialogueController.ShowDialogue(line, voiceClip, OnDialogueComplete); // ✅ now passes AudioClip
        }
    }

    // ✅ This method matches Action type (void return)
    private void OnDialogueComplete()
    {
        StartCoroutine(CheckAfterDialogue());
    }

    public void SkipInitialDialogues()
    {
        skipDialogues = true;
        completedDialogues = 3; // Assuming there are 3 dialogues to skip
        StartCoroutine(CheckAfterDialogue());
    }

    private IEnumerator CheckAfterDialogue()
    {
        yield return new WaitUntil(() => dialogueController.IsReady());

        if (completedDialogues >= 3 && shoeprintObject != null)
        {
            shoeprintObject.SetActive(true);
            Debug.Log("✅ Shoeprint object activated after dialogues.");
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroDialogueManager : MonoBehaviour
{
    public DialogueController dialogueController;
    public GameObject shoeprintObject; // Assign your Shoeprint GameObject here

    private int completedDialogues = 0;
    private bool skipDialogues = false;

    public void TriggerDialogue(string line)
    {
        if (skipDialogues)
        {
            completedDialogues++;
            StartCoroutine(CheckAfterDialogue());
        }
        else
        {
            completedDialogues++;
            dialogueController.ShowDialogue(line, OnDialogueComplete);
        }
    }

    // ✅ This method matches Action type (void return)
    private void OnDialogueComplete()
    {
        StartCoroutine(CheckAfterDialogue());
    }

    public void SkipInitialDialogues()
    {
        skipDialogues = true;
        completedDialogues = 3; // Assuming there are 3 dialogues to skip
        StartCoroutine(CheckAfterDialogue());
    }

    private IEnumerator CheckAfterDialogue()
    {
        yield return new WaitUntil(() => dialogueController.IsReady());

        if (completedDialogues >= 3 && shoeprintObject != null)
        {
            shoeprintObject.SetActive(true);
            Debug.Log("✅ Shoeprint object activated after dialogues.");
        }
    }
}
