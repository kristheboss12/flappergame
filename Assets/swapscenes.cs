using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public GameObject startImage;
    public GameObject startButton;
    public GameObject cutsceneImage;
    public float cutsceneDuration = 3f;
    public string mainSceneName = "MainScene"; // Set this in the Inspector

    public void OnStartButtonClicked()
    {
        // Hide button and start image
        startImage.SetActive(false);
        startButton.SetActive(false);

        // Show cutscene image
        cutsceneImage.SetActive(true);

        // Start the cutscene timer
        StartCoroutine(WaitAndLoadMainScene());
    }

    IEnumerator WaitAndLoadMainScene()
    {
        yield return new WaitForSeconds(cutsceneDuration);
        SceneManager.LoadScene(mainSceneName);
    }
}
