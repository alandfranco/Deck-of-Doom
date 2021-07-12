using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager instance;

    public List<GameObject> enemyTargets = new List<GameObject>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GameObject GetTarget()
    {
        return enemyTargets[Random.Range(0, enemyTargets.Count)];
    }
}