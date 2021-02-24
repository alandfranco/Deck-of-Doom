﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public CardSO cardSO;

    protected GameObject pl;

    protected bool canUse;

    protected virtual void Awake()
    {
        pl = FindObjectOfType<PlayerMovement>().gameObject;
        canUse = true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void TriggerCard(Enemy enemy)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random <= cardSO.chance / 100 && canUse)
            CardEffect(enemy);
    }

    protected abstract void CardEffect(Enemy enemy);

    protected virtual IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cardSO.cooldown);
        canUse = true;
    }
}
