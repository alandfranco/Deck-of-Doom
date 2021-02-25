using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        Debug.LogWarning("IsRolling " + anim.GetBool("isRolling"));
        Debug.LogWarning("IsWalking " + anim.GetBool("isWalking"));
    }

    public override void PassiveRegen()
    {
        stamina += config.staminaRegen * Time.deltaTime;
    }

    public override void GetScared(float duration)
    {
        base.GetScared(duration);
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

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
        goal.Add(new KeyValuePair<string, object>("stayAlive", true));
        return goal;
    }

    public override bool moveAgent(GOAPAction nextAction)
    {
        Debug.Log("ESNTRR");
        float dist = Vector3.Distance(this.transform.position, nextAction.target.transform.position);
        if (nextAction.requiesVision)
        {
            Debug.Log("ESNTRR a vbision");
            return MovementCheck(dist, nextAction);
        }
        else if (!nextAction.requiesVision)
        {
            Debug.Log("ESNTRR a no vbision");
            return MovementCheck(dist, nextAction);
        }
        
        return false;
    }

    bool MovementCheck(float dist, GOAPAction nextAction)
    {
        if (dist < config.aggroDist && dist > config.attackDistance && !anim.GetBool("isWalking"))
        {
            anim.SetBool("isRolling", true);
            agent.destination = nextAction.target.transform.position;
            agent.isStopped = false;
        }
        else if (dist < config.attackDistance * 2.5 && dist > config.attackDistance && !anim.GetBool("isRolling"))
        {
            anim.SetBool("isWalking", true);
            agent.destination = nextAction.target.transform.position;
            agent.isStopped = false;
        }
        if (dist <= config.attackDistance)
        {
            //anim.SetBool("isRolling", false);
            anim.SetBool("isWalking", false);
            agent.isStopped = true;
            nextAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}