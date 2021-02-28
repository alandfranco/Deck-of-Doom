using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostArmorCard : Card
{

    protected override void Awake()
    {
        base.Awake();

        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/FrostArmor");
    }

    protected override void CardEffect(Enemy enemy)
    {
        enemy.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg, pl);
        enemy.GetStuned(cardSO.duration);
    }
}