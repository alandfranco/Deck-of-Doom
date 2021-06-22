using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Handles Update Methods
    -Flags (isSprinting, isFalling, isInteracting, etc)
    Connnects all other functionality to player (other scripts)
*/

public class PlayerManager : Entity
{
    PlayerMovement playerMovement;
    InputHandler inputHandler;
    Transform cam;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    private float _delta;
    [Header("UI")]
    public Transform OnWorldCanvas;
   
    [Header("UI Stamina")]
    public Image staminaBardelay;
    public Image staminaBar;
    bool startRechargeStamina;
    bool startReducingStamina;
    float speedToReduce;

    public GameObject shieldEffect;

    [Header("UI Health")]
    public Image healthBar;
    TakeDamage plHealth;

    [Header("UI Potion")]
    public Image potionDurationFill;
    public Image potionImage;
    public GameObject potionUI;

    public GameObject potion;
    [SerializeField]Transform potionPos;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        plHealth = this.GetComponent<TakeDamage>();

        cam = Camera.main.transform;

        if (FindObjectOfType<PlayerPassives>() != null)
        {
            maxStamina = maxStamina + (maxStamina * PlayerPassives.instance.staminaBonus);
        }
        
        stamina = maxStamina;

        staminaBar.fillAmount = 1;
        staminaBardelay.fillAmount = 1;
        healthBar.fillAmount = 1;
        OnWorldCanvas.gameObject.SetActive(false);
        potionUI.SetActive(false);
        shieldEffect.SetActive(false);
    }

    void Update()
    {
        if(OnWorldCanvas.gameObject.activeInHierarchy)
            OnWorldCanvas.LookAt(cam);

        #region Stamina
        if (startRechargeStamina)
        {
            stamina += staminaRecoverPS * Time.deltaTime;
            staminaBar.fillAmount = stamina / maxStamina;
            if (stamina >= maxStamina)
            {
                startRechargeStamina = false;
                stamina = maxStamina;
                staminaBar.fillAmount = stamina / maxStamina;
                OnWorldCanvas.gameObject.SetActive(false);
            }
        }
        if (startReducingStamina)
            StartReducingStamina();
        #endregion
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
        inputHandler.skill_input = false;
        inputHandler.block_Input = false;
        inputHandler.jump_Input = false;

        if (isInAir)
        {
            playerMovement.inAirTimer = playerMovement.inAirTimer + Time.deltaTime;
        }
    }

    public void DeactivatePotionUI()
    {
        potionUI.SetActive(false);
    }

    public void UsePotion()
    {
        if(potion != null)
        {
            potion.TryGetComponent<Potion>(out var pot);
            pot.TriggerPotion();
            if (pot.duration > 0)
            {
                potionUI.SetActive(true);
                pot.potIcon = potionImage;
                pot.potFill = potionDurationFill;
            }
        }
        //potion = null;
    }

    public void BuffStats()
    {
        /*foreach (var item in listaDePasivas.Key)
        {
            if(item == "damage")
            {
                this.GetComponent<PlayerAttacker>().damage += item;
            }
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Potion>())
        {
            if (potion != null)
                return;
            potion = other.gameObject;
            potion.transform.parent = potionPos;
            potion.transform.localPosition = Vector3.zero;
            potion.transform.localRotation = Quaternion.identity;
            potion.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    #region Health
    public void UpdateHealthBar()
    {
        healthBar.fillAmount = plHealth.health / plHealth.maxHealth;
    }
    #endregion

    #region Stamina

    public override void ReduceStamina(float amount)
    {
        StopCoroutine(UpdateStaminaBar());
        OnWorldCanvas.gameObject.SetActive(true);
        startRechargeStamina = false;

        if (!startReducingStamina)
            staminaBardelay.fillAmount = stamina / maxStamina;

        base.ReduceStamina(amount);
                
        StartCoroutine(UpdateStaminaBar());
    }

    IEnumerator UpdateStaminaBar()
    {
        staminaBar.fillAmount = stamina / maxStamina;
        yield return new WaitForSeconds(0.2f);
        startReducingStamina = true;
        speedToReduce = (staminaBardelay.fillAmount - (stamina / maxStamina)) / 0.7f;        
        yield return new WaitForSeconds(1f);
        startReducingStamina = false;
        startRechargeStamina = true;
        yield break;
    }

    void StartReducingStamina()
    {
        staminaBardelay.fillAmount -= speedToReduce * Time.deltaTime;        
    }
    #endregion
}