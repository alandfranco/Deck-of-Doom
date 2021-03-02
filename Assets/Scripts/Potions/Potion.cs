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

    private bool canUse = true;

    public GameObject visual;

    protected PlayerManager pl;
    protected AnimatorHandler anim;

    public float duration;
    public float currentDuration;

    bool gotUse;
    //public string skillAnimation;

    protected virtual void Awake()
    {
        //skillImage.fillAmount = 0;
        pl = FindObjectOfType<PlayerManager>();
        anim = pl.GetComponentInChildren<AnimatorHandler>();
    }

    public void TriggerPotion()
    {
        if (canUse)
        {
            //OnPotionUse.Invoke(cooldownTime);
            currentDuration = duration;
            PotionEffect();
            gotUse = true;
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
    }

    protected virtual void LateUpdate()
    {
    }

    public abstract void PotionEffect();

}
