using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelDragonDevs.SavingLoading;

public class PlayerProfile : MonoBehaviour, ISaveable
{
    public static PlayerProfile instance;

    [SerializeField] public int level;
    [SerializeField] public int exp;
    [SerializeField] public int gold;
    [SerializeField] public int skillPoints;

    public Image progressBar;
    public Text textLevel;
    public Text textExp;
    public int expToNextLevel;

    [SerializeField] public List<int> passivesSkills = new List<int>();
    public List<PlayerSkill> skills = new List<PlayerSkill>(); 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CalculateProgress();
    }

    int CalculateExpToLvl()
    {
        expToNextLevel = 25 * level * (1 + level);
        return expToNextLevel;
    }

    public void CalculateLevelWithExp()
    {
        var levelToXp = (Mathf.Sqrt(625 + 100 * exp)) / 50;
        if(levelToXp > level)
        {
            LevelUp(levelToXp);
            Debug.Log("Subi de nivel");
        }
    }

    public void LevelUp(float _level)
    {
        level = Mathf.RoundToInt(_level);
        exp = 0;
    }

    [ContextMenu("Calculate Progress Bar")]
    public void CalculateProgress()
    {
        //level = CalculateLevelWithExp();
        progressBar.fillAmount = (float)exp / (float)CalculateExpToLvl();
        textLevel.text = level.ToString();
        textExp.text = exp + " / " + CalculateExpToLvl();
    }

    public void AddPassive(int index)
    {
        passivesSkills[index] = 1;
        skills[index].isOwned = true;
        PlayerPassives.instance.plSkills.Add(skills[index]);
    }

    public void RestartPassives()
    {
        foreach (var item in skills)
        {
            if(item.isOwned)
            {
                item.isOwned = false;
            }
        }
        for (int i = 0; i < passivesSkills.Count - 1; i++)
        {
            passivesSkills[i] = 0;
        }
    }

    #region SAVE
    public object CaptureState()
    {
        return new SaveData
        {
            _level = level,
            _exp = exp,
            _gold = gold,
            _passivesSkills = passivesSkills,
            _skillPoints = skillPoints,
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        exp = saveData._exp;
        level = saveData._level;
        gold = saveData._gold;
        passivesSkills = saveData._passivesSkills;
        skillPoints = saveData._skillPoints;
    }

    [Serializable]
    private struct SaveData
    {
        public int _level;
        public int _exp;
        public int _gold;
        public List<int> _passivesSkills;
        public int _skillPoints;
    }
    #endregion
}
