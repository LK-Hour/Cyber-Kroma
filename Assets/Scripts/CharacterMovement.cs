using UnityEngine;
using System.Collections; 

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Mobile Controls")]
    public Joystick moveJoystick;
    public TouchLook touchField;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float lookSensitivity = 0.2f;
    public float lookXLimit = 90.0f;
    [Range(0, 30)] public float cameraSmoothness = 15.0f; 

    [Header("Movement Settings")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float gravity = 25.0f;

    [Header("Roll Settings")]
    public float rollSpeed = 10.0f; 
    public float rollDuration = 0.8f; 

    [Header("Animation")]
    public Animator animator; 

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    
    // Camera Variables
    private float targetRotationX = 0;
    private float targetRotationY = 0;
    private float currentRotationX = 0;
    private float currentRotationY = 0;
    private float rotationX = 0; 
    
    // States
    public bool canMove = true;
    private bool isRolling = false; 

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        targetRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        // 1. Camera Logic
        if (canMove)
        {
            float inputX = 0;
            float inputY = 0;

            if (touchField != null)
            {
                inputX = touchField.lookX * lookSensitivity;
                inputY = touchField.lookY * lookSensitivity;
                touchField.lookX = 0;
                touchField.lookY = 0;
            }
            else if (Input.GetMouseButton(0)) 
            {
                inputX = Input.GetAxis("Mouse X") * 2.0f;
                inputY = Input.GetAxis("Mouse Y") * 2.0f;
            }

            targetRotationX += -inputY;
            targetRotationX = Mathf.Clamp(targetRotationX, -lookXLimit, lookXLimit);
            targetRotationY += inputX;

            if (cameraSmoothness > 0)
            {
                currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX, cameraSmoothness * Time.deltaTime);
                currentRotationY = Mathf.Lerp(currentRotationY, targetRotationY, cameraSmoothness * Time.deltaTime);
            }
            else
            {
                currentRotationX = targetRotationX;
                currentRotationY = targetRotationY;
            }

            playerCamera.transform.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
            transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
            rotationX = targetRotationX;
        }

        // 2. Movement Logic
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        // --- ROLL LOGIC ---
        if (isRolling)
        {
            moveDirection = forward * rollSpeed;
        }
        else
        {
            float verticalInput = 0;
            float horizontalInput = 0;

            // Combine Joystick + WASD input (whichever is stronger)
            float joystickV = 0;
            float joystickH = 0;
            float keyboardV = Input.GetAxis("Vertical");    // W/S or Up/Down
            float keyboardH = Input.GetAxis("Horizontal");  // A/D or Left/Right

            if (moveJoystick != null)
            {
                joystickV = moveJoystick.Vertical;
                joystickH = moveJoystick.Horizontal;
            }

            // Use the stronger input source (allows both to work simultaneously)
            verticalInput = Mathf.Abs(joystickV) > Mathf.Abs(keyboardV) ? joystickV : keyboardV;
            horizontalInput = Mathf.Abs(joystickH) > Mathf.Abs(keyboardH) ? joystickH : keyboardH;

            bool isRunning = Mathf.Abs(verticalInput) > 0.8f || Mathf.Abs(horizontalInput) > 0.8f;
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * verticalInput : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * horizontalInput : 0;

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (animator != null)
            {
                animator.SetFloat("InputX", horizontalInput, 0.1f, Time.deltaTime);
                animator.SetFloat("InputY", verticalInput, 0.1f, Time.deltaTime);
                animator.SetBool("IsRunning", isRunning);
            }
        }

        // Gravity
        if (characterController.isGrounded)
        {
            if (!isRolling) moveDirection.y = -2f; 
            else moveDirection.y = -2f; 
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    // --- TRIGGERED BY UI BUTTON ---
    public void DoRoll()
    {
        if (isRolling || !canMove) return;
        StartCoroutine(RollRoutine());
    }

    IEnumerator RollRoutine()
    {
        isRolling = true;
        if (animator != null) animator.SetTrigger("DoRoll");

        float originalHeight = characterController.height;
        Vector3 originalCenter = characterController.center;
        
        characterController.height = 0.5f; 
        characterController.center = new Vector3(0, 0.25f, 0); 

        yield return new WaitForSeconds(rollDuration);

        characterController.height = originalHeight;
        characterController.center = originalCenter;
        isRolling = false;
    }

    public void ResetCamera()
    {
        targetRotationX = 0;
        targetRotationY = transform.eulerAngles.y;
        currentRotationX = 0;
        currentRotationY = transform.eulerAngles.y;
        playerCamera.transform.localRotation = Quaternion.identity;
    }

    // ----------------------------------------------
    // Animation Event Callbacks (called from Roll animation)
    // ----------------------------------------------
    public void RollSound()
    {
        // TODO: Play roll sound effect here
        // AudioSource.PlayClipAtPoint(rollSoundClip, transform.position);
    }

    public void CantRotate()
    {
        // Called during roll animation to prevent rotation
        canMove = false;
    }

    public void EndRoll()
    {
        // Called at end of roll animation
        isRolling = false;
        canMove = true;
    }

    // ----------------------------------------------
    // THIS FIXES THE RED ERROR!
    // ----------------------------------------------
    public void FootStep()
    {
        // Leaving this empty creates a "Receiver" so the animation stops complaining.
    }
}