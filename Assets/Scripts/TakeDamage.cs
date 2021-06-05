using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    Entity me;

    public float health;
    public float maxHealth;
    
    bool imPlayer;

    public bool isBlocking;
    
    CardsContainer card;

    //AnimatorHandler anim;
    Animator anim;

    public ParticleSystem hitFX;

    public void Awake()
    {
        if (me is Enemy)
            maxHealth = this.GetComponent<Enemy>().config.maxHealth;
        if (this.GetComponent<PlayerMovement>())
        {
            imPlayer = true;
            card = this.GetComponent<CardsContainer>();
        }
        me = this.GetComponent<Entity>();

        if(me is PlayerManager)
        {
            anim = GetComponentInChildren<Animator>();
            maxHealth = maxHealth + (maxHealth * PlayerPassives.instance.healthBonus); 
        }
        health = maxHealth;
    }

    public void Heal(float amount)
    {        
        health += amount;
        if (health > maxHealth)
            health = maxHealth;

        if (imPlayer)
            (me as PlayerManager).UpdateHealthBar();
    }

    public bool TakeDamageToHealth(float damage, GameObject dmgDealer)
    {
        if(dmgDealer.GetComponent<PlayerManager>() == this.GetComponent<PlayerMovement>())
        {
            return false;
        }

        if(imPlayer && card.armorSlot)
        {
            card.armorSlot.TriggerCard(dmgDealer.GetComponent<Enemy>());
        }

        if (isBlocking && me.stamina > 0)
        {
            var initialDamage = damage;
            damage -= me.stamina;
            if(imPlayer)
            {
                anim.Play("BlockHit");
            }
            if (damage <= 0)
            {
                damage = 0;
                me.ReduceStamina(initialDamage);
            }
            else
            {
                hitFX.Play();
                me.ReduceStamina(damage);
            }
        }
        hitFX.Play();
        health -= damage;

        if (imPlayer)
        {
            (me as PlayerManager).UpdateHealthBar();
        }
        if(me is Enemy && (me as Enemy).canTeleport)
        {
            (me as Enemy).TeleportTo();
        }

        HandleAnimation();

        if (health <= 0)
        {
            if (dmgDealer.TryGetComponent<PlayerManager>(out var pl))
            {
                //le necesito informar al player que mato a alguien para algo??
            }
            Die();
            return true;
        }
        else
            return false;
    }

    void HandleAnimation()
    {
        if(imPlayer)
        {
            anim.Play("TakeDamage");
        }
    }

    public void TakeDamageOvertime(float duration, float damage, GameObject dmgDealer)
    {
        StartCoroutine(TakeDamageCoroutine(duration, damage, dmgDealer));
    }

    IEnumerator TakeDamageCoroutine(float duration, float damage, GameObject dmgDealer)
    {

        while (duration > 0)
        {
            yield return new WaitForSeconds(0.2f);
            duration -= 0.2f;
            TakeDamageToHealth(damage / 5, dmgDealer);
        }
        if(duration <= 0)
            yield break;
    }

    void Die()
    {
        if(this.GetComponent<PlayerManager>())
        {
            //Que pasa cuando muere el player
            GameManager.instance.RestartScene();
        }
        else
        {
            var enemy = me as Enemy;
            GameStats.instance.AddExp(enemy.experienceValue);
            if(enemy.canTeleport)
            {
                GameStats.instance.specialEnemiesKilled++;
            }
            else
                GameStats.instance.simpleEnemiesKilled++;
            me.anim.Play("Die");

            FindObjectOfType<PlayerManager>().potion.GetComponent<Potion>().FillPotion();
            StartCoroutine(DieCourutine());
        }
    }

    IEnumerator DieCourutine()
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
        yield break;
    }

    private void OnDisable()
    {
        health = maxHealth;
    }
}
