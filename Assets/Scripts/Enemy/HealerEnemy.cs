using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        SetStartingValues(Resources.Load<ScriptableObject>("ScriptableObjects/HealerEnemy"));
    }

    public override void PassiveRegen()
    {
        stamina += config.staminaRegen * Time.deltaTime;
    }

    public override void AddBuff(string effect, float bonus, float duration)
    {
        base.AddBuff(effect, bonus, duration);
    }

    public override IEnumerator RemoveBuff(string effect, float duration)
    {
        return base.RemoveBuff(effect, duration);
    }

    public override void VisualBuff()
    {
        base.VisualBuff();
    }

    public override Transform BestPlaceToTeleport(Vector3 posToTravel)
    {
        return base.BestPlaceToTeleport(posToTravel);
    }

    public override bool moveAgent(GOAPAction nextAction)
    {                
        float dist = Vector3.Distance(this.transform.position, nextAction.target.transform.position);
        if (nextAction.requiesVision)
        {
            if (dist < config.aggroDist)
            {
                var teleporTo = BestPlaceToTeleport(nextAction.transform.transform.position);
                if (teleporTo != null)
                {
                    this.transform.position = teleporTo.position;
                }

                anim.Play("Move");
                anim.SetBool("isWalking", true);
                agent.destination = nextAction.target.transform.position;
                agent.isStopped = false;
            }
            if (dist <= config.attackDistance)
            {
                agent.isStopped = true;
                nextAction.setInRange(true);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (!nextAction.requiesVision)
        {
            var teleporTo = BestPlaceToTeleport(nextAction.transform.transform.position);
            if (teleporTo != null)
            {
                this.transform.position = teleporTo.position;
            }

            agent.destination = nextAction.target.transform.position;
            agent.isStopped = false;
            return true;
        }
        else
        {
            anim.Play("Idle");
            return false;
        }
    }

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
        goal.Add(new KeyValuePair<string, object>("buffEnemy", true));
        goal.Add(new KeyValuePair<string, object>("stayAlive", true));
        return goal;
    }
}
