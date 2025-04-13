using UnityEngine;
using UnityEngine.Video;

public class ForcePlayVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            Debug.Log("🎥 Video play triggered.");
        }
    }
}
