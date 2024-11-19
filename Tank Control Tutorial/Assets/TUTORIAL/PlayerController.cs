using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Needed to access input system library
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    //use to store player input
    private Vector2 playerInput;

    //store ref to player's character controller to be used to move. 
    //Fill out field in inspector
    [SerializeField] CharacterController controller;

    //player speed adjustment value. Modify in inspector for quick prototyping.
    [SerializeField] private float playerSpeed = 15f;

    //player rotation speed adjustment. Modify in inspector for quick prototyping.
    [SerializeField] private float playerRotation = 40f;

    //used to control the speed of gravity that effects the player
    [SerializeField] private float gravityValue = -10f;

    //used to determine jump height
    [SerializeField] private float jumpHeight = 6f;

    private bool playerGrounded;
    private bool startJump = false;
    private float groundedTimer = 0f;

    //use to store the incremental velocity of gravity to be applied to player when not grounded.
    private Vector3 playerVelocity = Vector3.zero;

    private void OnMove(InputValue value)
    {
        //store value recieved from input either keyboard or controller
        playerInput = value.Get<Vector2>();
        //playerInput.Normalize();
    }

    private void OnJump()
    {
        //if player is "grounded", allow jump attempt
        if (groundedTimer > 0)
        {
            startJump = true;
        }
    }

    private void PlayerMovement()
    {
        //store character controller isGrounded  for ease of access
        playerGrounded = controller.isGrounded;

        //if player is grounded, set our grounded timer to X value.
        //This timer helps prevent the inconsistent nature of isGrounded.
        //This states the player is "grounded" when timer is greater than 0
        if (playerGrounded)
        {
            groundedTimer = 0.2f;
        }

        //If our timer is greater than zero, decrease timer by time.deltaTime
        //This timer will tell us if the player is still "grounded" without the
        //use of the isGrounded var.
        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }

        //reset player's vertical velocity to zero when grounded.
        if (playerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Increase gravity value over time
        playerVelocity.y += gravityValue * Time.deltaTime;

        //calculate players movement and store in a local vec3
        Vector3 storeMovement = transform.forward * playerInput.y * playerSpeed * Time.deltaTime;

        //inject player x and z movement back into our player's velocity vec3
        playerVelocity.x = storeMovement.x;
        playerVelocity.z = storeMovement.z;

        //if jump button is pressed and ground timer greater than zero, than jump will activate
        if (startJump && groundedTimer > 0)
        {
            groundedTimer = 0; //set to zero to prevent jumping while in the air
            startJump = false; //set to false to prevent a false jump input
            playerVelocity.y = jumpHeight; //set the .y velocity to move the player
        }

        //Move player using character controller .move API 
        controller.Move(playerVelocity * Time.deltaTime);

        //Rotate player using .Rotate API (using rotate around given axis via X amount of degrees)
        transform.Rotate(transform.up, playerRotation * playerInput.x * Time.deltaTime);

        startJump = false; //ensure jump command resets back to false after all player movement code runs.
    }


    // Update is called once per frame
    void Update()
    {
        //Run playerMovement code once per frame.
        PlayerMovement();
    }
}
