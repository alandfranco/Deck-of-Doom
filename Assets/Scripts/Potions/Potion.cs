using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Potion : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnPotionUse = new MyFloatEvent();
    [Header("Potion Info")]
    public string title;
    public Sprite icon;

    private bool canUse = true;

    public GameObject visual;

    protected PlayerManager pl;
    protected AnimatorHandler anim;

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
            PotionEffect();
        }
    }

    protected virtual void LateUpdate()
    {
    }

    public abstract void PotionEffect();

}
