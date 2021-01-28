using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HealAction : GOAPAction
{
    private bool healed = false;

    public HealAction()
    {
        addEffect("buffEnemy", true);
        cost = 100f;
    }
    public override void Reset()
    {
        healed = false;
        target = null;
    }

    public override bool IsDone()
    {
        return healed;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        var posibleTarget = FindObjectsOfType<Enemy>().Where(x => x.health <= x.config.maxHealth / 3)
            .OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).ToList();
        if (posibleTarget.Count > 0)
        {
            target = posibleTarget[0].gameObject;
            return true;
        }
        return false;
    }

    public override bool Perform(GameObject agent)
    {
        Enemy currEnemy = agent.GetComponent<Enemy>();
        if (currEnemy.stamina >= (cost))
        {
            currEnemy.agent.isStopped = true;
            currEnemy.anim.Play("Attack");
            float damage = currEnemy.config.dmg;
            var _tar = target.GetComponent<Enemy>();
            _tar.health += _tar.config.maxHealth / damage;
            //target.GetComponent<TakeDamage>().TakeDamage();

            currEnemy.stamina -= cost;

            healed = true;
            return true;
        }
        else
            return false;
    }
}
