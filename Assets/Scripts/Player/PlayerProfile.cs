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

    public Text gameStatsText;
    public Text pointsText;

    [SerializeField] public List<int> passivesSkills = new List<int>();
    public List<PlayerSkill> skills = new List<PlayerSkill>(); 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CalculateProgress();
        pointsText.text = "Points Available: " + skillPoints;
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
            var extraXp = exp - expToNextLevel;
            LevelUp(levelToXp, extraXp);            
        }
    }

    public void LevelUp(float _level, int extraXP)
    {
        level = Mathf.RoundToInt(_level);
        exp = extraXP > 0 ? extraXP : 0;
        Debug.Log("Subi de nivel " + level);
        skillPoints++;
        CalculateProgress();
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
        if (passivesSkills[index] == 1)
            return;        
        passivesSkills[index] = 1;
        skills[index].isOwned = true;
        PlayerPassives.instance.AddPasive(skills[index]);

        foreach (var item in skills)
        {
            item.CheckIfAvailable();
        }
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

    [ContextMenu("LvlUpDEV")]
    public void LvlUpDEV()
    {
        int xp = CalculateExpToLvl() + 1;
        exp += xp;
        CalculateLevelWithExp();
    }

    public void ShowGameStats()
    {
        GameStats gs = GameStats.instance;
        int currentLvl = level;
        int totalXpWon = gs.expWon;

        while(gs.expWon >= CalculateExpToLvl())
        {
            var xpTo = CalculateExpToLvl();
            Debug.Log(xpTo);
            LvlUpDEV();
            Debug.Log(exp);
            GameStats.instance.expWon -= xpTo;
        }

        exp += GameStats.instance.expWon;

        gameStatsText.text = "Enemies Killed: " + gs.totalEnemiesKilled + "\n" + "Special Enemies Killed: " + gs.specialEnemiesKilled +
            "\n" + "Experience Won: " + totalXpWon;

        if(level > currentLvl)
        {
            gameStatsText.text += "\n" + "New Level Up!" + "\n" + "Now Level " + level + "!";
        }

        Destroy(GameStats.instance.gameObject);
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
