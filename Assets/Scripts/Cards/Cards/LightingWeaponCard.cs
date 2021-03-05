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
        EnemyInFront().GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);
        //Lighting effect over enemy
    }

    Enemy EnemyInFront()
    {
        var enemiesInFront = FindObjectsOfType<Enemy>();

        var _enemiesInFront = enemiesInFront.Where(x => Mathf.Abs(Vector3.Angle(cam.forward, x.transform.position - cam.position))
        <= 90 && !x.GetComponent<PlayerMovement>() && x.GetComponent<Enemy>()).OrderBy(x => Random.value).Select(x => x.GetComponent<Enemy>()).First();
        return _enemiesInFront;
    }

    protected override void CardEffect()
    {
    }
}
