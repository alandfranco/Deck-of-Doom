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
        enemy.GetComponent<TakeDamage>().TakeDamageToHealth(cardSO.dmg, pl);
        enemy.AddDebuff(cardSO.dmg, cardSO.duration);
        //enemy.PlayVenom();
    }
}