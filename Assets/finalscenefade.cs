using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneEndSequence : MonoBehaviour
{
    [Header("UI References")]
    public Image imageToShow;       // Assign in inspector (the image that appears)
    public Image fadeImage;         // Assign in inspector (black fullscreen image)

    [Header("Settings")]
    public float imageDisplayTime = 5f;
    public float fadeDuration = 1.5f;
    public string sceneToLoad = "MainScene"; // Replace with your scene name

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Ensure image and fade are initially invisible
        imageToShow.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0); // Transparent black
        fadeImage.gameObject.SetActive(true);

        // Show image for a few seconds
        yield return new WaitForSeconds(imageDisplayTime);

        // Fade to black
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = Color.black;

        // Load the next scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
