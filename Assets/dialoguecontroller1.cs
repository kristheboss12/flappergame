using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    public Image backgroundImage;             // UI image for the dialogue background
    public TextMeshProUGUI dialogueText;      // Text component
    public float typeSpeed = 0.05f;
    public float holdTime = 2f;
    public float fadeTime = 0.5f;
    [Range(0f, 1f)] public float imageMaxAlpha = 0.5f; // Max opacity for the panel background

    private Coroutine currentDialogue;

    public void ShowDialogue(string line)
    {
        if (currentDialogue != null)
        {
            StopCoroutine(currentDialogue);
        }

        currentDialogue = StartCoroutine(PlayDialogue(line));
    }

    public bool IsReady()
    {
        return currentDialogue == null;
    }


    IEnumerator PlayDialogue(string line)
    {
        backgroundImage.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        // Set initial colors with 0 alpha
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

        yield return new WaitForSeconds(holdTime);

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

        currentDialogue = null;
    }
}
