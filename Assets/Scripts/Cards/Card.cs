using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public CardSO cardSO;

    protected GameObject pl;

    protected bool canUse;

    public bool activateToUse;
    protected bool isActivated;
    
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

    public virtual void CanUseCard()
    {
        if (activateToUse)
        {
            isActivated = true;
        }
    }

    public virtual void TriggerCard(Enemy enemy)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random <= cardSO.chance / 100 && canUse)
        {
            CardEffect(enemy);
        }
    }

    public virtual void TriggerCard()
    {
        CardEffect();        
    }

    protected abstract void CardEffect(Enemy enemy);
    protected abstract void CardEffect();

    protected virtual IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cardSO.cooldown);
        canUse = true;
    }
}
