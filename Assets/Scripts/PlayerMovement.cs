using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;

    Transform cameraObject;
    InputHandler inputHandler;
    public Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animatorHandler;

    public new Rigidbody rigidbody;
    public Transform cam;

    [Header("Ground & Air Detection Stats")]
    [SerializeField]
    float groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField]
    float groundDirectionRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;

    [Header("Momevent Stats")]
    [SerializeField]
    float movementSpeed = 5f;
    [SerializeField]
    float sprintSpeed = 7f;
    [SerializeField]
    float rotationSpeed = 10f;
    [SerializeField]
    float fallingSpeed = 45f;
    [SerializeField]
    float ledgePush = 10f;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 7 | 1 << 11); //We could just set the Layer directly
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
            return;

        //Disable the player control when the character is Interacting or doing certain things.
        if (playerManager.isInteracting)
            return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if (inputHandler.moveAmount < 0.5f)
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.anim.GetBool("isInteracting"))
        {
            return;
        }
        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Roll", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                Debug.Log("Backstep");
                animatorHandler.PlayTargetAnimation("Backstep", true);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / ledgePush); //The ledgePush acts like a force, making the character kind of leap forwards when falling.
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("you were in the air for " + inAirTimer);
                    animatorHandler.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Movement", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if(playerManager.isGrounded)
            {
                playerManager.isGrounded = true;
            }
            if (playerManager.isInAir == false)
            {
                if (playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true);
                }
                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            myTransform.position = targetPosition;
        }

        if (playerManager.isGrounded)
        {
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (playerManager.isInteracting)
            return;

        if (inputHandler.jump_Input)
        {
            if (inputHandler.moveAmount > 0) //Can only jump while moving. We can ignore this IF, BUT we need to change how Jumping Works
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                animatorHandler.PlayTargetAnimation("Jump", true);

                //Y=0 because we are using the root motion of the jump animation
                moveDirection.y = 0;

                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRotation;

            }
        }
    }

    #endregion
    
    /*
    public CharacterController controller;
    public Transform cam;

    public Controls inputActions = null;

    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    //HIDE THESE
    public Vector3 velocity;
    public Vector2 axisInput;
    public bool isGrounded;

    private void Awake()
    {
        if (inputActions == null)
        {
            //We need to Instantiate the Control Asset via Script. It doesnt show in the inspector
            inputActions = new Controls();

            inputActions.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
            inputActions.Player.Sprint.performed += ctx => Sprint();
            inputActions.Player.Jump.performed += ctx => Jump();
            inputActions.Player.Attack.performed += ctx => Attack();
            inputActions.Player.Block.performed += ctx => Block();
            inputActions.Player.Skill1.performed += ctx => SkillOne();
            inputActions.Player.Skill2.performed += ctx => SkillTwo();
            inputActions.Player.Skill3.performed += ctx => SkillThree();
            inputActions.Player.Skill4.performed += ctx => SkillFour();
            inputActions.Player.Interact.performed += ctx => Interact();
            inputActions.Player.UseItem.performed += ctx => UseItem();
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        axisInput = inputActions.Player.Movement.ReadValue<Vector2>();
        Move(axisInput);
    }

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

    void Sprint() { Debug.Log("Sprint"); }

    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    void Attack() { Debug.Log("Attack"); }

    void Block() { Debug.Log("Block"); }

    void SkillOne() { Debug.Log("Skill1"); }

    void SkillTwo() { Debug.Log("Skill2"); }

    void SkillThree() { Debug.Log("Skill3"); }

    void SkillFour() { Debug.Log("Skill4"); }

    void Interact() { Debug.Log("Interact"); }

    void UseItem() { Debug.Log("UseItem"); }

    void ActionMapping()
    {

    }
*/
}
