using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    [Header("Player & Camera")]
    public Camera cam;
    public KeyCode shootKey = KeyCode.Space;

    [Header("Detection")]
    public float detectDistance = 10f;

    [Header("Aim Dot")]
    public Image aimDot;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.red;

    [Header("Score")]
    public Score scoreManager;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private bool gameEnded = false;

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");

        GameObject closestVirus = null;
        float minScreenDistance = Mathf.Infinity;

        foreach (GameObject virus in viruses)
        {
            float worldDistance = Vector3.Distance(cam.transform.position, virus.transform.position);
            if (worldDistance > detectDistance) continue;

            Vector3 screenPos = cam.WorldToScreenPoint(virus.transform.position);
            if (screenPos.z <= 0) continue;

            float screenDistance = Vector2.Distance(
                new Vector2(screenPos.x, screenPos.y),
                new Vector2(screenCenter.x, screenCenter.y)
            );

            if (screenDistance < minScreenDistance)
            {
                minScreenDistance = screenDistance;
                closestVirus = virus;
            }
        }

        // Aim dot
        if (closestVirus != null)
        {
            aimDot.transform.position = cam.WorldToScreenPoint(closestVirus.transform.position);
            aimDot.color = hoverColor;
        }
        else
        {
            aimDot.transform.position = screenCenter;
            aimDot.color = normalColor;
        }

        // Shoot
        if (Input.GetKeyDown(shootKey) && closestVirus != null)
        {
            Virus virusScript = closestVirus.GetComponent<Virus>();
            if (virusScript != null)
            {
                virusScript.OnHit();
                Score.score++;

                // WIN
                if (GameObject.FindGameObjectsWithTag("Virus").Length <= 1)
                {
                    ShowWin();
                }
            }
        }
    }

    void ShowWin()
    {
        gameEnded = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f; // pause game
    }

    public void ShowLose()
    {
        gameEnded = true;
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
