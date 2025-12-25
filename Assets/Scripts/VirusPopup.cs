using UnityEngine;
using UnityEngine.UI; // For CanvasGroup
using System.Collections;

public class VirusPopup : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject virusWindow;   // The Red Panel
    public CanvasGroup controlsHUD;  // The Group containing Joystick/Buttons

    [Header("Player Reference")]
    public CharacterMovement playerMovement; // DRAG PLAYER HERE

    [Header("Settings")]
    public float autoCloseTime = 5.0f;

    public void OpenPopup()
    {
        if (virusWindow != null)
        {
            virusWindow.SetActive(true);
            
            // 1. FREEZE CONTROLS (UI)
            if (controlsHUD != null)
            {
                controlsHUD.alpha = 0.5f; // Dim the buttons slightly
                controlsHUD.interactable = false;
                controlsHUD.blocksRaycasts = false; // Prevents touching joystick
            }

            // 2. FREEZE PHYSICS (Player)
            // This ensures the player stops moving immediately
            if (playerMovement != null)
            {
                playerMovement.canMove = false;
            }

            // 3. Start Timer
            StopAllCoroutines();
            StartCoroutine(AutoCloseRoutine());
        }
    }

    public void ClosePopup()
    {
        if (virusWindow != null)
        {
            virusWindow.SetActive(false);

            // 1. UNFREEZE CONTROLS
            if (controlsHUD != null)
            {
                controlsHUD.alpha = 1f; // Back to normal brightness
                controlsHUD.interactable = true;
                controlsHUD.blocksRaycasts = true;
            }

            // 2. UNFREEZE PHYSICS
            if (playerMovement != null)
            {
                playerMovement.canMove = true;
            }
        }
    }

    IEnumerator AutoCloseRoutine()
    {
        yield return new WaitForSeconds(autoCloseTime);
        ClosePopup();
    }
}