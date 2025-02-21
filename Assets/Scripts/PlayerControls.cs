using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables
    public CharacterController controller;
    public Animator anim;
    public AudioClip runningSound;
    private AudioSource audioSource;

    public float runningSpeed = 4.0f;
    public float rotationSpeed = 100.0f;
    public float jumpHeight = 6.0f;

    private float jumpInput;
    private float runInput;
    private float rotateInput;

    public Vector3 moveDir;

    // Assuming you want to handle two players
    public bool playerOne = true; // Set this based on the current player
    public bool playerTwo = false;

    // Start function to get the components
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update function where movement and input handling happens
    void Update()
    {
        // Handle input for Player One or Player Two
        if (playerOne)
        {
            runInput = Input.GetAxis("Vertical");
            rotateInput = Input.GetAxis("Horizontal");
            if (controller.isGrounded)
            {
                jumpInput = Input.GetAxis("Jump");
            }
        }
        else
        {
            runInput = Input.GetAxis("VerticalTwo");
            rotateInput = Input.GetAxis("HorizontalTwo");
            if (controller.isGrounded)
            {
                jumpInput = Input.GetAxis("JumpTwo");
            }
        }

        // Move and Jump
        CheckJump();
        moveDir = new Vector3(0, jumpInput * jumpHeight, runInput * runningSpeed);

        // Transform moveDir to game world space
        moveDir = transform.TransformDirection(moveDir);

        // Move the character using the controller
        controller.Move(moveDir * Time.deltaTime);

        // Rotate the character based on horizontal input
        transform.Rotate(0f, rotateInput * rotationSpeed * Time.deltaTime, 0f);

        // Handle animations and sound effects
        Effects();
    }

    // Function to check for jump input and control jumping behavior
    void CheckJump()
    {
        // Check if the player is grounded
        if (controller.isGrounded)
        {
            if (jumpInput > 0) // Check if jump input is pressed
            {
                jumpInput = 1; // Start jumping
            }
            else
            {
                jumpInput = 0; // Reset jump input when grounded
            }
        }
    }

    // Function to handle animations and sound effects
    void Effects()
    {
        // Stop the running sound if it's not running or grounded
        if (audioSource != null && !controller.isGrounded && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Check if the player is running but not jumping
        if (runInput != 0 && jumpInput == 0)
        {
            anim.SetBool("Run Forward", true);

            // Play the running sound if not already playing
            if (audioSource != null && !audioSource.isPlaying && controller.isGrounded)
            {
                audioSource.clip = runningSound;
                audioSource.Play();
            }
        }
        else
        {
            // Stop running animation and sound if not running
            anim.SetBool("Run Forward", false);
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Handle Jump animation
        if (jumpInput != 0)
        {
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);
        }
    }
}
