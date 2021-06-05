using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomingShieldCard : Card
{
    TakeDamage plTD;

    protected override void Awake()
    {
        base.Awake();
        plTD = pl.GetComponent<TakeDamage>();
        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/BoomingShield");
    }

    void Start()
    {

    }

    public override void TriggerCard(Enemy enemy)
    {
        if(plTD.isBlocking)
        {
            CardEffect(enemy);
        }
    }

    protected override void CardEffect(Enemy enemy)
    {
        foreach (var item in EnemiesInFront(enemy))
        {
            if (!item.GetComponent<Enemy>())
                continue;
            item.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);

            //effect
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
