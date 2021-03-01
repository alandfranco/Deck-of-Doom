using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    public BasicMeleeAction myAction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerManager>())
        {
            myAction.DealDamage(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}
