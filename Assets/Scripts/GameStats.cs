using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats instance;

    public int simpleEnemiesKilled;
    public int specialEnemiesKilled;
    public int totalEnemiesKilled;
    public int expWon;

    public bool justFinishedGame;

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

    public void ShowGameStats()
    {
        //PlayerProfile.instance.ShowGameStats();
    }

    public void AddExp(int exp)
    {
        expWon += exp;
    }
}