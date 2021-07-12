using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BasicRangeAction : GOAPAction
{
    private bool attacked = false;

    public float bulletSpeed;

    public GameObject bulletPrefab;

    public BasicRangeAction()
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
        Enemy currEnemy = agent.GetComponent<Enemy>();
        if (currEnemy.stamina >= (cost) && !currEnemy.isDisable)
        {
            Vector3 targetDirection = target.transform.position - this.transform.position;
            Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, targetDirection, 120 * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(newDirection);   
            
            currEnemy.agent.isStopped = true;
            currEnemy.anim.Play("Attack");

            float damage = currEnemy.config.dmg + currEnemy.BonusBuff();
            var bullet = ObjectPooler.instance.GetPooledObject(bulletPrefab, currEnemy.weapon.transform.position);
            bullet.SetActive(true);
            bullet.transform.position = currEnemy.weapon.transform.position;
            bullet.transform.LookAt(target.transform.position + new Vector3(0, 1.5f, 0));

            bullet.GetComponent<EnemyBullet>().speed = bulletSpeed;
            bullet.GetComponent<EnemyBullet>().damage = damage;

            currEnemy.stamina -= cost;

            attacked = true;
            return true;
        }
        else
            return false;
    }
}
