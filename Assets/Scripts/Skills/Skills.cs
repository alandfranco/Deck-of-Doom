using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skills : MonoBehaviour
{
    public class MyFloatEvent : UnityEvent<float> { }
    public MyFloatEvent OnSkillUse = new MyFloatEvent();
    [Header("Skill Info")]
    public string title;
    public Sprite icon;
    public float cooldownTime;
    private bool canUse = true;
    public GameObject visual;


    public void TriggerAbility()
    {
        if (canUse)
        {
            OnSkillUse.Invoke(cooldownTime);
            Skill();
            StartCooldown();
        }

    }
    public abstract void Skill();
    void StartCooldown()
    {
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            canUse = false;
            yield return new WaitForSeconds(cooldownTime);
            canUse = true;
        }
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