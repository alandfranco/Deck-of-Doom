using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxAmountEnemies;

    public int enemiesAttacking;
    public int maxAmountAttacking;

    private void Awake()
    {
        instance = this;
        QualitySettings.vSyncCount = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(enemiesAttacking);
    }

    public void RestartScene()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
