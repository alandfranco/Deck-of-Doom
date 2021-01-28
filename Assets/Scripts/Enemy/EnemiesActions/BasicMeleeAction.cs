﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeAction : GOAPAction
{
    private bool attacked = false;

    public GameObject bulletPrefab;

    public BasicMeleeAction()
    {
        addEffect("damagePlayer", true);
        cost = 100f;
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
        target = FindObjectOfType<PlayerMovement>().gameObject;
        if (this.GetComponent<Enemy>().isScared)
        {
            target = null;
            return false;
        }
        else
            return target != null;
    }

    public override bool Perform(GameObject agent)
    {
        Enemy currEnemy = agent.GetComponent<Enemy>();
        if (currEnemy.stamina >= (cost) && !currEnemy.isScared)
        {
            currEnemy.agent.isStopped = true;
            currEnemy.anim.Play("Attack");
            
            float damage = currEnemy.config.dmg;
            //target.GetComponent<TakeDamage>().TakeDamage();

            currEnemy.stamina -= cost;

            attacked = true;
            return true;
        }
        else
            return false;
    }
}
