using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Auto-configures all beautiful UI settings on scene load
/// Sets text content, colors, animation settings, etc.
/// Add this to any GameObject in the scene - runs once on Start()
/// </summary>
public class UIAutoConfigurator : MonoBehaviour
{
    void Start()
    {
        ConfigureAllUI();
    }
    
    /// <summary>
    /// Automatically configure all UI panels and elements
    /// </summary>
    void ConfigureAllUI()
    {
        Debug.Log("🎨 Auto-configuring beautiful UI...");
        
        ConfigureShopPanel();
        ConfigureEducationPanel();
        ConfigureVictoryPanel();
        ConfigureDefeatPanel();
        ConfigureHUD();
        ConfigurePanelAnimations();
        DisablePopupPanels();
        
        Debug.Log("✨ Beautiful UI configuration complete!");
    }
    
    #region Shop Panel Configuration
    void ConfigureShopPanel()
    {
        // Find ShopPanel
        GameObject shopPanel = GameObject.Find("Canvas/ShopPanel");
        if (shopPanel == null)
        {
            Debug.LogWarning("ShopPanel not found!");
            return;
        }
        
        // Configure ShopTitle
        Transform shopTitle = shopPanel.transform.Find("ShopTitle");
        if (shopTitle != null)
        {
            TextMeshProUGUI titleText = shopTitle.GetComponent<TextMeshProUGUI>();
            if (titleText != null)
            {
                titleText.text = "🛒 LOK TA SHOP";
                titleText.fontSize = 56;
                titleText.fontStyle = FontStyles.Bold;
                titleText.alignment = TextAlignmentOptions.Center;
                titleText.color = new Color(1f, 0.84f, 0f); // Gold
                titleText.outlineWidth = 0.3f;
                titleText.outlineColor = new Color(0, 0, 0, 0.8f);
                
                // Apply gradient
                titleText.enableVertexGradient = true;
                titleText.colorGradient = new VertexGradient(
                    new Color(0f, 0.9f, 1f),    // Cyan top-left
                    new Color(1f, 0f, 0.8f),    // Magenta top-right
                    new Color(1f, 0.84f, 0f),   // Gold bottom-left
                    new Color(1f, 0.84f, 0f)    // Gold bottom-right
                );
            }
        }
        
        // Configure RecommendationText
        Transform recText = shopPanel.transform.Find("RecommendationText");
        if (recText != null)
        {
            TextMeshProUGUI text = recText.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "💡 Choose wisely, defender!";
                text.fontSize = 28;
                text.alignment = TextAlignmentOptions.Center;
                text.color = new Color(0f, 0.9f, 1f); // Cyan
                text.outlineWidth = 0.2f;
                text.outlineColor = new Color(0, 0, 0, 0.8f);
            }
        }
        
        // Configure Buttons
        ConfigureShopButton(shopPanel, "BtnHealth", "🏥 Restore Health (+50 HP)\n<size=24>50 Points</size>");
        ConfigureShopButton(shopPanel, "BtnShield", "🛡️ Shield Boost (+100)\n<size=24>75 Points</size>");
        ConfigureShopButton(shopPanel, "BtnAmmo", "🔫 Ammo Refill (+30)\n<size=24>30 Points</size>");
        ConfigureShopButton(shopPanel, "BtnClose", "✖ Close", 32);
        
