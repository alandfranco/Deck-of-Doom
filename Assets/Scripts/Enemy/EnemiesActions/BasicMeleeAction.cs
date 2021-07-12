using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeAction : GOAPAction
{
    private bool attacked = false;

    public GameObject bulletPrefab;

    Enemy currEnemy;

    GameObject player;

    public BasicMeleeAction()
    {
        addEffect("damagePlayer", true);
        cost = 100f;
    }

    private void Start()
    {
        currEnemy = this.GetComponent<Enemy>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    public override void Reset()
    {
        attacked = false;
        target = null;
    }

    public override bool IsDone()
    {
        return attacked;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = EnemiesManager.instance.GetTarget();
        if (this.GetComponent<Enemy>().isDisable)
        {
            target = null;
            return false;
        }
        else
            return target != null;
    }

    public override bool Perform(GameObject agent)
    {        
        if (currEnemy.stamina >= (cost) && !currEnemy.isDisable)
        {
            currEnemy.agent.isStopped = true;
            if (currEnemy.anim.GetBool("isRolling") && GameManager.instance.enemiesAttacking < GameManager.instance.maxAmountAttacking)
            {
                currEnemy.anim.SetBool("rollAttack", true);
                //currEnemy.anim.Play("RollAttack");
                currEnemy.anim.SetBool("isRolling", false);
            }
            else if(currEnemy.anim.GetBool("isRolling") && GameManager.instance.enemiesAttacking >= GameManager.instance.maxAmountAttacking)
            {
                currEnemy.anim.SetBool("isRolling", false);
                currEnemy.anim.SetBool("rollAttack", false);
                return false;
            }
            else
                currEnemy.anim.Play("Attack");


            GameManager.instance.enemiesAttacking++;

            currEnemy.stamina -= cost;

            attacked = true;            
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DealDamage(GameObject victim)
    {
        float damage = currEnemy.config.dmg;
        victim.GetComponent<TakeDamage>().TakeDamageToHealth(currEnemy.config.dmg + currEnemy.BonusBuff(), this.gameObject);
    }
}
