using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeflectingBodyCard : Card
{
    float count;

    public float range;    

    protected override void Awake()
    {
        base.Awake();

        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/DeflectingBody");
    }

    private void Update()
    {
        if (count > 0)
        {
            count -= Time.deltaTime;
        }
        else
            count = 0;
    }

    public override void TriggerCard(Enemy enemy)
    {
        if(count == 0)
        {
            CardEffect(enemy);
            count = cardSO.cooldown;
        }
    }

    protected override void CardEffect(Enemy enemy)
    {
        foreach (var item in EnemiesInFront())
        {
            if (!item.GetComponent<Enemy>())
                continue;
            //enemy.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);
            enemy.GetPush();
        }        
    }

    Collider[] EnemiesInFront()
    {
        var pl = FindObjectOfType<PlayerManager>().transform;
        var enemiesInFront = Physics.OverlapSphere(pl.transform.position, range);

        return enemiesInFront;
    }

    protected override void CardEffect()
    {
        throw new System.NotImplementedException();
    }
}
