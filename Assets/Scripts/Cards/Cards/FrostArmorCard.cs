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

    public override void TriggerCard(Enemy enemy)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random <= cardSO.chance / 100 && canUse)
        {
            CardEffect(enemy);
        }
        CardEffectTwo(enemy);
    }

    protected override void CardEffect(Enemy enemy)
    {
        enemy.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);
        enemy.GetStuned(cardSO.duration + (cardSO.duration * PlayerPassives.instance.skillAndCardBonus));
    }

    void CardEffectTwo(Enemy enemy)
    {
        enemy.AddDebuff(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), cardSO.duration + (cardSO.duration * PlayerPassives.instance.skillAndCardBonus));
    }

    protected override void CardEffect()
    {
        throw new System.NotImplementedException();
    }
}