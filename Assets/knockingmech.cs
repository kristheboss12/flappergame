using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class KnockToEnter : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject knockPromptUI;

    [Header("Settings")]
    public string sceneToLoad;

    private bool playerInside = false;
    private int knockCount = 0;
    private float lastKnockTime = 0f;
    private bool waitingForPause = false;
    private float knockResetDelay = 2f;

    [Header("Fade")]
    public Image fadeImage; // Drag your FadePanel Image here
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource knockAudioSource;
    public AudioClip knockClip;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(playerInside
                ? "👊 E pressed inside knock zone."
                : "❌ E pressed outside knock zone.");

            if (playerInside)
                RegisterKnock();
        }

        if (waitingForPause && Time.time - lastKnockTime >= knockResetDelay)
        {
            if (knockCount == 3)
            {
                Debug.Log("🚪 3 knocks registered. Opening...");
                LoadFinalScene();
            }
            else
            {
                Debug.Log("❌ Incorrect knock pattern. Resetting...");
                ResetKnocks();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            knockPromptUI?.SetActive(true);
            Debug.Log("🟢 Player entered knock zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            knockPromptUI?.SetActive(false);
            ResetKnocks();
            Debug.Log("🔴 Player exited knock zone.");
        }
    }

    void RegisterKnock()
    {
        knockCount++;
        lastKnockTime = Time.time;
        waitingForPause = true;

        if (knockAudioSource != null && knockClip != null)
        {
            knockAudioSource.PlayOneShot(knockClip);
        }

        Debug.Log($"🔊 Knock #{knockCount}");

        if (knockCount > 3)
        {
            Debug.Log("⚠️ Too many knocks! Resetting soon...");
        }
    }


    void ResetKnocks()
    {
        knockCount = 0;
        waitingForPause = false;
        Debug.Log("🔁 Knock sequence reset.");
    }

    void LoadFinalScene()
    {
        ResetKnocks();
        StartCoroutine(FadeAndLoadScene());
    }

    IEnumerator FadeAndLoadScene()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color color = fadeImage.color;

            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }

        Debug.Log($"🌒 Fade complete. Loading scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }

}
