using UnityEngine;

public class HeadLook : MonoBehaviour
{
    [Header("Settings")]
    public Camera playerCamera;
    
    // 1 = Force head to look exactly at camera. 0 = Don't look.
    [Range(0, 1)] public float globalWeight = 1.0f;
    
    // How much the body/spine rotates to help the head (0.2 is usually good)
    [Range(0, 1)] public float bodyWeight = 0.2f; 
    
    // How much the head bone rotates
    [Range(0, 1)] public float headWeight = 1.0f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // This is a special Unity function that runs after the animation finishes
    void OnAnimatorIK()
    {
        if (animator != null && playerCamera != null)
        {
            // 1. Set how strong the effect is
            // (Global, Body, Head, Eyes, Clamp)
            animator.SetLookAtWeight(globalWeight, bodyWeight, headWeight, 1.0f, 0.5f);

            // 2. Calculate where to look
            // We take the camera position and extend a line 10 meters forward
            Vector3 lookTarget = playerCamera.transform.position + playerCamera.transform.forward * 10;

            // 3. Apply the look
            animator.SetLookAtPosition(lookTarget);
        }
    }
}