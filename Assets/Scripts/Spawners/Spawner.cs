using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public List<Transform> spawners = new List<Transform>();

    public int amountToSpawn;
    int currentSpawned = 0;

    public GameObject objectToSpawn;

    void Start()
    {
        StartSpawning();
    }
        
    void Update()
    {
        
    }

    void StartSpawning()
    {
        currentSpawned = 0;
        var obj = ObjectPooler.instance.GetPooledObject(objectToSpawn, amountToSpawn);
        StartCoroutine(Spawn(obj));
    }

    IEnumerator Spawn(List<GameObject> obj)
    {        
        while (currentSpawned < amountToSpawn)
        {
            obj[currentSpawned].transform.position = this.transform.position;
            obj[currentSpawned].SetActive(true);
            currentSpawned++;            
            yield return new WaitForSeconds(0f);
            yield return new WaitForSeconds(0.5f);
        }
        if (currentSpawned == amountToSpawn)
            StopAllCoroutines();
    }
}
