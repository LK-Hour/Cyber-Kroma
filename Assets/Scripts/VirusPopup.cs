using UnityEngine;
using UnityEngine.UI;

public class VirusPopup : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject virusWindow; // Drag your Panel here

    [Header("Settings")]
    public bool openOnStart = false; 
    public float autoCloseTime = 0f; // 0 = never auto close

    void Start()
    {
        if (openOnStart)
        {
            OpenPopup();
        }
    }

    // Call this function to SHOW the virus
    public void OpenPopup()
    {
        if (virusWindow != null)
        {
            virusWindow.SetActive(true);
            
            // Optional: Play a sound effect here!
            // AudioSource.PlayClipAtPoint(virusSound, transform.position);
        }
    }

    // Call this function to HIDE the virus
    public void ClosePopup()
    {
        if (virusWindow != null)
        {
            virusWindow.SetActive(false);
        }
    }
}