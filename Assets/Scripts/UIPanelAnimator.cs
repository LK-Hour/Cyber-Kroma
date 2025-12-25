using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Beautiful panel animations - slide in, fade in, scale bounce
/// Makes UI feel professional and polished
/// </summary>
public class UIPanelAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    public AnimationType animationType = AnimationType.SlideAndFade;
    public float animationDuration = 0.5f;
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Slide Settings")]
    public SlideDirection slideDirection = SlideDirection.Bottom;
    public float slideDistance = 1000f;
    
    [Header("Scale Settings")]
    public bool useScaleBounce = true;
    public Vector3 startScale = new Vector3(0.5f, 0.5f, 1f);
    public Vector3 overshootScale = new Vector3(1.1f, 1.1f, 1f);
    
    [Header("Fade Settings")]
    public bool useFade = true;
    public float startAlpha = 0f;
    
    [Header("Sound Effects")]
    public AudioClip openSound;
    public AudioClip closeSound;
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private AudioSource audioSource;
    
    public enum AnimationType
    {
        Slide,
        Fade,
        Scale,
        SlideAndFade,
        ScaleAndFade,
        All
    }
    
    public enum SlideDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        // Add CanvasGroup if needed for fading
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null && useFade)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // Add AudioSource for sound effects
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (openSound != null || closeSound != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        
        originalPosition = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
    }
    
    void OnEnable()
    {
        PlayOpenAnimation();
    }
    
    /// <summary>
    /// Play beautiful opening animation
    /// </summary>
    public void PlayOpenAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateOpen());
        
        if (openSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }
    
    /// <summary>
    /// Play closing animation then disable
    /// </summary>
    public void PlayCloseAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateClose());
        
        if (closeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
    
    IEnumerator AnimateOpen()
    {
        float elapsed = 0f;
        
        // Calculate start position
        Vector3 startPosition = originalPosition;
        if (animationType == AnimationType.Slide || 
            animationType == AnimationType.SlideAndFade || 
            animationType == AnimationType.All)
        {
            startPosition = GetSlideStartPosition();
        }
        
        // Set initial state
        rectTransform.anchoredPosition = startPosition;
        if (useScaleBounce) rectTransform.localScale = startScale;
        if (useFade && canvasGroup != null) canvasGroup.alpha = startAlpha;
        
        // Animate to final state
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            float curveValue = animationCurve.Evaluate(t);
            
            // Position animation
            if (animationType == AnimationType.Slide || 
                animationType == AnimationType.SlideAndFade || 
                animationType == AnimationType.All)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(
                    startPosition, 
                    originalPosition, 
                    curveValue
                );
            }
            
            // Scale animation with bounce
            if (useScaleBounce && (
                animationType == AnimationType.Scale || 
                animationType == AnimationType.ScaleAndFade || 
                animationType == AnimationType.All))
            {
                Vector3 targetScale = t < 0.7f ? 
                    Vector3.Lerp(startScale, overshootScale, t / 0.7f) :
                    Vector3.Lerp(overshootScale, originalScale, (t - 0.7f) / 0.3f);
                rectTransform.localScale = targetScale;
            }
            
            // Fade animation
            if (useFade && canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, curveValue);
            }
            
            yield return null;
        }
        
        // Ensure final state
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localScale = originalScale;
        if (useFade && canvasGroup != null) canvasGroup.alpha = 1f;
    }
    
    IEnumerator AnimateClose()
    {
        float elapsed = 0f;
        Vector3 endPosition = GetSlideStartPosition();
        
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            float curveValue = animationCurve.Evaluate(t);
            
            // Reverse position animation
            if (animationType == AnimationType.Slide || 
                animationType == AnimationType.SlideAndFade || 
                animationType == AnimationType.All)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(
                    originalPosition, 
                    endPosition, 
                    curveValue
                );
            }
            
            // Reverse scale animation
            if (useScaleBounce && (
                animationType == AnimationType.Scale || 
                animationType == AnimationType.ScaleAndFade || 
                animationType == AnimationType.All))
            {
                rectTransform.localScale = Vector3.Lerp(
                    originalScale, 
                    startScale, 
                    curveValue
                );
            }
            
            // Reverse fade
            if (useFade && canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, startAlpha, curveValue);
            }
            
            yield return null;
        }
        
        gameObject.SetActive(false);
    }
    
    Vector3 GetSlideStartPosition()
    {
        Vector3 offset = Vector3.zero;
        
        switch (slideDirection)
        {
            case SlideDirection.Left:
                offset = new Vector3(-slideDistance, 0, 0);
                break;
            case SlideDirection.Right:
                offset = new Vector3(slideDistance, 0, 0);
                break;
            case SlideDirection.Top:
                offset = new Vector3(0, slideDistance, 0);
                break;
            case SlideDirection.Bottom:
                offset = new Vector3(0, -slideDistance, 0);
                break;
        }
        
        return originalPosition + offset;
    }
}
