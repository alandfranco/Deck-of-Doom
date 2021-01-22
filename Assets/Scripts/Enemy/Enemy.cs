using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IGOAP
{
    [HideInInspector]
    public EnemySO config;
    [HideInInspector]
    public float health;
    //[HideInInspector]
    public float stamina;

    [HideInInspector]
    public NavMeshAgent agent;

    [HideInInspector]
    public Animator anim;

    public bool isScared;

    private void Start()
    {
        SetStartingValues(Resources.Load<ScriptableObject>("ScriptableObjects/HealerEnemy"));
    }

    public virtual void SetStartingValues(ScriptableObject SO)
    {
        config = (EnemySO)SO;
        health = config.maxHealth;

        stamina = config.maxStamina;

        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = config.speed;

        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        if (stamina < config.maxStamina)
            Invoke("PassiveRegen", 1.0f);
        else
            stamina = config.maxStamina;
        
        //Debug.LogWarning(isScared);
    }

    public abstract void PassiveRegen();

    public virtual void GetScared(float duration)
    {
        isScared = true;
        GetComponent<GotScaredAction>().count = duration;        
    }

    #region GOAP
    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("damagePlayer", false));
        worldData.Add(new KeyValuePair<string, object>("evadePlayer", false));
        return worldData;
    }

    public abstract HashSet<KeyValuePair<string, object>> createGoalState();

    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
    }

    public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions)
    {
    }

    public void actionsFinished()
    {
    }

    public void planAborted(GOAPAction aborter)
    {
    }

    public virtual bool moveAgent(GOAPAction nextAction)
    {
        float dist = Vector3.Distance(this.transform.position, nextAction.target.transform.position);
        if (nextAction.requiesVision)
        {
            if (dist < config.aggroDist)
            {
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
        else if(!nextAction.requiesVision)
        {

            agent.destination = nextAction.target.transform.position;
            agent.isStopped = false;
            return true;
        }
        else
        {
            return false;
        }
        
    }
    #endregion
}