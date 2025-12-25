using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Beautiful UI styling system with cyber Phnom Penh aesthetic
/// Manages colors, gradients, animations for all UI panels
/// </summary>
public class UIStyleManager : MonoBehaviour
{
    [Header("Cyber Kroma Color Palette")]
    public Color primaryCyan = new Color(0f, 0.9f, 1f, 1f);        // #00E5FF - Electric Cyan
    public Color primaryMagenta = new Color(1f, 0f, 0.8f, 1f);     // #FF00CC - Neon Magenta
    public Color accentGold = new Color(1f, 0.84f, 0f, 1f);        // #FFD700 - Khmer Gold
    public Color darkPurple = new Color(0.15f, 0.05f, 0.3f, 0.95f); // #260D4D - Dark Purple BG
    public Color successGreen = new Color(0f, 1f, 0.5f, 1f);       // #00FF80 - Success
    public Color dangerRed = new Color(1f, 0.1f, 0.3f, 1f);        // #FF1A4D - Danger
    
    [Header("UI References")]
    public Image shopPanelBG;
    public Image educationPanelBG;
    public Image victoryPanelBG;
    public Image defeatPanelBG;
    public Image[] buttonBackgrounds;
    
    [Header("Gradient Settings")]
    public bool useGradients = true;
    public float gradientAngle = 45f;
    
    [Header("Glow Effect")]
    public bool enableGlow = true;
    public float glowIntensity = 2f;
    public float glowPulseSpeed = 1.5f;
    
    private Material glowMaterial;
    
    void Start()
    {
        ApplyBeautifulStyles();
    }
    
    /// <summary>
    /// Apply cyber Phnom Penh aesthetic to all UI panels
    /// </summary>
    public void ApplyBeautifulStyles()
    {
        // Shop Panel - Cyan/Gold gradient
        if (shopPanelBG != null)
        {
            ApplyGradient(shopPanelBG, primaryCyan, accentGold);
            AddShadowAndOutline(shopPanelBG.gameObject);
        }
        
        // Education Panel - Magenta/Cyan gradient
        if (educationPanelBG != null)
        {
            ApplyGradient(educationPanelBG, primaryMagenta, primaryCyan);
            AddShadowAndOutline(educationPanelBG.gameObject);
        }
        
        // Victory Panel - Green/Gold gradient
        if (victoryPanelBG != null)
        {
            ApplyGradient(victoryPanelBG, successGreen, accentGold);
            AddShadowAndOutline(victoryPanelBG.gameObject);
        }
        
        // Defeat Panel - Red/Purple gradient
        if (defeatPanelBG != null)
        {
            ApplyGradient(defeatPanelBG, dangerRed, darkPurple);
            AddShadowAndOutline(defeatPanelBG.gameObject);
        }
        
        // Style all buttons
        foreach (Image btnBG in buttonBackgrounds)
        {
            if (btnBG != null)
            {
                StyleButton(btnBG);
            }
        }
        
        Debug.Log("✨ Beautiful UI styles applied! Cyber Kroma aesthetic active.");
    }
    
    /// <summary>
    /// Apply beautiful gradient to UI element
    /// </summary>
    void ApplyGradient(Image image, Color color1, Color color2)
    {
        if (!useGradients)
        {
            image.color = color1;
            return;
        }
        
        // Create gradient material
        Material gradientMat = new Material(Shader.Find("UI/Default"));
        gradientMat.SetColor("_Color", color1);
        
        // Apply to image
        image.material = gradientMat;
        image.color = Color.Lerp(color1, color2, 0.5f);
        
        // Add glow effect
        if (enableGlow)
        {
            AddGlowEffect(image.gameObject, color1);
        }
    }
    
    /// <summary>
    /// Add glowing outline to UI element
    /// </summary>
    void AddGlowEffect(GameObject uiElement, Color glowColor)
    {
        Outline outline = uiElement.GetComponent<Outline>();
        if (outline == null)
        {
            outline = uiElement.AddComponent<Outline>();
        }
        
        outline.effectColor = glowColor;
        outline.effectDistance = new Vector2(3, -3);
        outline.useGraphicAlpha = false;
        
        // Add pulsing animation
        UIGlowPulse pulse = uiElement.GetComponent<UIGlowPulse>();
        if (pulse == null)
        {
            pulse = uiElement.AddComponent<UIGlowPulse>();
        }
        pulse.pulseSpeed = glowPulseSpeed;
        pulse.minIntensity = 0.5f;
        pulse.maxIntensity = glowIntensity;
    }
    
