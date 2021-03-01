using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBodyCard : Card
{
    float count;
    float count2;

    protected override void Awake()
    {
        base.Awake();

        cardSO = Resources.Load<CardSO>("ScriptableObjects/Cards/HealingBody");
    }

    protected override void CardEffect(Enemy enemy)
    {
        
    }

    protected override void CardEffect()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            count += Time.deltaTime;
            if (count >= cardSO.cooldown)
            {
                count2 += Time.deltaTime;
                if (count2 >= 1)
                {
                    pl.GetComponent<TakeDamage>().Heal(cardSO.dmg);
                    count2 = 0;
                }
            }
        }
        else
            count = 0;
    }
}
