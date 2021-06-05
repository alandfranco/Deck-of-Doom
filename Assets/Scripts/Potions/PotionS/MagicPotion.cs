using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPotion : Potion
{
    public float cooldownReduction;
    
    public override void PotionEffect()
    {
        //visual.SetActive(false);
        StartCoroutine(CooldownCourutine());
    }

    IEnumerator CooldownCourutine()
    {
        List<float> originalCD = new List<float>();
        foreach (var item in pl.GetComponent<SkillManager>().skills)
        {
            originalCD.Add(item.cooldownTime);
            item.cooldownTime /= cooldownReduction;
            if (item.cooldownTime < item.currentCooldown)
            {
                item.RecallCooldown();
            }
        }
        yield return new WaitForSeconds(0);

        yield return new WaitForSeconds(duration);
        for (int i = 0; i < originalCD.Count - 1; i++)
        {
            pl.GetComponent<SkillManager>().skills[i].cooldownTime = originalCD[i];
        }
        //pl.DeactivatePotionUI();        
        yield break;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
