using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 30f;         // Total time in seconds
    public TextMeshProUGUI timerText;     // UI text to display time
    private bool gameOver = false;
    public GameOverController gameOverUI;  

    void Update()
    {
        if (gameOver) return;

        // Decrease timer
        timeLimit -= Time.deltaTime;

        // Update UI
        if (timerText != null)
            timerText.text = "Time: " + Mathf.Ceil(timeLimit);

        // Check if time is up
        if (timeLimit <= 0f)
        {
            timeLimit = 0f;
            gameOver = true;
            CheckGameOver();
        }
    }
void CheckGameOver()
{
    if (GameObject.FindGameObjectsWithTag("Virus").Length > 0)
    {
        Debug.Log("Time's up! Game Over!");
        if(gameOverUI != null)
            gameOverUI.ShowGameOver();
    }
    else
    {
        Debug.Log("All viruses destroyed before time! You win!");
    }
}
}
