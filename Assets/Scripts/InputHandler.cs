using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool roll_Input;
    public bool lightAttack_Input;
    public bool heavyAttack_Input;
    public bool block_Input;
    public bool jump_Input;
    public bool skill_input1;
    public bool skill_input2;
    public bool skill_input3;
    public bool skill_input4;
    public bool skill_input;
    public bool useItem_input;

    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public float rollInputTimer;


    Controls inputActions = null; //Cant be applied in Inspector
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    SkillManager skillManager;
    AnimatorHandler animHandler;
    Vector2 _movementInput;
    Vector2 _cameraInput;


    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        skillManager = GetComponent<SkillManager>();
        animHandler = GetComponentInChildren<AnimatorHandler>();

        if (inputActions == null)
        {
            //We need to Instantiate the Control Asset via Script. It doesnt show in the inspector
            inputActions = new Controls();

            inputActions.Player.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            //inputActions.Player.XYAxis.performed += ctx => _cameraInput = ctx.ReadValue<Vector2>();
        }

        SetCursorState(false, true);
    }

    #region Enable/Disable
    //If we have to apply changes to all control schemes we just have to put "inputActions.Enable" "...Disable".

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    #endregion

    #region Actions
    public void TickInput(float delta)
    {
        if (Input.GetKey(KeyCode.Tab))
            return;
        MoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleBlockInput();
        HandleJumpInput();
        HandleSkillOne();
        HandleSkillTwo();
        HandleSkillThree();
        HandleSkillFour();
        HandleSkill();
        HandleUseItem();
    }

    private void MoveInput(float delta)
    {
        horizontal = _movementInput.x;
        vertical = _movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        //mouseX = _cameraInput.x;
        //mouseY = _cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        roll_Input = inputActions.Player.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        if (roll_Input)
        {
            rollInputTimer += delta;
            playerManager.anim.Play("Dash");
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleAttackInput(float delta)
    {
        inputActions.Player.LightAttack.performed += i => lightAttack_Input = true;
        inputActions.Player.HeavyAttack.performed += i => heavyAttack_Input = true;

        //Right Handed
        if (lightAttack_Input)
        {
            if (animHandler.anim.GetBool("isReturning") && !playerManager.canDoCombo)
                return;
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleAttack(playerInventory.rightWeapon, true);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                {
                    playerManager.anim.SetBool("canDoCombo", false);
                    comboFlag = false;
                    return;
                }
                comboFlag = false;
                playerAttacker.HandleAttack(playerInventory.rightWeapon, false);
            }
        }
        //if (heavyAttack_Input)
        //{
        //    if (playerManager.isInteracting)
        //        return;
        //    playerAttacker.HandleAttack(playerInventory.rightWeapon);
        //}
    }

    private void HandleBlockInput()
    {
        block_Input = (inputActions.Player.Block.phase == UnityEngine.InputSystem.InputActionPhase.Performed);
        //inputActions.Player.Block.performed += i => block_Input = true;
        if (block_Input && !this.GetComponent<TakeDamage>().isBlocking)
        {
            playerManager.anim.Play("BlockStart");
            this.GetComponent<TakeDamage>().isBlocking = true;                        
        }
        else if (block_Input && this.GetComponent<TakeDamage>().isBlocking)
        {
            playerManager.shieldEffect.SetActive(true);
            //playerManager.anim.Play("BlockLoop");
            playerManager.anim.SetBool("isBlocking", true);
        }
        else
        {
            playerManager.shieldEffect.SetActive(false);
            playerManager.anim.SetBool("isBlocking", false);

            this.GetComponent<TakeDamage>().isBlocking = false;
        }
    }

    //HandleQuickSlotsInput
    //HandleInteractingButtonInput

    private void HandleJumpInput()
    {
        //inputActions.Player.Jump.performed += i => jump_Input = true;
    }

    private void SetCursorState(bool isVisible, bool isLocked)
    {
        Cursor.visible = isVisible;
        if(isLocked)
        Cursor.lockState = CursorLockMode.Locked;
        else
        Cursor.lockState = CursorLockMode.Confined;
    }

    void HandleSkill()
    {
        skill_input = inputActions.Player.Skill.triggered;

        if(skill_input)
        {
            skillManager.PerfomSkill();
        }
    }

    void HandleSkillOne()
    {
        skill_input1 = inputActions.Player.Skill1.triggered;

        if (skill_input1)
        {
            skillManager.SelectSkill(0);
            //skillManager.PerformSkilOne();
        }
    }

    void HandleSkillTwo()
    {
        skill_input2 = inputActions.Player.Skill2.triggered;

        if (skill_input2)
        {
            skillManager.SelectSkill(1);
            //skillManager.PerfomSkillTwo();
        }
    }

    void HandleSkillThree()
    {
        skill_input3 = inputActions.Player.Skill3.triggered;

        if(skill_input3)
        {
            skillManager.SelectSkill(2);
            //skillManager.PerfomSkillThree();
        }
    }

    void HandleSkillFour()
    {
        skill_input4 = inputActions.Player.Skill4.triggered;

        if(skill_input4)
        {
            skillManager.SelectSkill(3);
            //skillManager.PerfomSkillFour();
        }
    }

    void HandleUseItem()
    {
        useItem_input = inputActions.Player.UseItem.triggered;

        if(useItem_input)
        {
            playerManager.UsePotion();
        }
    }

    /*
    void Sprint() { Debug.Log("Sprint"); }

    void Jump()
    {
        //if (isGrounded)
        //{
        //    velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        //}
    }

    void Attack() { Debug.Log("Attack"); }

    void Block() { Debug.Log("Block"); }

    

    

    

    void SkillFour() { Debug.Log("Skill4"); }

    void Interact() { Debug.Log("Interact"); }

    void UseItem() { Debug.Log("UseItem"); }
      
    void Move(Vector2 direction)
    {
        //This projects a sphere on the bottom of the character that is going to check if it is touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //0f sometimes doesnt work as expected
        }

        if (direction.magnitude >= 0.1f)
        {
            //Atan2 returns an angle in the YAxis that corresponds to the direction of the object.
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;  //Rad2Deg because Atan2 is in Radians
            //This will generate a transition between the last and current direction
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    */
    #endregion
}
