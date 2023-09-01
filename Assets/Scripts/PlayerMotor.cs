using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isSprinting;
    private bool isCrouching;

    public float speed = 7.5f;
    public float sprintSpeed = 14f;
    public float crouchSpeed = 2.5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float controllerHeight;
    public Transform playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        controllerHeight = controller.height;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        //Determine if left shift key is pressed down to switch flag for isSprinting
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching) 
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        //Determine if C key is pressed to switch flag for isCrouching
        if (Input.GetKey(KeyCode.C) && isGrounded)
        {
            isCrouching = true;
            if(isCrouching)
            {
                playerCamera.localPosition = new Vector3(0,.5f,0);
                Debug.Log("Crouching");
            }
        }
        else
        {
            isCrouching = false;
            playerCamera.localPosition = new Vector3(0,.8f,0);
            Debug.Log("Standing");
        }
    }

    //Recieves input from InputManager.cs and applies them to our character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (isSprinting) 
        {
            controller.Move(transform.TransformDirection(moveDirection) * sprintSpeed * Time.deltaTime);
        }
        else if (isCrouching)
        {
            controller.Move(transform.TransformDirection(moveDirection) * crouchSpeed * Time.deltaTime);
        }
        else 
        {
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        }
        
        //Implementing gravity to increase over time 
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2;
        controller.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);
    }
    public void Jump()
    {   
        if(isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
    }
}
