using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel; // Drag your Panel here
    public float exitDelay = 3f;     // Time before exiting game

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Hide at start
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true); // Show panel

        // Exit game after delay
        Invoke("ExitGame", exitDelay);
    }

    void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();

        // For testing in editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
