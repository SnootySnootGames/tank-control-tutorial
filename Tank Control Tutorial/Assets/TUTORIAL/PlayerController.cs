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
    [SerializeField] private float playerSpeed = 3f;

    //player rotation speed adjustment. Modify in inspector for quick prototyping.
    [SerializeField] private float playerRotation = 40f;

    private void OnMove(InputValue value)
    {
        //store value recieved from input either keyboard or controller
        playerInput = value.Get<Vector2>(); 
    }

    private void PlayerMovement()
    {
        //use character controller api to move player. transform.forward 
        //gives player's facing direciton, which is multiplied by speed,  
        //and time.deltatime to make it smoother, and playinput.y to only
        //allow for forward and backward movement.
        controller.Move(transform.forward * playerInput.y * playerSpeed * Time.deltaTime);

        //use transform.rotate api. Use the declaration that uses axis of rotation
        //and "X" amount of degrees to rotate around given axis by. We will use
        //transform.up since that is the center axis of our character. For the degrees,
        //take player .x input, multiply it by rotation speed and time.deltaTime to smooth.
        transform.Rotate(transform.up, playerRotation * playerInput.x * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
}
