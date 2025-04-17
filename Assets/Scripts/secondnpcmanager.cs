using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SecondNPCDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueController dialogueController;
    [TextArea] public string[] dialogueLines;
    public AudioClip[] voiceClips;

    [Header("Photo Flash")]
    public GameObject whiteFlash;
    public GameObject photoImage;
    public AudioSource photoAudioSource;
    public AudioClip flashSound;

    [Header("After Dialogue")]
    public GameObject permanentObjectToActivate;

    private bool triggered = false;

    void Start()
    {
        if (whiteFlash != null) whiteFlash.SetActive(false);
        if (photoImage != null) photoImage.SetActive(false);
        if (permanentObjectToActivate != null) permanentObjectToActivate.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(PlayDialogueSequence());
    }

    IEnumerator PlayDialogueSequence()
    {
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            string line = dialogueLines[i];
            AudioClip voice = (i < voiceClips.Length) ? voiceClips[i] : null;

            bool dialogueCompleted = false;
            dialogueController.ShowDialogue(line, voice, () => dialogueCompleted = true);
            yield return new WaitUntil(() => dialogueCompleted && dialogueController.IsReady());
        }

        yield return StartCoroutine(FlashAndShowImage());
    }

    IEnumerator FlashAndShowImage()
    {
        // Flash and sound
        if (whiteFlash != null)
        {
            if (photoAudioSource != null && flashSound != null)
                photoAudioSource.PlayOneShot(flashSound);

            whiteFlash.SetActive(true);
            Image flashImage = whiteFlash.GetComponent<Image>();
            if (flashImage != null)
            {
                flashImage.color = Color.white;
                yield return StartCoroutine(FadeOutImage(flashImage, 0.8f));
            }
        }

        if (photoImage != null)
            photoImage.SetActive(true);

        yield return new WaitForSeconds(3f);

        if (photoImage != null)
            photoImage.SetActive(false);

        if (permanentObjectToActivate != null)
        {
            permanentObjectToActivate.SetActive(true);
            Debug.Log("📸 Permanent object activated after photo.");

            var nextSequence = permanentObjectToActivate.GetComponent<TriggerNextNPCSequence>();
            if (nextSequence != null)
                StartCoroutine(nextSequence.StartSequence());

            var finalDialogueOnly = permanentObjectToActivate.GetComponent<TriggerFinalDialogueOnly>();
            if (finalDialogueOnly != null)
                finalDialogueOnly.TriggerDialogueManually();

            if (nextSequence == null && finalDialogueOnly == null)
                Debug.LogWarning("⚠️ No trigger script found on the activated permanent object.");
        }
    }

    IEnumerator FadeOutImage(Image image, float duration)
    {
        float elapsed = 0f;
        Color original = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            image.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }

        image.color = new Color(original.r, original.g, original.b, 0f);
        image.gameObject.SetActive(false);
    }
}