    /// <summary>
    /// Add shadow and outline for depth
    /// </summary>
    void AddShadowAndOutline(GameObject uiElement)
    {
        // Shadow
        Shadow shadow = uiElement.GetComponent<Shadow>();
        if (shadow == null)
        {
            shadow = uiElement.AddComponent<Shadow>();
        }
        shadow.effectColor = new Color(0, 0, 0, 0.5f);
        shadow.effectDistance = new Vector2(5, -5);
        
        // Outline for cyber aesthetic
        Outline outline = uiElement.GetComponent<Outline>();
        if (outline == null)
        {
            outline = uiElement.AddComponent<Outline>();
        }
        outline.effectColor = primaryCyan;
        outline.effectDistance = new Vector2(2, -2);
    }
    
    /// <summary>
    /// Style button with hover effects
    /// </summary>
    void StyleButton(Image buttonBG)
    {
        // Gradient background
        ApplyGradient(buttonBG, primaryCyan, primaryMagenta);
        
        // Add button hover effect
        Button btn = buttonBG.GetComponent<Button>();
        if (btn != null)
        {
            ColorBlock colors = btn.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = accentGold;
            colors.pressedColor = successGreen;
            colors.selectedColor = primaryCyan;
            colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            colors.colorMultiplier = 1.5f;
            colors.fadeDuration = 0.2f;
            btn.colors = colors;
        }
        
        // Add scale animation on hover
        UIButtonHoverScale hoverScale = buttonBG.GetComponent<UIButtonHoverScale>();
        if (hoverScale == null)
        {
            hoverScale = buttonBG.gameObject.AddComponent<UIButtonHoverScale>();
        }
        hoverScale.normalScale = Vector3.one;
        hoverScale.hoverScale = new Vector3(1.1f, 1.1f, 1f);
        hoverScale.animationSpeed = 5f;
    }
    
    /// <summary>
    /// Update text styling for cyber aesthetic
    /// </summary>
    public void StyleText(TextMeshProUGUI textElement, bool isTitle = false)
    {
        if (textElement == null) return;
        
        if (isTitle)
        {
            // Title styling
            textElement.fontSize = 48;
            textElement.fontStyle = FontStyles.Bold;
            textElement.color = accentGold;
            textElement.enableAutoSizing = false;
            
            // Gradient on title
            textElement.enableVertexGradient = true;
            VertexGradient gradient = new VertexGradient(
                primaryCyan,    // Top Left
                primaryMagenta, // Top Right
                accentGold,     // Bottom Left
                accentGold      // Bottom Right
            );
            textElement.colorGradient = gradient;
        }
        else
        {
            // Body text styling
            textElement.fontSize = 24;
            textElement.fontStyle = FontStyles.Normal;
            textElement.color = Color.white;
        }
        
        // Add outline for readability
        textElement.outlineWidth = 0.2f;
        textElement.outlineColor = new Color(0, 0, 0, 0.8f);
        
        // Add glow
        textElement.fontMaterial.EnableKeyword("GLOW_ON");
        textElement.fontMaterial.SetColor("_GlowColor", primaryCyan);
        textElement.fontMaterial.SetFloat("_GlowPower", 0.5f);
    }
}

/// <summary>
/// Pulsing glow animation for UI elements
/// </summary>
public class UIGlowPulse : MonoBehaviour
{
    public float pulseSpeed = 1.5f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    
    private Outline outline;
    private float timer;
    
    void Start()
    {
        outline = GetComponent<Outline>();
    }
    
    void Update()
    {
        if (outline == null) return;
        
        timer += Time.deltaTime * pulseSpeed;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, 
            (Mathf.Sin(timer) + 1f) * 0.5f);
        
        Color glowColor = outline.effectColor;
        glowColor.a = intensity;
        outline.effectColor = glowColor;
    }
}

/// <summary>
/// Button hover scale animation
/// </summary>
public class UIButtonHoverScale : MonoBehaviour
{
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    public float animationSpeed = 5f;
    
    private RectTransform rectTransform;
    private bool isHovered = false;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogWarning($"⚠️ UIButtonHoverScale on '{gameObject.name}' requires RectTransform! Disabling component.");
            enabled = false;
        }
    }
    
    void Update()
    {
        if (rectTransform == null) return;
        
        Vector3 targetScale = isHovered ? hoverScale : normalScale;
        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale, 
            targetScale, 
            Time.deltaTime * animationSpeed
        );
    }
    
    public void OnPointerEnter()
    {
        isHovered = true;
    }
    
    public void OnPointerExit()
    {
        isHovered = false;
    }
}
