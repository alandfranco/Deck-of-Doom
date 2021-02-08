using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    Entity me;

    public float health;
    public float maxHealth;

    public void Awake()
    {
        health = maxHealth;
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
