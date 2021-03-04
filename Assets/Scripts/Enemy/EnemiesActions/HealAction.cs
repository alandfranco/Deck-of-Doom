using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HealAction : GOAPAction
{
    private bool healed = false;
    Enemy me;
    public HealAction()
    {
        addEffect("buffEnemy", true);
        cost = 100f;
    }

    private void Start()
    {
        me = this.GetComponent<Enemy>();
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
        var posibleTarget = FindObjectsOfType<Enemy>().Where(x => x.GetComponent<TakeDamage>().health < x.GetComponent<TakeDamage>().maxHealth)
            .OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).ToList();

        if (posibleTarget.Count > 0 && !this.GetComponent<Enemy>().isDisable)
        {
            target = posibleTarget[0].gameObject;
            return true;
        }
        return false;
    }

    public override bool Perform(GameObject agent)
    {
        Enemy currEnemy = agent.GetComponent<Enemy>();
        if (currEnemy.stamina >= (cost) && !currEnemy.isDisable)
        {
            currEnemy.agent.isStopped = true;
            //currEnemy.anim.Play("Attack");
            float damage = currEnemy.config.dmg;
            var _tar = target.GetComponent<TakeDamage>();
            _tar.Heal(_tar.maxHealth / damage);

            currEnemy.stamina -= cost;

            healed = true;
            return true;
        }
        else
            return false;
    }
}
