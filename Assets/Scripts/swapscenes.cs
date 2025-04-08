using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject startImage;
    public GameObject startButton;
    public GameObject cutsceneImage;

    [Header("Fade")]
    public Image fadeImage; // assign your black fullscreen image here
    public float fadeDuration = 1f;

    [Header("Cutscene Settings")]
    public float cutsceneDuration = 3f;
    public string mainSceneName = "MainScene"; // Set this in the Inspector

    public void OnStartButtonClicked()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // Fade to black before switching from start to cutscene
        yield return StartCoroutine(Fade(0f, 1f));

        // Hide start elements
        startImage.SetActive(false);
        startButton.SetActive(false);

        // Show cutscene image
        cutsceneImage.SetActive(true);

        // Fade back in with cutscene image visible
        yield return StartCoroutine(Fade(1f, 0f));

        // Wait for the cutscene to display
        yield return new WaitForSeconds(cutsceneDuration);

        // Fade back to black before loading scene
        yield return StartCoroutine(Fade(0f, 1f));

        // Load the main game scene
        SceneManager.LoadScene(mainSceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
