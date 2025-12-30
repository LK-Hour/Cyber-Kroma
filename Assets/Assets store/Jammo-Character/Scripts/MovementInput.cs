
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// //This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
// //With a blend tree to control the inputmagnitude and allow blending between animations.
// [RequireComponent(typeof(CharacterController))]
// public class MovementInput : MonoBehaviour {

//     public float Velocity;
//     [Space]

// 	public float InputX;
// 	public float InputZ;
// 	public Vector3 desiredMoveDirection;
// 	public bool blockRotationPlayer;
// 	public float desiredRotationSpeed = 0.1f;
// 	public Animator anim;
// 	public float Speed;
// 	public float allowPlayerRotation = 0.1f;
// 	public Camera cam;
// 	public CharacterController controller;
// 	public bool isGrounded;

//     [Header("Animation Smoothing")]
//     [Range(0, 1f)]
//     public float HorizontalAnimSmoothTime = 0.2f;
//     [Range(0, 1f)]
//     public float VerticalAnimTime = 0.2f;
//     [Range(0,1f)]
//     public float StartAnimTime = 0.3f;
//     [Range(0, 1f)]
//     public float StopAnimTime = 0.15f;

//     public float verticalVel;
//     private Vector3 moveVector;

// 	// Use this for initialization
// 	void Start () {
// 		anim = this.GetComponent<Animator> ();
// 		cam = Camera.main;
// 		controller = this.GetComponent<CharacterController> ();
// 	}
	
// 	// Update is called once per frame
// 	void Update () {
// 		InputMagnitude ();

//         isGrounded = controller.isGrounded;
//         if (isGrounded)
//         {
//             verticalVel -= 0;
//         }
//         else
//         {
//             verticalVel -= 1;
//         }
//         moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
//         controller.Move(moveVector);


//     }

//     void PlayerMoveAndRotation() {
// 		InputX = Input.GetAxis ("Horizontal");
// 		InputZ = Input.GetAxis ("Vertical");

// 		var camera = Camera.main;
// 		var forward = cam.transform.forward;
// 		var right = cam.transform.right;

// 		forward.y = 0f;
// 		right.y = 0f;

// 		forward.Normalize ();
// 		right.Normalize ();

// 		desiredMoveDirection = forward * InputZ + right * InputX;

// 		if (blockRotationPlayer == false) {
// 			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
//             controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
// 		}
// 	}

//     public void LookAt(Vector3 pos)
//     {
//         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
//     }

//     public void RotateToCamera(Transform t)
//     {

//         var camera = Camera.main;
//         var forward = cam.transform.forward;
//         var right = cam.transform.right;

//         desiredMoveDirection = forward;

//         t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
//     }

// 	void InputMagnitude() {
// 		//Calculate Input Vectors
// 		InputX = Input.GetAxis ("Horizontal");
// 		InputZ = Input.GetAxis ("Vertical");

// 		//anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
// 		//anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

// 		//Calculate the Input Magnitude
// 		Speed = new Vector2(InputX, InputZ).sqrMagnitude;

//         //Physically move player

// 		if (Speed > allowPlayerRotation) {
// 			anim.SetFloat ("Blend", Speed, StartAnimTime, Time.deltaTime);
// 			PlayerMoveAndRotation ();
// 		} else if (Speed < allowPlayerRotation) {
// 			anim.SetFloat ("Blend", Speed, StopAnimTime, Time.deltaTime);
// 		}
// 	}
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 15f;           // forward/backward speed
    public float rotationSpeed = 200f;     // left/right rotation speed
    public float gravity = -9.81f;         // gravity for character

    [Header("Animation Settings")]
    public Animator anim;                  // assign Jammo Animator here
    [Range(0, 1f)] public float StartAnimTime = 0.3f;
    [Range(0, 1f)] public float StopAnimTime = 0.15f;

    [Header("References")]
    public CharacterController controller;

    private Vector3 velocity;              // for gravity
    private float inputZ;                  // W/S input
    private float inputX;                  // A/D input

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (controller == null) controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
        HandleMovement();
        HandleAnimation();
    }

    void GetInput()
    {
        inputZ = Input.GetAxis("Vertical");   // W/S
        inputX = Input.GetAxis("Horizontal"); // A/D
    }

    void HandleMovement()
    {
        // Apply forward/backward movement
        Vector3 move = transform.forward * inputZ * moveSpeed * Time.deltaTime;
        controller.Move(move);

        // Apply rotation with A/D
        transform.Rotate(Vector3.up * inputX * rotationSpeed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f; // small downward force to keep grounded
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleAnimation()
    {
        // Use only forward/backward for Blend
        float speed = Mathf.Abs(inputZ);
        anim.SetFloat("Blend", speed, StartAnimTime, Time.deltaTime);
    }
}
