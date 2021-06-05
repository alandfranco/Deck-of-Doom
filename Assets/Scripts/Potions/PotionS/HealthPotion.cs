using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    public float healingAmountOnPorcentage;

    public override void PotionEffect()
    {
        pl.TryGetComponent<TakeDamage>(out var x);
        x.Heal(x.maxHealth * healingAmountOnPorcentage / 100);
        //Destroy(this.gameObject);
    }
}
