using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightingWeaponCard : Card
{
    float count;

    Transform cam;
        
    protected override void Awake()
    {
        base.Awake();
        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/LightingWeapon");
        cam = Camera.main.transform;
    }

    public override void TriggerCard(Enemy enemy)
    {
        count++;
        if (count >= cardSO.cooldown)
            CardEffect(enemy);
    }

    protected override void CardEffect(Enemy enemy)
    {
        foreach (var item in EnemiesInFront(enemy))
        {
            if (!item.GetComponent<Enemy>())
                continue;
            item.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);
            
            //Lighting effect over enemy
        }
    }

    Collider[] EnemiesInFront(Enemy enemy)
    {        
        var enemiesInFront = Physics.OverlapSphere(enemy.transform.position, 3);

        return enemiesInFront;
    }

    protected override void CardEffect()
    {
    }
}
