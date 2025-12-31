using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Tutorial Manager - Shows gameplay instructions
/// Brief tutorial then proceeds to combat
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI instructionText;
    public Button startGameButton;
    public Button skipButton;

    [Header("Tutorial Steps")]
    public string[] tutorialSteps = new string[]
    {
        "<b>OBJECTIVE</b>\n\nDefend the <color=#00E5FF>DATA CORE</color> from cyber threats!\nProtect Phnom Penh's digital infrastructure.",
        
        "<b>CONTROLS</b>\n\n<b>WASD</b> - Move\n<b>Mouse</b> - Look Around\n<b>Left Click</b> - Shoot\n<b>R</b> - Reload",
        
        "<b>ENEMIES</b>\n\n<color=#FF00CC>Phisher</color> - Ranged attacker\n<color=#00E5FF>Ghost Account</color> - Fast & stealthy\n<color=#FFD700>DeepFake</color> - Powerful tank",
        
        "<b>LOK TA SHOP</b>\n\nEarn points by defeating enemies\nBuy upgrades between waves:\n- Health Potion\n- Shield Boost\n- Ammo Refill",
        
        "<b>READY?</b>\n\nDefend the Data Core!\nSurvive all waves to win!\n\nGood luck, Guardian!"
    };

    private int currentStep = 0;

    void Start()
    {
        SetupUI();
        ShowCurrentStep();
    }

    void SetupUI()
    {
        // Setup title
        if (titleText != null)
        {
            titleText.text = "📚 TUTORIAL";
            titleText.fontSize = 80;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = new Color(0f, 0.9f, 1f); // Cyan
        }

        // Setup buttons
        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(OnNextStep);
            startGameButton.gameObject.SetActive(false); // Hide until last step
        }

        if (skipButton != null)
        {
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(OnSkipTutorial);
            
            TextMeshProUGUI btnText = skipButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "⏭️ Skip Tutorial";
            }
        }

        // Ensure panel is visible
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
    }

    void ShowCurrentStep()
    {
        if (instructionText == null) return;

        if (currentStep < tutorialSteps.Length)
        {
            instructionText.text = tutorialSteps[currentStep];
            instructionText.fontSize = 70;
            instructionText.alignment = TextAlignmentOptions.Center;

            // Show start button on last step
            if (currentStep == tutorialSteps.Length - 1 && startGameButton != null)
            {
                startGameButton.gameObject.SetActive(true);
                TextMeshProUGUI btnText = startGameButton.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = "START GAME";
                    btnText.fontSize = 40;
                }
            }
        }
    }

    void OnNextStep()
    {
        currentStep++;

        if (currentStep >= tutorialSteps.Length)
        {
            // Tutorial complete, start game
            StartGame();
        }
        else
        {
            ShowCurrentStep();
        }
    }

    void OnSkipTutorial()
    {
        Debug.Log("⏭️ Skipping tutorial...");
        StartGame();
    }

    void StartGame()
    {
        Debug.Log("Starting game from tutorial...");
        
        // Tutorial leads to Network Lobby (where players can host/join)
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.LoadNetworkLobby();
        }
        else
        {
            Debug.LogError("GameSceneManager not found!");
        }
    }

    void Update()
    {
        // Allow spacebar/enter to advance tutorial
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            OnNextStep();
        }
    }
}
