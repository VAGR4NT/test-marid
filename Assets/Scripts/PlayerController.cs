using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float walkSpd = 2; // Set Walking Speed
    public float runSpd = 6; //Set Running Speed
    public float jumpHeight = 2;
    public float gravity = -12;

    public float turnSmoothTime = 0.1f; // Amount of time it takes to change directions while in motion
    public float turnSmoothVelocity; // Empty Variable representing the speed of the turn

    public float speedSmoothTime = 0.1f; // Amount of time necessary to transition from idle to walking or walking to running
    public float speedSmoothVelocity; // Empty Variable representing the speed of the acceleration
    public float currentSpeed;
    public float velocityY;

    Animator animator; // Animator Object
    Transform cameraT; // Camera Tracking TArget Object
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // Connect to Existing Animator Object Attatched to This Model
        cameraT = Camera.main.transform; // Sets Camera Transform thing
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Input from wasd or arrow keys
        Vector2 inputDir = input.normalized; // Direction the Model is facing and moving in

        if (inputDir != Vector2.zero) {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            //transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg; // Model Rotation Without Smoothing
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((isRunning) ? runSpd : walkSpd) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up*velocityY;
        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded) { velocityY = 0; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //float animationSpeedPercent = ((isRunning) ? 1 : .5f) * inputDir.magnitude;
        float animationSpeedPercent = ((isRunning) ? currentSpeed / runSpd : currentSpeed / walkSpd * .5f);
        animator.SetFloat("Speed_Percent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

    }

    void Jump() {
        if (controller.isGrounded) 
        {
            float jumpVelocity = Mathf.Sqrt(-2*gravity*jumpHeight);
            velocityY += jumpVelocity;
        }
    
    }

}
