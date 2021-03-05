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
        plSkills.Add(skill);
    }

    public void CalculateBonus()
    {
        foreach (var item in plSkills)
        {
            if(item.isOwned)
            {
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.health))
                {
                    healthBonus += item.multiplier;
                }
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.damage))
                {
                    damageBonus += item.multiplier;
                }
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.skill))
                {
                    skillAndCardBonus += item.multiplier;
                }
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.stamina))
                {
                    staminaBonus += item.multiplier;
                }
                if (item.typeBonus.Contains(PlayerSkill.TypeBonus.cooldown))
                {
                    cooldownBonus += item.multiplier;
                }
            }            
        }
    }
}