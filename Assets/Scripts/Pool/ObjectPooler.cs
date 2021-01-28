using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    public List<GameObject> objectToPool = new List<GameObject>();
    public int pooledAmountToStart;
    public bool willGrow;
    public List<GameObject> pooledObjects = new List<GameObject>();

    void Awake()
    {
        instance = this;
        for (int i = 0; i < pooledAmountToStart; i++)
        {
            foreach (var item in objectToPool)
            {
                GameObject obj = Instantiate(item);
                obj.name = item.name;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }            
        }
    }

    public List<GameObject> GetPooledObject(GameObject _obj, int amountRequired)
    {
        var _objetcToPool = pooledObjects.Where(x => x.name == _obj.name).ToList();
        List<GameObject> objectsToReturn = new List<GameObject>();
        for (int i = 0; i < _objetcToPool.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                objectsToReturn.Add(_objetcToPool[i]);
            }
        }

        if (objectsToReturn.Count < amountRequired)
        {
            var amount = amountRequired - objectsToReturn.Count;            
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = Instantiate(_obj);
                obj.name = _obj.name;
                objectsToReturn.Add(obj);
            }                        
        }

        return objectsToReturn;
    }

    public GameObject GetPooledObject(GameObject _obj)
    {
        var _objetcToPool = pooledObjects.Where(x => x.name == _obj.name && !x.activeInHierarchy).FirstOrDefault();
        
        if (_objetcToPool == null)
        {
            GameObject obj = Instantiate(_obj);
            obj.name = _obj.name;
            _objetcToPool = obj;
        }

        return _objetcToPool;
    }
}
