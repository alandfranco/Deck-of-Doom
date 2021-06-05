using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicMeleeEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        SetStartingValues(Resources.Load<ScriptableObject>("ScriptableObjects/BasicEnemy"));
    }

    protected override void Update()
    {
        base.Update();
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
        float dist = Vector3.Distance(this.transform.position, nextAction.target.transform.position);
        if (nextAction.requiesVision)
        {
            return MovementCheck(dist, nextAction);
        }
        else if (!nextAction.requiesVision)
        {
            return MovementCheck(dist, nextAction);
        }
        
        return false;
    }

    bool MovementCheck(float dist, GOAPAction nextAction)
    {
        /*NavMeshPath navMeshPath = new NavMeshPath();
        bool canReach = agent.CalculatePath(nextAction.target.transform.position, navMeshPath);
        if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
            return false;*/
        if (dist < config.aggroDist && dist > config.attackDistance && !anim.GetBool("isWalking"))
        {
            anim.SetBool("isRolling", true);
            anim.speed = Random.Range(0.8f, 1.2f);
            agent.destination = nextAction.target.transform.position;
            agent.isStopped = false;
        }
        else if (dist < config.attackDistance * 2.5 && dist > config.attackDistance && !anim.GetBool("isRolling"))
        {
            anim.SetBool("isWalking", true);
            anim.speed = Random.Range(0.8f, 1.2f);
            agent.destination = nextAction.target.transform.position;
            agent.isStopped = false;
        }
        if (dist <= config.attackDistance)
        {
            //anim.SetBool("isRolling", false);
            anim.SetBool("isWalking", false);
            anim.speed = 1;
            agent.isStopped = true;
            nextAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }
    /*
    public bool IsReachable(Vector3 pos)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        bool canReach = agent.CalculatePath(pos, navMeshPath);
        if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
            return false;
        else
            return true;
    }*/
}