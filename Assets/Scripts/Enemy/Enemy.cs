using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Entity, IGOAP
{
    [HideInInspector]
    public EnemySO config;
    [HideInInspector]
    public float health;

    [HideInInspector]
    public NavMeshAgent agent;

    [HideInInspector]
    public Animator anim;

    public bool isScared;
    public bool isStuned;
    public bool isDisable;
    float disableCD;

    public GameObject weapon;

    public Dictionary<string, float> buffs = new Dictionary<string, float>();

    [Header("BuffsFXs")]
    public GameObject venomFX;

    protected virtual void Awake()
    {
        SetStartingValues(Resources.Load<ScriptableObject>("ScriptableObjects/BasicEnemy"));
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
        CheckDisabled();
    }

    protected virtual void CheckDisabled()
    {
        if (disableCD > 0)
        {            
            disableCD -= Time.deltaTime;            
        }
        else
        {
            isDisable = false;
            isStuned = false;
            isScared = false;            
        }
    }

    public abstract void PassiveRegen();

    public virtual void GetScared(float duration)
    {
        isScared = true;
        GetComponent<GotScaredAction>().count = duration;
        GetDisabled(duration);
    }

    public virtual void GetStuned(float duration)
    {
        isStuned = true;
        GetComponent<GotStunedAction>().count = duration;
        Debug.Log("GOT SUTNEd");
        GetDisabled(duration);
    }

    public virtual void GetDisabled(float duration)
    {
        if (disableCD < duration)
            disableCD = duration;
        isDisable = true;        
    }

    public virtual void AddBuff(string effect, float bonus, float duration)
    {
        buffs.Add(effect, bonus);
        if(this.gameObject.activeInHierarchy)
            StartCoroutine(RemoveBuff(effect, duration));
    }

    public virtual IEnumerator RemoveBuff(string effect, float duration)
    {
        yield return new WaitForSeconds(duration);
        buffs.Remove(effect);
        yield break;
    }

    public virtual float BonusBuff()
    {
        float bonus = 0f;
        foreach (var item in buffs.Values)
        {
            bonus += item;
        }
        return bonus;
    }

    public virtual void VisualBuff()
    {
        if(buffs.ContainsKey("Venom"))
        {
            venomFX.SetActive(true);
        }
        else
            venomFX.SetActive(false);
    }

    public virtual void AddDebuff(float debuff, float duration)
    {
        if (this.gameObject.activeInHierarchy)
            StartCoroutine(Debuffer(debuff, duration));
    }

    IEnumerator Debuffer(float debuff, float duration)
    {
        agent.speed /= debuff;
        agent.angularSpeed /= debuff;
        config.dmg /= debuff;

        yield return new WaitForSeconds(0);
        yield return new WaitForSeconds(duration);

        agent.speed *= debuff;
        agent.angularSpeed *= debuff;
        config.dmg *= debuff;

        yield break;
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
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