using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Skills : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnSkillUse = new MyFloatEvent();
    [Header("Skill Info")]
    public string title;
    public Sprite icon;
    public float cooldownTime;
    public float currentCooldown;
    protected bool canUse = true;
    public GameObject visual;

    protected PlayerManager pl;
    protected AnimatorHandler anim;

    public string skillAnimation;

    protected virtual void Awake()
    {
        //skillImage.fillAmount = 0;
        pl = FindObjectOfType<PlayerManager>();
        anim = pl.GetComponentInChildren<AnimatorHandler>();
    }

    public virtual void TriggerAbility()
    {
        if (canUse)
        {
            OnSkillUse.Invoke(cooldownTime);
            Skill();
            StartCooldown();
            //anim.PlayTargetAnimation(skillAnimation, true, 0.2f);
            pl.anim.Play(skillAnimation);
        }
    }

    protected virtual void LateUpdate()
    {
        if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0.1f)
                currentCooldown = 0;
        }
        //skillImage.fillAmount = currentCooldown / cooldownTime;
    }

    public abstract void Skill();
    protected virtual void StartCooldown()
    {
        currentCooldown = cooldownTime;
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            canUse = false;
            yield return new WaitForSeconds(cooldownTime);            
            canUse = true;
        }
    }

    public void RecallCooldown()
    {
        StartCooldown();
    }

    protected virtual void Deactivate()
    {        
        visual.SetActive(false);
    }

    protected virtual void Activate()
    {
        visual.SetActive(true);
    }
}