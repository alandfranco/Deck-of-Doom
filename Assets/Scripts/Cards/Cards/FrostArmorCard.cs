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
        enemy.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);
        enemy.GetStuned(cardSO.duration + (cardSO.duration * PlayerPassives.instance.skillAndCardBonus));
    }

    protected override void CardEffect()
    {
        throw new System.NotImplementedException();
    }
}