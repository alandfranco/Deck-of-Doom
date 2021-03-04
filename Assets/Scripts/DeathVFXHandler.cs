using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVFXHandler : MonoBehaviour
{
    public GameObject deathParticles;
    public Transform vfxContainer;

    void Start()
    {
        
    }
        
    void Update()
    {
        
    }

    public void Die()
    {
        var obj = ObjectPooler.instance.GetPooledObject(deathParticles, vfxContainer.position);        
    }
}
