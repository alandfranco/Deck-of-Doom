using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassives : MonoBehaviour
{
    public static PlayerPassives instance;

    public List<PlayerSkill> plSkills = new List<PlayerSkill>();

    public float healthBonus;
    public float damageBonus;
    public float staminaBonus;
    public float skillAndCardBonus;
    public float cooldownBonus;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void AddPasive(PlayerSkill skill)
    {
        if (!plSkills.Contains(skill))
        {
            plSkills.Add(skill);
            ResetBonus();
            CalculateBonus();
            PlayerProfile.instance.skillPoints--;
            PlayerProfile.instance.pointsText.text = "Points Available: " + PlayerProfile.instance.skillPoints;
        }            
    }

    public void CalculateBonus()
    {
        foreach (var item in plSkills)
        {
            if(item.isOwned)
            {
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.health))
                {
                    healthBonus += item.multiplier / 100f;
                }
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.damage))
                {
                    damageBonus += item.multiplier / 100f;
                }
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.skill))
                {
                    skillAndCardBonus += item.multiplier / 100f;
                }
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.stamina))
                {
                    staminaBonus += item.multiplier / 100f;
                }
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.cooldown))
                {
                    cooldownBonus += item.multiplier / 100f;
                }
            }            
        }
    }

    void ResetBonus()
    {
        healthBonus = 0;
        damageBonus = 0;
        skillAndCardBonus = 0;
        staminaBonus = 0;
        cooldownBonus = 0;
    }
}