using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Handles Update Methods
    -Flags (isSprinting, isFalling, isInteracting, etc)
    Connnects all other functionality to player (other scripts)
*/

public class PlayerManager : Entity
{
    PlayerMovement playerMovement;
    InputHandler inputHandler;
    //Animator anim;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    private float _delta;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {


    }

    private void FixedUpdate()
    {
        Time.fixedDeltaTime = 1f / 60f;
        _delta = Time.deltaTime;
        inputHandler.TickInput(_delta);

        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        anim.SetBool("isInAir", isInAir);

        playerMovement.HandleMovement(_delta);
        playerMovement.HandleRotation(_delta);
        playerMovement.HandleRollingAndSprinting(_delta);
        playerMovement.HandleFalling(_delta, playerMovement.moveDirection);
        playerMovement.HandleJumping();        
    }

    private void LateUpdate()
    {
        //At the end of the Frame we reset all inputs
        //This also prevents input spam
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.lightAttack_Input = false;
        inputHandler.heavyAttack_Input = false;
        inputHandler.block_Input = false;
        inputHandler.jump_Input = false;

        if (isInAir)
        {
            playerMovement.inAirTimer = playerMovement.inAirTimer + Time.deltaTime;
        }
    }
}
