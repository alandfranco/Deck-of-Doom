using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : Enemy
{
    protected override void Awake()
    {
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

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
        goal.Add(new KeyValuePair<string, object>("buffEnemy", true));
        goal.Add(new KeyValuePair<string, object>("stayAlive", true));
        return goal;
    }
}
