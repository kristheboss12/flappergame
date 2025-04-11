using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioSource audioSource;         // Drag the AudioSource here (leave Clip empty)
    public AudioClip[] playlist;            // Drag your songs here in order

    private int currentTrackIndex = 0;

    void Start()
    {
        if (playlist.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("🎧 Assign an AudioSource and at least one AudioClip.");
            return;
        }

        PlayCurrentTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying && playlist.Length > 0)
        {
            PlayNextTrack();
        }
    }

    void PlayCurrentTrack()
    {
        audioSource.clip = playlist[currentTrackIndex];
        audioSource.Play();
        Debug.Log($"🎵 Now playing: {audioSource.clip.name}");
    }

    void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
        PlayCurrentTrack();
    }
}
