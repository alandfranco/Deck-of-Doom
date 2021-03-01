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

    public void Awake()
    {
        health = maxHealth;
        if (this.GetComponent<PlayerMovement>())
        {
            imPlayer = true;
            card = this.GetComponent<CardsContainer>();
        }
        me = this.GetComponent<Entity>();
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
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
            if (damage <= 0)
            {
                damage = 0;
                me.ReduceStamina(initialDamage);
            }
            else
                me.ReduceStamina(damage);
        }
        //else
         //   me.anim.Play("TakeDamage");

        health -= damage;

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
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        health = maxHealth;
    }
}
