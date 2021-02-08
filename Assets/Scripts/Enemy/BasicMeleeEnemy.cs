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
}
