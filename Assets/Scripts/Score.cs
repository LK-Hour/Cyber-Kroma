using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static int score = 0;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = "Score: " + score;
    }
}
