using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour
{
    public Image backgroundImage;
    public TextMeshProUGUI dialogueText;
    public float typeSpeed = 0.05f;
    public float holdTime = 2f;
    public float fadeTime = 0.5f;
    [Range(0f, 1f)] public float imageMaxAlpha = 0.5f;

    private Coroutine currentDialogue;
    private Queue<(string, Action)> dialogueQueue = new Queue<(string, Action)>();
    private bool isPlaying = false;

    public void ShowDialogue(string line, Action onComplete = null)
    {
        dialogueQueue.Enqueue((line, onComplete));

        if (currentDialogue == null)
        {
            currentDialogue = StartCoroutine(PlayQueue());
        }
    }

    public bool IsReady()
    {
        return currentDialogue == null;
    }

    IEnumerator PlayQueue()
    {
        isPlaying = true;

        while (dialogueQueue.Count > 0)
        {
            var (line, callback) = dialogueQueue.Dequeue();
            yield return StartCoroutine(PlayDialogue(line, callback));
        }

        isPlaying = false;
        currentDialogue = null;
    }

    IEnumerator PlayDialogue(string line, Action onComplete)
    {
        backgroundImage.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        Color textColor = dialogueText.color;
        Color imageColor = backgroundImage.color;

        textColor.a = 0;
        imageColor.a = 0;

        dialogueText.color = textColor;
        backgroundImage.color = imageColor;

        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        // Fade in
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = t / fadeTime;

            textColor.a = Mathf.Lerp(0, 1f, alpha);
            imageColor.a = Mathf.Lerp(0, imageMaxAlpha, alpha);

            dialogueText.color = textColor;
            backgroundImage.color = imageColor;

            yield return null;
        }

        // Typewriter
        for (int i = 0; i <= line.Length; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        // Fade out
        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = 1 - (t / fadeTime);

            textColor.a = Mathf.Lerp(0, 1f, alpha);
            imageColor.a = Mathf.Lerp(0, imageMaxAlpha, alpha);

            dialogueText.color = textColor;
            backgroundImage.color = imageColor;

            yield return null;
        }

        backgroundImage.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);

        onComplete?.Invoke();
    }
}
