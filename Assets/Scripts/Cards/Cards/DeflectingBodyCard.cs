using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectingBodyCard : Card
{

    protected override void Awake()
    {
        base.Awake();

        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/DeflectingBody");
    }

    protected override void CardEffect(Enemy enemy)
    {
        enemy.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);
        enemy.GetPush();
    }

    protected override void CardEffect()
    {
        throw new System.NotImplementedException();
    }
}