        Debug.Log("✅ Shop Panel configured");
    }
    
    void ConfigureShopButton(GameObject panel, string buttonName, string text, float fontSize = 32)
    {
        Transform btn = panel.transform.Find(buttonName);
        if (btn == null) return;
        
        // Get or create text child
        TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText == null)
        {
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btn);
            RectTransform rt = textObj.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
            
            btnText = textObj.AddComponent<TextMeshProUGUI>();
        }
        
        btnText.text = text;
        btnText.fontSize = fontSize;
        btnText.fontStyle = FontStyles.Bold;
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.color = Color.white;
        btnText.outlineWidth = 0.2f;
        btnText.outlineColor = new Color(0, 0, 0, 0.8f);
        
        // Configure button colors
        Button button = btn.GetComponent<Button>();
        if (button != null)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1f, 0.84f, 0f); // Gold
            colors.pressedColor = new Color(0f, 1f, 0.5f); // Green
            colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            colors.colorMultiplier = 1.5f;
            colors.fadeDuration = 0.2f;
            button.colors = colors;
        }
    }
    #endregion
    
    #region Education Panel Configuration
    void ConfigureEducationPanel()
    {
        GameObject eduPanel = GameObject.Find("Canvas/EducationPanel");
        if (eduPanel == null)
        {
            Debug.LogWarning("EducationPanel not found!");
            return;
        }
        
        // Configure EducationTitle
        Transform eduTitle = eduPanel.transform.Find("EducationTitle");
        if (eduTitle != null)
        {
            TextMeshProUGUI titleText = eduTitle.GetComponent<TextMeshProUGUI>();
            if (titleText != null)
            {
                titleText.text = "⚠️ SCAM ALERT!";
                titleText.fontSize = 64;
                titleText.fontStyle = FontStyles.Bold;
                titleText.alignment = TextAlignmentOptions.Center;
                titleText.color = new Color(1f, 0.1f, 0.3f); // Danger Red
                titleText.outlineWidth = 0.3f;
                titleText.outlineColor = new Color(0, 0, 0, 1f);
            }
        }
        
        // Configure DescriptionText
        Transform descText = eduPanel.transform.Find("DescriptionText");
        if (descText != null)
        {
            TextMeshProUGUI text = descText.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "Stay vigilant against cyber threats!\nLearn to protect yourself and others.";
                text.fontSize = 28;
                text.alignment = TextAlignmentOptions.Center;
                text.color = Color.white;
                text.outlineWidth = 0.2f;
                text.outlineColor = new Color(0, 0, 0, 0.8f);
                
                // Enable auto-sizing
                text.enableAutoSizing = true;
                text.fontSizeMin = 18;
                text.fontSizeMax = 28;
            }
        }
        
        // Configure ScamIcon size
        Transform icon = eduPanel.transform.Find("ScamIcon");
        if (icon != null)
        {
            RectTransform rt = icon.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(128, 128);
            }
            
            Image img = icon.GetComponent<Image>();
            if (img != null)
            {
                img.color = new Color(1f, 0.1f, 0.3f, 0.8f); // Red icon placeholder
            }
        }
        
        // Configure close button
        Transform btnClose = eduPanel.transform.Find("BtnCloseEducation");
        if (btnClose != null)
        {
            ConfigureCloseButton(btnClose.gameObject, "✖");
        }
        
        Debug.Log("✅ Education Panel configured");
    }
    #endregion
    
    #region Victory/Defeat Panels
    void ConfigureVictoryPanel()
    {
        GameObject victoryPanel = GameObject.Find("Canvas/VictoryPanel");
        if (victoryPanel == null) return;
        
        Transform title = victoryPanel.transform.Find("VictoryTitle");
        if (title != null)
        {
            TextMeshProUGUI titleText = title.GetComponent<TextMeshProUGUI>();
            if (titleText != null)
            {
                titleText.text = "🏆 VICTORY! 🎉";
                titleText.fontSize = 72;
                titleText.fontStyle = FontStyles.Bold;
                titleText.alignment = TextAlignmentOptions.Center;
                
                // Victory gradient (Green → Gold)
                titleText.enableVertexGradient = true;
                titleText.colorGradient = new VertexGradient(
                    new Color(0f, 1f, 0.5f),    // Success Green top
                    new Color(0f, 1f, 0.5f),    // Success Green top
                    new Color(1f, 0.84f, 0f),   // Gold bottom
                    new Color(1f, 0.84f, 0f)    // Gold bottom
                );
                
                titleText.outlineWidth = 0.3f;
                titleText.outlineColor = new Color(0, 0, 0, 1f);
            }
        }
        
        Debug.Log("✅ Victory Panel configured");
    }
    
    void ConfigureDefeatPanel()
    {
        GameObject defeatPanel = GameObject.Find("Canvas/DefeatPanel");
        if (defeatPanel == null) return;
        
        Transform title = defeatPanel.transform.Find("DefeatTitle");
        if (title != null)
        {
            TextMeshProUGUI titleText = title.GetComponent<TextMeshProUGUI>();
            if (titleText != null)
            {
                titleText.text = "💀 DEFEAT";
                titleText.fontSize = 72;
                titleText.fontStyle = FontStyles.Bold;
                titleText.alignment = TextAlignmentOptions.Center;
                
                // Defeat gradient (Red → Purple)
                titleText.enableVertexGradient = true;
                titleText.colorGradient = new VertexGradient(
                    new Color(1f, 0.1f, 0.3f),    // Danger Red top
                    new Color(1f, 0.1f, 0.3f),    // Danger Red top
                    new Color(0.15f, 0.05f, 0.3f), // Dark Purple bottom
                    new Color(0.15f, 0.05f, 0.3f)  // Dark Purple bottom
                );
                
                titleText.outlineWidth = 0.3f;
                titleText.outlineColor = new Color(0, 0, 0, 1f);
            }
        }
        
        Debug.Log("✅ Defeat Panel configured");
    }
    #endregion
    
    #region HUD Configuration
    void ConfigureHUD()
    {
        GameObject hud = GameObject.Find("Canvas/HUD");
        if (hud == null)
        {
            Debug.LogWarning("HUD not found!");
            return;
        }
        
        // Configure WaveText
        Transform waveText = hud.transform.Find("WaveText");
        if (waveText != null)
        {
            TextMeshProUGUI text = waveText.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "Wave: 1/10";
                text.fontSize = 40;
                text.fontStyle = FontStyles.Bold;
                text.alignment = TextAlignmentOptions.Left;
                text.color = new Color(0f, 0.9f, 1f); // Cyan
                text.outlineWidth = 0.2f;
                text.outlineColor = new Color(0, 0, 0, 0.8f);
                
                // Set anchor to top-left
                RectTransform rt = text.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchorMin = new Vector2(0, 1);
                    rt.anchorMax = new Vector2(0, 1);
                    rt.pivot = new Vector2(0, 1);
                    rt.anchoredPosition = new Vector2(30, -30);
                }
            }
        }
        
        // Configure EnemyCountText
        Transform enemyText = hud.transform.Find("EnemyCountText");
        if (enemyText != null)
        {
            TextMeshProUGUI text = enemyText.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "Enemies: 0";
                text.fontSize = 48;
                text.fontStyle = FontStyles.Bold;
                text.alignment = TextAlignmentOptions.Center;
                text.color = new Color(1f, 0f, 0.8f); // Magenta
                text.outlineWidth = 0.2f;
                text.outlineColor = new Color(0, 0, 0, 0.8f);
                
                // Set anchor to top-center
                RectTransform rt = text.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchorMin = new Vector2(0.5f, 1);
                    rt.anchorMax = new Vector2(0.5f, 1);
                    rt.pivot = new Vector2(0.5f, 1);
                    rt.anchoredPosition = new Vector2(0, -30);
                }
            }
        }
        
        // Configure PointsText
        Transform pointsText = hud.transform.Find("PointsText");
        if (pointsText != null)
        {
            TextMeshProUGUI text = pointsText.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "💰 Points: 0";
                text.fontSize = 52;
                text.fontStyle = FontStyles.Bold;
                text.alignment = TextAlignmentOptions.Right;
                text.color = new Color(1f, 0.84f, 0f); // Gold
                text.outlineWidth = 0.2f;
                text.outlineColor = new Color(0, 0, 0, 0.8f);
                
                // Set anchor to top-right
                RectTransform rt = text.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchorMin = new Vector2(1, 1);
                    rt.anchorMax = new Vector2(1, 1);
                    rt.pivot = new Vector2(1, 1);
                    rt.anchoredPosition = new Vector2(-30, -30);
                }
            }
        }
        
        Debug.Log("✅ HUD configured");
    }
    #endregion
    
    #region Panel Animations Configuration
    void ConfigurePanelAnimations()
    {
        // Shop Panel - Slide from bottom
        ConfigurePanelAnimation("Canvas/ShopPanel", 
            UIPanelAnimator.AnimationType.SlideAndFade, 
            UIPanelAnimator.SlideDirection.Bottom);
        
        // Education Panel - Scale with fade
        ConfigurePanelAnimation("Canvas/EducationPanel", 
            UIPanelAnimator.AnimationType.ScaleAndFade, 
            UIPanelAnimator.SlideDirection.Bottom);
        
        // Victory Panel - All animations from top
        ConfigurePanelAnimation("Canvas/VictoryPanel", 
            UIPanelAnimator.AnimationType.All, 
            UIPanelAnimator.SlideDirection.Top);
        
        // Defeat Panel - Scale with fade
        ConfigurePanelAnimation("Canvas/DefeatPanel", 
            UIPanelAnimator.AnimationType.ScaleAndFade, 
            UIPanelAnimator.SlideDirection.Bottom);
        
        Debug.Log("✅ Panel animations configured");
    }
    
    void ConfigurePanelAnimation(string panelPath, UIPanelAnimator.AnimationType animType, 
        UIPanelAnimator.SlideDirection slideDir)
    {
        GameObject panel = GameObject.Find(panelPath);
        if (panel == null) return;
        
        UIPanelAnimator animator = panel.GetComponent<UIPanelAnimator>();
        if (animator != null)
        {
            animator.animationType = animType;
            animator.slideDirection = slideDir;
            animator.animationDuration = 0.5f;
            animator.useScaleBounce = true;
            animator.useFade = true;
            animator.startScale = new Vector3(0.5f, 0.5f, 1f);
            animator.overshootScale = new Vector3(1.1f, 1.1f, 1f);
            animator.startAlpha = 0f;
        }
    }
    #endregion
    
    #region Helper Methods
    void DisablePopupPanels()
    {
        // Disable panels that should be hidden initially
        DisablePanel("Canvas/ShopPanel");
        DisablePanel("Canvas/EducationPanel");
        DisablePanel("Canvas/VictoryPanel");
        DisablePanel("Canvas/DefeatPanel");
        
        Debug.Log("✅ Popup panels disabled");
    }
    
    void DisablePanel(string panelPath)
    {
        GameObject panel = GameObject.Find(panelPath);
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
    
    void ConfigureCloseButton(GameObject button, string text)
    {
        TextMeshProUGUI btnText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText == null)
        {
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(button.transform);
            RectTransform rt = textObj.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
            
            btnText = textObj.AddComponent<TextMeshProUGUI>();
        }
        
        btnText.text = text;
        btnText.fontSize = 40;
        btnText.fontStyle = FontStyles.Bold;
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.color = Color.white;
    }
    #endregion
}
