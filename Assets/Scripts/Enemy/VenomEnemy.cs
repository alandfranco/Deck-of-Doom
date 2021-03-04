using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        SetStartingValues(Resources.Load<ScriptableObject>("ScriptableObjects/VenomEnemy"));
    }

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
        goal.Add(new KeyValuePair<string, object>("buffEnemy", true));
        goal.Add(new KeyValuePair<string, object>("stayAlive", true));
        return goal;
    }

    public override void PassiveRegen()
    {
        stamina += config.staminaRegen * Time.deltaTime;
    }

    void Start()
    {

    }
}
