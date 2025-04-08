using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneIntroFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        // Start fully black
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 1f);
            StartCoroutine(FadeFromBlack());
        }
    }

    IEnumerator FadeFromBlack()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}
