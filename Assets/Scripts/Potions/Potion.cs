using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Potion : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnPotionUse = new MyFloatEvent();
    [Header("Potion Info")]
    public string title;
    public Sprite icon;

    public Image potIcon;
    public Image potFill;

    private bool canUse = false;

    public GameObject visual;

    protected PlayerManager pl;
    protected AnimatorHandler anim;

    public float duration;
    public float currentDuration;

    public int fillAmount;

    bool gotUse;
    //public string skillAnimation;

    protected virtual void Awake()
    {
        //skillImage.fillAmount = 0;
        pl = FindObjectOfType<PlayerManager>();
        anim = pl.GetComponentInChildren<AnimatorHandler>();
    }

    public virtual void FillPotion()
    {
        fillAmount++;
        if (fillAmount >= 10)
        {
            gotUse = false;
            canUse = true;
            fillAmount = 10;
            potFill.fillAmount = fillAmount / 10;
        }
    }

    public void TriggerPotion()
    {
        if (canUse)
        {
            //OnPotionUse.Invoke(cooldownTime);
            currentDuration = duration;
            PotionEffect();
            gotUse = true;
            fillAmount = 0;
            canUse = false;
        }
    }

    protected virtual void Update()
    {
        if(duration > 0 && gotUse)
        {
            currentDuration -= Time.deltaTime;
            potIcon.sprite = icon;
            potFill.fillAmount = currentDuration / duration;
        }
        if(gotUse && duration == 0)
        {
            potIcon.sprite = icon;
            potFill.fillAmount = 0;
        }        
    }

    protected virtual void LateUpdate()
    {
    }

    public abstract void PotionEffect();

}
