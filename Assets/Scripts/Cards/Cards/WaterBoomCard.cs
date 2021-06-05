using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaterBoomCard : Card
{
    float count;
    float timesUsed;

    public float range;
    public float angle;

    protected override void Awake()
    {
        base.Awake();
        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/WaterBoom");
        activateToUse = true;
        count = cardSO.duration;
    }

    private void Update()
    {
        if(isActivated)
        {
            count -= Time.deltaTime;
            if (count <= 0)
            {
                count = cardSO.duration;
                isActivated = false;
            }
        }
    }

    public override void TriggerCard(Enemy enemy)
    {
        if(isActivated)
        {
            if (timesUsed <= cardSO.chance)
            {
                CardEffect(enemy);
                timesUsed++;
            }
            else
            {
                timesUsed = 0;
                isActivated = false;
            }
        }        
    }

    protected override void CardEffect(Enemy enemy)
    {
        foreach (var item in EnemyInFront())
        {
            item.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);
        }
    }

    IEnumerable<Enemy> EnemyInFront()
    {
        var pl = FindObjectOfType<PlayerManager>().transform;
        var enemiesInFront = Physics.OverlapSphere(pl.transform.position, range);

        var _enemiesInFront = enemiesInFront.Where(x => Mathf.Abs(Vector3.Angle(this.transform.forward, x.transform.position - this.transform.position))
        <= angle && !x.GetComponent<PlayerMovement>() && x.GetComponent<Enemy>()).Select(x => x.GetComponent<Enemy>());

        return _enemiesInFront;
    }

    protected override void CardEffect()
    {
    }

    
}
