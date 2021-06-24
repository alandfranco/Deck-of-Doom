using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimatorHandler : MonoBehaviour
{
    PlayerManager playerManager;
    public Animator anim;
    InputHandler inputHandler;
    PlayerMovement playerMovement;
    int vertical;
    int horizontal;
    public bool canRotate;

    PlayerAttacker pl;

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        pl = GetComponentInParent<PlayerAttacker>();
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1f;
        }
        else
        {
            v = 0;
        }

        #endregion

        #region Horizontal
        float h = 0;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1f;
        }
        else
        {
            h = 0;
        }

        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, float fade)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, fade);
        playerManager.anim.SetBool("canDoCombo", false);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void EnableCombo()
    {
        //anim.SetBool("isInteracting", false);
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        //anim.SetBool("isInteracting", false);
        anim.SetBool("canDoCombo", false);
    }

    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false)
            return;

        float delta = Time.deltaTime;
        playerMovement.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerMovement.rigidbody.velocity = velocity;
    }

    public void DealDamage(float multiplier)
    {
        EnemiesInFront(pl.damageRadius, multiplier);
    }

    void EnemiesInFront(float radious, float dmgMultiplier)
    {
        var enemiesInFront = Physics.OverlapSphere(this.transform.position, radious);

        var _enemiesInFront = enemiesInFront.Where(x => Mathf.Abs(Vector3.Angle(pl.transform.forward, x.transform.position - pl.transform.position))
        <= 90 && !x.GetComponent<PlayerMovement>() && x.GetComponent<Enemy>()).Select(x => x.GetComponent<Enemy>());

        foreach (var item in _enemiesInFront)
        {
            if(FindObjectOfType<PlayerPassives>() && PlayerPassives.instance.damageBonus > 0)
            {
                item.GetComponent<TakeDamage>().TakeDamageToHealth(pl.damage + (pl.damage * PlayerPassives.instance.damageBonus) * dmgMultiplier, pl.gameObject);
            }
            else
                item.GetComponent<TakeDamage>().TakeDamageToHealth(pl.damage * dmgMultiplier, pl.gameObject);
            if (pl.cards.weaponSlot != null && item != null)
                pl.cards.weaponSlot.TriggerCard(item);
        }
    }
}
