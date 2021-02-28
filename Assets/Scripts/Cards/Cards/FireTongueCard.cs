using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTongueCard : Card
{
    protected override void Awake()
    {
        base.Awake();
        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/FireTongue");
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
        enemy.GetComponent<TakeDamage>().TakeDamageOvertime(cardSO.duration, cardSO.dmgOvertime, pl);
        //enemy.PlayFire();
    }
}
