using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomWeaponCard : Card
{
    protected override void Awake()
    {
        base.Awake();
        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/VenomWeapon");
    }

    void Start()
    {

    }

    void Update()
    {

    }

    protected override void CardEffect(Enemy enemy)
    {
        enemy.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), pl);
        enemy.AddDebuff(cardSO.dmg + (cardSO.dmg * PlayerPassives.instance.skillAndCardBonus), cardSO.duration + (cardSO.duration * PlayerPassives.instance.skillAndCardBonus));
        //enemy.PlayVenom();
    }

    protected override void CardEffect()
    {
    }
}