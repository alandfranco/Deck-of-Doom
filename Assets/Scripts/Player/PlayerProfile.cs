using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelDragonDevs.SavingLoading;

public class PlayerProfile : MonoBehaviour, ISaveable
{
    [SerializeField] public int level;
    [SerializeField] public int exp;
    [SerializeField] public int gold;

    public Image progressBar;

    public int expToNextLevel;

    private void Start()
    {
        CalculateProgressBar();
    }

    int CalculateExpToLvl()
    {
        expToNextLevel = 25 * level * (1 + level);
        return expToNextLevel;
    }

    int CalculateLevelWithExp()
    {
        var levelToXp = (Mathf.Sqrt(625 + 100 * exp)) / 50;
        return Mathf.RoundToInt(levelToXp);
    }

    [ContextMenu("Calculate Progress Bar")]
    public void CalculateProgressBar()
    {
        level = CalculateLevelWithExp();
        progressBar.fillAmount = (float)exp / (float)CalculateExpToLvl();
    }

    #region SAVE
    public object CaptureState()
    {
        return new SaveData
        {
            _level = level,
            _exp = exp,
            _gold = gold,
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        exp = saveData._exp;
        level = saveData._level;
        gold = saveData._gold;
    }

    [Serializable]
    private struct SaveData
    {
        public int _level;
        public int _exp;
        public int _gold;
    }
    #endregion
}
