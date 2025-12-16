using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Mobile Controls")]
    public Joystick moveJoystick;  // Drag "Fixed Joystick" here
    public TouchLook touchField;   // Drag "TouchZone" here

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float lookSensitivity = 0.2f; // Sensitivity for Touch
    public float mouseSensitivity = 2.0f; // Sensitivity for PC Mouse
    public float lookXLimit = 90.0f;
    [Range(0, 30)] public float cameraSmoothness = 15.0f; 

    [Header("Movement Settings")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float jumpForce = 8.0f;
    public float gravity = 25.0f;

    [Header("Animation")]
    public Animator animator; 

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    
    // Camera Variables
    private float targetRotationX = 0;
    private float targetRotationY = 0;
    private float currentRotationX = 0;
    private float currentRotationY = 0;
    
    // Internal
    private float rotationX = 0; 
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        // Auto-find Animator
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Set initial rotation
        targetRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        // =========================================================
        // 1. CAMERA LOGIC (LOOKING)
        // =========================================================
        if (canMove)
        {
            float inputX = 0;
            float inputY = 0;

            // A. Try Mobile Touch Input
            if (touchField != null)
            {
                inputX = touchField.lookX * lookSensitivity;
                inputY = touchField.lookY * lookSensitivity;
                
                // Reset touch immediately
                touchField.lookX = 0;
                touchField.lookY = 0;
            }
            
            // B. Fallback to PC Mouse (If no touch detected)
            if (inputX == 0 && inputY == 0)
            {
                inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
                inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            }

            // Calculate targets
            targetRotationX += -inputY;
            targetRotationX = Mathf.Clamp(targetRotationX, -lookXLimit, lookXLimit);
            targetRotationY += inputX;

            // Smooth Camera Movement
            if (cameraSmoothness > 0)
            {
                currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX, cameraSmoothness * Time.deltaTime);
                currentRotationY = Mathf.Lerp(currentRotationY, targetRotationY, cameraSmoothness * Time.deltaTime);
            }
            else
            {
                // Instant Movement (Valorant Style)
                currentRotationX = targetRotationX;
                currentRotationY = targetRotationY;
            }

            // Apply Rotation
            playerCamera.transform.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
            transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
        }

        // =========================================================
        // 2. MOVEMENT LOGIC (WALKING/RUNNING)
        // =========================================================
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float verticalInput = 0;
        float horizontalInput = 0;

        // A. Mobile Joystick Input
        if (moveJoystick != null)
        {
            verticalInput = moveJoystick.Vertical;
            horizontalInput = moveJoystick.Horizontal;
        }

        // B. Fallback to PC Keyboard (WASD)
        if (verticalInput == 0 && horizontalInput == 0)
        {
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
        }

        // Check for Run (Auto-run on mobile if pushed far, or Shift key on PC)
        bool isRunning = (Mathf.Abs(verticalInput) > 0.8f) || Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * verticalInput : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * horizontalInput : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // =========================================================
        // 3. JUMP & GRAVITY LOGIC
        // =========================================================
        if (characterController.isGrounded)
        {
            moveDirection.y = -2f; // Stick to ground

            // PC Jump (Space) OR Mobile Jump Button (requires button setup)
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
        else
        {
            // Apply Gravity
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Apply Final Move
        characterController.Move(moveDirection * Time.deltaTime);

        // =========================================================
        // 4. ANIMATION LOGIC
        // =========================================================
        if (animator != null)
        {
            animator.SetFloat("InputX", horizontalInput, 0.1f, Time.deltaTime);
            animator.SetFloat("InputY", verticalInput, 0.1f, Time.deltaTime);
            animator.SetBool("IsRunning", isRunning);
            animator.SetBool("IsGrounded", characterController.isGrounded);
        }
    }

    // Helper functions
    public void Jump()
    {
        if (characterController.isGrounded) moveDirection.y = jumpForce;
    }
    
    public void FootStep() { } 

    public void ResetCamera()
    {
        targetRotationX = 0;
        targetRotationY = transform.eulerAngles.y;
        currentRotationX = 0;
        currentRotationY = transform.eulerAngles.y;
        
        // Reset the visual camera rotation
        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.identity;
    }
}