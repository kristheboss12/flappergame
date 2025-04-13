using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    // Call this from your Button's OnClick
    public void QuitGame()
    {
        Debug.Log("Game is quitting...");

#if UNITY_EDITOR
        // Stop play mode if you're in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application if it's a build
        Application.Quit();
#endif
    }
}
