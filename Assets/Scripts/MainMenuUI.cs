using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Main Menu UI Controller
/// Displays title, play button, tutorial button, quit button
/// Cyber Phnom Penh themed with beautiful animations
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mainMenuPanel;
    public TextMeshProUGUI titleText;
    public Button playButton;
    public Button tutorialButton;
    public Button quitButton;
    
    [Header("Animation")]
    public float titleAnimationSpeed = 2f;
    public Color[] glowColors = { Color.cyan, Color.magenta };
    
    private int colorIndex = 0;
    private float colorTransition = 0f;

    void Start()
    {
        SetupUI();
        SetupButtons();
        
        // Ensure panel is visible
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    void Update()
    {
        AnimateTitle();
    }

    void SetupUI()
    {
        // Configure title text
        if (titleText != null)
        {
            titleText.text = "⚡ CYBER KROMA ⚡\n<size=36>Defend Phnom Penh's Digital Heart</size>";
            titleText.fontSize = 85;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = Color.white;
            titleText.fontStyle = FontStyles.Bold;
        }
    }

    void SetupButtons()
    {
        // Play button - starts tutorial
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayClicked);
            
            TextMeshProUGUI btnText = playButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "PLAY GAME";
                btnText.fontSize = 15;
            }
        }

        // Tutorial button - goes to tutorial
        if (tutorialButton != null)
        {
            tutorialButton.onClick.RemoveAllListeners();
            tutorialButton.onClick.AddListener(OnTutorialClicked);
            
            TextMeshProUGUI btnText = tutorialButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "TUTORIAL";
                btnText.fontSize = 15;
            }
        }

        // Quit button
        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(OnQuitClicked);
            
            TextMeshProUGUI btnText = quitButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "QUIT";
                btnText.fontSize = 15;
            }
        }
    }

    void AnimateTitle()
    {
        if (titleText == null) return;

        // Smooth color transition
        colorTransition += Time.deltaTime * titleAnimationSpeed;
        
        if (colorTransition >= 1f)
        {
            colorTransition = 0f;
            colorIndex = (colorIndex + 1) % glowColors.Length;
        }

        Color currentColor = Color.Lerp(
            glowColors[colorIndex],
            glowColors[(colorIndex + 1) % glowColors.Length],
            colorTransition
        );

        titleText.color = currentColor;
    }

    #region Button Handlers

    void OnPlayClicked()
    {
        Debug.Log("🎮 Play button clicked - Loading Network Lobby!");
        
        // Go to multiplayer lobby (Host/Join screen)
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.LoadNetworkLobby();
        }
        else
        {
            Debug.LogError("GameSceneManager not found!");
        }
    }

    void OnTutorialClicked()
    {
        Debug.Log("📚 Tutorial button clicked!");
        
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.LoadTutorial();
        }
        else
        {
            Debug.LogError("GameSceneManager not found!");
        }
    }

    void OnQuitClicked()
    {
        Debug.Log("❌ Quit button clicked!");
        
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.QuitGame();
        }
        else
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    #endregion
}
