using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeWithScreenCover_UIOnly : MonoBehaviour
{
    public Image screenFade;     // Fullscreen black UI Image
    public Image objectToFade;   // Your non-button asset, now a UI Image
    public Button button1;       // Start or play button
    public Button button2;       // Exit or other button

    public float screenFadeDuration = 1f;
    public float fadeDuration = 1f;
    public float delayBetweenFades = 0.5f;

    void Start()
    {
        SetImageAlpha(screenFade, 1f);
        SetImageAlpha(objectToFade, 0f);
        SetButtonAlpha(button1, 0f);
        SetButtonAlpha(button2, 0f);

        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Step 1: Fade out the full black screen
        yield return StartCoroutine(FadeOutImage(screenFade, screenFadeDuration));

        // Step 2: Fade in the title or object
        yield return StartCoroutine(FadeInImage(objectToFade));
        yield return new WaitForSeconds(delayBetweenFades);

        // Step 3: Fade in both buttons together
        StartCoroutine(FadeInButton(button1));
        yield return StartCoroutine(FadeInButton(button2));
    }

    IEnumerator FadeOutImage(Image img, float duration)
    {
        float elapsed = 0f;
        Color original = img.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            img.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }

        img.color = new Color(original.r, original.g, original.b, 0f);
        img.gameObject.SetActive(false);
    }

    IEnumerator FadeInImage(Image img)
    {
        float elapsed = 0f;
        Color original = img.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            img.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }

        img.color = new Color(original.r, original.g, original.b, 1f);
    }

    IEnumerator FadeInButton(Button btn)
    {
        Image img = btn.GetComponent<Image>();
        if (img == null) yield break;

        Color original = img.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            img.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }

        img.color = new Color(original.r, original.g, original.b, 1f);
    }

    void SetImageAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    void SetButtonAlpha(Button btn, float alpha)
    {
        var img = btn.GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }
}
